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
                var response = await _httpClient.DeleteAsync($"api/inventario/reservar/{reservaId}");
                var result = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                return (response.IsSuccessStatusCode, result?.Mensaje ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<bool> CrearBodegaAsync(BodegaViewModel model)
        {
            LastErrorMessage = null;
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/inventario/bodegas", model);
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
                var response = await _httpClient.PutAsJsonAsync($"api/inventario/bodegas/{model.BodegaId}", model);
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
                var response = await _httpClient.DeleteAsync($"api/inventario/bodegas/{id}");
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
        [JsonPropertyName("resultado")]
        public bool Resultado { get; set; }
        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
