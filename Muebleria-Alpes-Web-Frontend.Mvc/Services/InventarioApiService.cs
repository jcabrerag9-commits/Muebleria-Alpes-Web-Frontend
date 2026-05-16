using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Inventario;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class InventarioApiService
    {
        private readonly HttpClient _httpClient;
        public string? LastErrorMessage { get; private set; }

        public InventarioApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExistenciaViewModel>> ObtenerExistenciasAsync(int productoId)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<InventarioApiResponse<List<ExistenciaViewModel>>>($"api/inventario/existencia/{productoId}");
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<List<ReservaViewModel>> ObtenerReservasAsync(int productoId)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<InventarioApiResponse<List<ReservaViewModel>>>($"api/inventario/reservas/{productoId}");
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<List<KardexMovimientoViewModel>> ObtenerKardexAsync(int productoId)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<InventarioApiResponse<List<KardexMovimientoViewModel>>>($"api/inventario/kardex/{productoId}");
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<List<BodegaViewModel>> ObtenerBodegasAsync() => await ListarBodegasAsync();

        public async Task<List<BodegaViewModel>> ListarBodegasAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<InventarioApiResponse<List<BodegaViewModel>>>("api/inventario/bodegas");
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<bool> RegistrarEntradaAsync(MovimientoInventarioViewModel model)
        {
            LastErrorMessage = null;
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/inventario/entrada", model);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                    LastErrorMessage = error?.Mensaje ?? $"Error {(int)response.StatusCode}";
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) { LastErrorMessage = ex.Message; return false; }
        }

        public async Task<bool> RegistrarSalidaAsync(MovimientoInventarioViewModel model)
        {
            LastErrorMessage = null;
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/inventario/salida", model);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                    LastErrorMessage = error?.Mensaje ?? $"Error {(int)response.StatusCode}";
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) { LastErrorMessage = ex.Message; return false; }
        }

        public async Task<bool> ReservarStockAsync(ReservaStockViewModel model)
        {
            LastErrorMessage = null;
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/inventario/reservar", model);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                    LastErrorMessage = error?.Mensaje ?? $"Error {(int)response.StatusCode}";
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) { LastErrorMessage = ex.Message; return false; }
        }

        public async Task<(bool success, string message)> LiberarReservaAsync(int reservaId)
        {
            try
            {
                // La API real usa "liberar/{id}" según InventarioController.cs:152
                var response = await _httpClient.DeleteAsync($"api/inventario/liberar/{reservaId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                    return (true, result?.Mensaje ?? "Reserva liberada con éxito");
                }
                else
                {
                    // Manejo seguro: si no es JSON o falla, capturamos el error sin romper el flujo
                    try 
                    {
                        var errorResult = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                        return (false, errorResult?.Mensaje ?? $"Error {(int)response.StatusCode}");
                    }
                    catch 
                    {
                        return (false, $"Error del servidor ({(int)response.StatusCode}): {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex) 
            { 
                return (false, $"Error de comunicación: {ex.Message}"); 
            }
        }

        public async Task<bool> CrearBodegaAsync(BodegaViewModel model)
        {
            LastErrorMessage = null;
            try
            {
                // El backend real tiene BodegaController en api/Bodega
                var response = await _httpClient.PostAsJsonAsync("api/Bodega", model);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                    LastErrorMessage = error?.Mensaje ?? $"Error {(int)response.StatusCode}";
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) { LastErrorMessage = ex.Message; return false; }
        }

        public async Task<bool> ActualizarBodegaAsync(BodegaViewModel model)
        {
            LastErrorMessage = null;
            try
            {
                // El backend real tiene BodegaController en api/Bodega/{id}
                var response = await _httpClient.PutAsJsonAsync($"api/Bodega/{model.BodegaId}", model);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                    LastErrorMessage = error?.Mensaje ?? $"Error {(int)response.StatusCode}";
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) { LastErrorMessage = ex.Message; return false; }
        }

        public async Task<bool> InactivarBodegaAsync(int id)
        {
            LastErrorMessage = null;
            try
            {
                // El backend real usa PATCH para cambiar estado (api/Bodega/{id}/estado)
                // Inactivar es poner estado=INACTIVO
                var url = $"api/Bodega/{id}/estado?estado=INACTIVO&motivo=Inactivacion desde Panel Administrativo&usuarioId=1";
                var response = await _httpClient.PatchAsync(url, null);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                    LastErrorMessage = error?.Mensaje ?? $"Error {(int)response.StatusCode}";
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) { LastErrorMessage = ex.Message; return false; }
        }

        public async Task<List<KardexMovimientoViewModel>> ObtenerMovimientosGlobalesAsync(int? bodegaId, string? desde, string? hasta, string? tipo)
        {
            try
            {
                var url = $"api/inventario/movimientos?bodegaId={bodegaId}&desde={desde}&hasta={hasta}&tipo={tipo}";
                var result = await _httpClient.GetFromJsonAsync<InventarioApiResponse<List<KardexMovimientoViewModel>>>(url);
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<InventarioDashboardViewModel?> ObtenerDashboardAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<InventarioApiResponse<InventarioDashboardViewModel>>("api/inventario/dashboard");
                return result?.Data;
            }
            catch { return null; }
        }
    }

    public class InventarioApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        
        [JsonPropertyName("resultado")]
        public string? Resultado { get; set; }
        
        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }
        
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
