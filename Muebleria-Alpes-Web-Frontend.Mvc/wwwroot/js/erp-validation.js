/**
 * ERP Production Ready - Validation & UX Helper
 * Mueblería Los Alpes
 * Centraliza validaciones de formularios y prevención de doble submit.
 */

const ERPValidation = {
    /**
     * Previene que un formulario se envíe varias veces.
     * @param {string} formSelector Selector del formulario.
     * @param {Function} submitFn Función async que realiza la acción.
     */
    preventDoubleSubmit: function(formSelector, submitFn) {
        const form = document.querySelector(formSelector);
        if (!form) return;

        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            const btn = form.querySelector('button[type="submit"]');
            
            if (btn && btn.disabled) return;

            if (btn) {
                btn.disabled = true;
                const originalText = btn.innerHTML;
                btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span>';
                
                try {
                    await submitFn(e);
                } catch (err) {
                    console.error('[Validation Submit Error]', err);
                } finally {
                    btn.disabled = false;
                    btn.innerHTML = originalText;
                }
            } else {
                await submitFn(e);
            }
        });
    },

    /**
     * Configura inputs monetarios para solo permitir números y decimales.
     */
    setupMonetaryInputs: function() {
        const inputs = document.querySelectorAll('.erp-currency-input');
        inputs.forEach(input => {
            input.addEventListener('input', (e) => {
                e.target.value = e.target.value.replace(/[^0-9.]/g, '');
                const dots = e.target.value.split('.').length - 1;
                if (dots > 1) {
                    e.target.value = e.target.value.substring(0, e.target.value.lastIndexOf('.'));
                }
            });
        });
    },

    /**
     * Validación de rangos de fecha.
     */
    isValidDateRange: function(startDate, endDate) {
        if (!startDate || !endDate) return true;
        const start = new Date(startDate);
        const end = new Date(endDate);
        if (end < start) {
            ERPHelpers.showToast('La fecha final no puede ser anterior a la fecha inicial', 'warning');
            return false;
        }
        return true;
    }
};

// Auto-inicialización de listeners globales
document.addEventListener('DOMContentLoaded', () => {
    ERPValidation.setupMonetaryInputs();
});

window.ERPValidation = ERPValidation;
