using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Inventario;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Shared;
using System.Net.Http.Json;
using System.Text.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.Inventario
{
    public class InventarioApiService
    {
        private readonly HttpClient _httpClient;
        /// <summary>Último mensaje de error recibido del backend. Se restablece en cada llamada exitosa.</summary>
        public string? LastErrorMessage { get; private set; }

        public InventarioApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExistenciaViewModel>> ObtenerExistenciasAsync(int productoId)
        {
            try 
            {
                var response = await _httpClient.GetFromJsonAsync<BackendResponse<List<ExistenciaViewModel>>>($"api/Inventario/existencia/{productoId}");
                return response?.Data ?? new List<ExistenciaViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API ERROR] ObtenerExistenciasAsync: {ex.Message}");
                return new();
            }
        }

        public async Task<List<ReservaViewModel>> ObtenerReservasAsync(int productoId)
        {
            var response = await _httpClient.GetAsync($"api/Inventario/reservas/{productoId}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener reservas: {response.StatusCode} - {errorContent}");
            }
            var result = await response.Content.ReadFromJsonAsync<BackendResponse<List<ReservaViewModel>>>();
            return result?.Data ?? new List<ReservaViewModel>();
        }

        public async Task<List<BodegaViewModel>> ObtenerBodegasAsync()
        {
            var response = await _httpClient.GetAsync("api/Bodega?soloActivas=true");
            if (!response.IsSuccessStatusCode) return new();
            var result = await response.Content.ReadFromJsonAsync<BackendResponse<List<BodegaViewModel>>>();
            return result?.Data ?? new();
        }

        public async Task<bool> RegistrarEntradaAsync(MovimientoInventarioViewModel model)
        {
            LastErrorMessage = null;
            Console.WriteLine($"[API SERVICE] Enviando POST api/Inventario/entrada: {JsonSerializer.Serialize(model)}");
            var response = await _httpClient.PostAsJsonAsync("api/Inventario/entrada", model);
            Console.WriteLine($"[API SERVICE] Respuesta Entrada: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[API SERVICE ERROR] Entrada: {errorBody}");
                LastErrorMessage = ExtraerMensajeError(errorBody);
                return false;
            }
            var result = await response.Content.ReadFromJsonAsync<BackendResponse<object>>();
            return result?.IsSuccess ?? false;
        }

        public async Task<bool> RegistrarSalidaAsync(MovimientoInventarioViewModel model)
        {
            LastErrorMessage = null;
            Console.WriteLine($"[API SERVICE] Enviando POST api/Inventario/salida: {JsonSerializer.Serialize(model)}");
            var response = await _httpClient.PostAsJsonAsync("api/Inventario/salida", model);
            Console.WriteLine($"[API SERVICE] Respuesta Salida: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[API SERVICE ERROR] Salida: {errorBody}");
                LastErrorMessage = ExtraerMensajeError(errorBody);
                return false;
            }
            var result = await response.Content.ReadFromJsonAsync<BackendResponse<object>>();
            return result?.IsSuccess ?? false;
        }

        public async Task<bool> ReservarStockAsync(ReservaStockViewModel model)
        {
            LastErrorMessage = null;
            Console.WriteLine($"[API SERVICE] Enviando POST api/Inventario/reservar: {JsonSerializer.Serialize(model)}");
            var response = await _httpClient.PostAsJsonAsync("api/Inventario/reservar", model);
            Console.WriteLine($"[API SERVICE] Respuesta Reserva: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[API SERVICE ERROR] Reserva: {errorBody}");
                LastErrorMessage = ExtraerMensajeError(errorBody);
                return false;
            }
            var result = await response.Content.ReadFromJsonAsync<BackendResponse<object>>();
            return result?.IsSuccess ?? false;
        }

        public async Task<(bool success, string message)> LiberarReservaAsync(int reservaId)
        {
            var response = await _httpClient.DeleteAsync($"api/Inventario/liberar/{reservaId}");
            var result = await response.Content.ReadFromJsonAsync<BackendResponse<object>>();
            return (response.IsSuccessStatusCode && (result?.IsSuccess ?? false), result?.Mensaje ?? "Error desconocido en el servidor");
        }

        public async Task<List<KardexMovimientoViewModel>> ObtenerKardexAsync(int productoId)
        {
            try 
            {
                var response = await _httpClient.GetFromJsonAsync<BackendResponse<List<KardexMovimientoViewModel>>>($"api/Inventario/kardex/{productoId}");
                return response?.Data ?? new List<KardexMovimientoViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API ERROR] ObtenerKardexAsync: {ex.Message}");
                return new();
            }
        }

        public async Task<bool> InactivarBodegaAsync(int bodegaId)
        {
            var response = await _httpClient.PatchAsync($"api/Bodega/{bodegaId}/estado?estado=INACTIVO&motivo=Cierre administrativo", null);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                LastErrorMessage = ExtraerMensajeError(body);
                Console.WriteLine($"[API SERVICE ERROR] InactivarBodega: {body}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<List<BodegaViewModel>> ListarBodegasAsync(bool soloActivas = false)
        {
            try 
            {
                var response = await _httpClient.GetFromJsonAsync<BackendResponse<List<BodegaViewModel>>>($"api/Bodega?soloActivas={soloActivas}");
                return response?.Data ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API ERROR] ListarBodegasAsync: {ex.Message}");
                return new();
            }
        }

        public async Task<bool> CrearBodegaAsync(BodegaViewModel bodega)
        {
            LastErrorMessage = null;
            Console.WriteLine($"[API SERVICE] Enviando POST api/Bodega: {JsonSerializer.Serialize(bodega)}");
            var response = await _httpClient.PostAsJsonAsync("api/Bodega", bodega);
            Console.WriteLine($"[API SERVICE] Respuesta CrearBodega: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                LastErrorMessage = ExtraerMensajeError(body);
                Console.WriteLine($"[API SERVICE ERROR] CrearBodega: {body}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarBodegaAsync(BodegaViewModel bodega)
        {
            LastErrorMessage = null;
            Console.WriteLine($"[API SERVICE] Enviando PUT api/Bodega/{bodega.BodegaId}: {JsonSerializer.Serialize(bodega)}");
            var response = await _httpClient.PutAsJsonAsync($"api/Bodega/{bodega.BodegaId}", bodega);
            Console.WriteLine($"[API SERVICE] Respuesta ActualizarBodega: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                LastErrorMessage = ExtraerMensajeError(body);
                Console.WriteLine($"[API SERVICE ERROR] ActualizarBodega: {body}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<List<KardexMovimientoViewModel>> ObtenerMovimientosGlobalesAsync(int? bodegaId, string? desde, string? hasta, string? tipoMovimiento = null)
        {
            try 
            {
                var query = $"api/Inventario/movimientos?bodegaId={bodegaId}&fechaDesde={desde}&fechaHasta={hasta}&tipoMovimiento={tipoMovimiento}";
                var response = await _httpClient.GetFromJsonAsync<BackendResponse<List<KardexMovimientoViewModel>>>(query);
                return response?.Data ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API ERROR] ObtenerMovimientosGlobalesAsync: {ex.Message}");
                return new();
            }
        }

        public async Task<InventarioDashboardViewModel?> ObtenerDashboardAsync()
        {
            try 
            {
                var response = await _httpClient.GetFromJsonAsync<BackendResponse<InventarioDashboardViewModel>>("api/Inventario/dashboard");
                return response?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API ERROR] ObtenerDashboardAsync: {ex.Message}");
                return null;
            }
        }

        private static string ExtraerMensajeError(string body)
        {
            if (string.IsNullOrWhiteSpace(body)) return "Error desconocido del servidor";
            try
            {
                var j = JsonDocument.Parse(body);
                if (j.RootElement.TryGetProperty("mensaje", out var m) && m.GetString() != null) return m.GetString()!;
                if (j.RootElement.TryGetProperty("message", out var m2) && m2.GetString() != null) return m2.GetString()!;
                if (j.RootElement.TryGetProperty("resultado", out var m3) && m3.GetString() != null) return m3.GetString()!;
            }
            catch { }
            return body.Length > 500 ? body[..500] : body;
        }
    }
}
