/**
 * ERP Helpers - Mueblería Los Alpes
 * Estandarización de Fetch, Manejo de Errores Oracle y Loaders Globales.
 * Versión H.4 - Estabilización de Integración Real.
 */

const ERPHelpers = {
    // URL base de la API obtenida de la configuración global inyectada en _Layout.cshtml
    apiBaseUrl: window.erpConfig ? window.erpConfig.apiBaseUrl : '/api',

    /**
     * Fetch estandarizado con manejo de errores ERP, timeout y logging
     */
    async fetchERP(endpoint, options = {}, timeoutMs = 15000) {
        let url = endpoint.startsWith('http') ? endpoint : `${this.apiBaseUrl}${endpoint}`;
        
        const defaultHeaders = {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        };

        options.headers = { ...defaultHeaders, ...options.headers };

        const controller = new AbortController();
        const id = setTimeout(() => controller.abort(), timeoutMs);
        options.signal = controller.signal;

        try {
            console.log(`[ERP API Request] ${options.method || 'GET'} ${url}`);
            this.showLoader();
            
            const response = await fetch(url, options);
            clearTimeout(id);
            
            console.log(`[ERP API Response] ${options.method || 'GET'} ${url} - Status: ${response.status}`);
            console.log("FETCH STATUS:", response.status);

            const rawText = await response.text();
            let rawData = {};
            try {
                rawData = JSON.parse(rawText);
            } catch (e) {
                console.error(`[ERP API Invalid JSON]`, rawText);
                if (!response.ok) {
                    // Si no es OK y no es JSON, probablemente es una página de error HTML (404/500)
                    console.error(`[ERP RAW ERROR RESPONSE]`, rawText);
                    this.showToast('Error crítico del servidor. Ver consola.', 'error');
                    throw new Error(rawText);
                }
            }

            const normalized = this.handleApiResponse(rawData);
            
            if (!response.ok || !normalized.success) {
                console.error(`[ERP API Failed]`, normalized);
                console.error(`[ERP API Raw Body]`, rawText);
                
                this.showApiError(normalized);
                throw normalized;
            }

            return normalized;
        } catch (error) {
            clearTimeout(id);
            if (error.name === 'AbortError') {
                console.error('[ERP API Timeout]', `La petición a ${url} superó el límite de ${timeoutMs}ms`);
                this.showToast('Tiempo de espera agotado. Por favor, intente nuevamente.', 'error');
                throw { success: false, message: 'Timeout' };
            }
            if (error.message === 'Failed to fetch' || error instanceof TypeError) {
                console.error('[ERP API Network Error]', `No se pudo conectar a ${url}. Verifique CORS o si el API está corriendo.`);
                this.showToast('Error de red. No se pudo conectar al ERP.', 'error');
                throw { success: false, message: 'Network Error' };
            }
            
            throw error;
        } finally {
            this.hideLoader();
        }
    },

    /**
     * Normaliza respuestas del API bajo el contrato H.5
     */
    handleApiResponse(raw) {
        // Validación Defensiva H.5: Detectar el colapso de contrato StartArray -> String
        if (raw.resultado && Array.isArray(raw.resultado)) {
            console.error("❌ ERROR CRÍTICO DE CONTRATO: El servidor envió un Array en 'resultado'. Se esperaba un String (OK/EXITO/RECHAZADO).", raw);
            // Intento de auto-recuperación: si 'resultado' es el array, lo movemos a 'data'
            if (raw.data === undefined) {
                raw.data = raw.resultado;
                raw.resultado = raw.success ? "OK" : "ERROR";
            }
        }

        const success = raw.success === true;
        const message = raw.message || raw.mensaje || '';
        const data = raw.data !== undefined ? raw.data : null;
        const errores = raw.errores || [];
        const status = raw.resultado === "EXITO" ? "OK" : (raw.resultado || (success ? "OK" : "ERROR"));

        return { success, message, data, status, errores };
    },

    /**
     * Muestra errores de la API de forma amigable
     */
    showApiError(error) {
        const msg = error?.message || error?.mensaje || JSON.stringify(error);
        this.showToast(msg, 'error');
    },

    /**
     * Renderizado genérico de tablas para módulos SPA (H.4)
     */
    renderTable(data, tableBodyId, rowTemplateFn) {
        const tbody = document.getElementById(tableBodyId);
        if (!tbody) return;

        if (!data || data.length === 0) {
            tbody.innerHTML = '<tr><td colspan="100%" class="text-center py-4 text-muted">No se encontraron registros.</td></tr>';
            return;
        }

        tbody.innerHTML = data.map(rowTemplateFn).join('');
    },

    /**
     * Mostrar Toast de notificación
     */
    showToast(message, type = 'success') {
        if (window.Swal) {
            Swal.fire({
                icon: type,
                title: type === 'error' ? 'Error ERP' : 'Éxito',
                text: message,
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000
            });
        } else {
            const toastEl = document.getElementById('erpToast');
            const toastMsg = document.getElementById('erpToastMessage');
            if (toastEl && toastMsg) {
                toastEl.className = `toast align-items-center text-white border-0 ${type === 'error' ? 'bg-danger' : (type === 'warning' ? 'bg-warning' : 'bg-success')}`;
                toastMsg.textContent = message;
                const bsToast = bootstrap.Toast.getOrCreateInstance(toastEl);
                bsToast.show();
            } else {
                console.log(`[${type.toUpperCase()}] ${message}`);
            }
        }
    },

    showLoader() {
        const loader = document.getElementById('global-loader');
        if (loader) loader.style.display = 'flex';
    },

    hideLoader() {
        const loader = document.getElementById('global-loader');
        if (loader) loader.style.display = 'none';
    },

    formatCurrency(amount) {
        return new Intl.NumberFormat('es-GT', {
            style: 'currency',
            currency: 'GTQ'
        }).format(amount || 0);
    }
};

window.ERPHelpers = ERPHelpers;
