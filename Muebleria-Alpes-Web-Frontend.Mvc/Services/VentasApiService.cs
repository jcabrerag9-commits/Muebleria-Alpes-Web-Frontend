using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class VentasApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

        public VentasApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private class VentasOrdenesWrapper
        {
            [JsonPropertyName("ordenes")]
            public List<OrdenClienteViewModel> Ordenes { get; set; } = new();
        }

        // Generic wrapper for { success, data } responses from api/Envios/*
        private class AlpesDataWrapper<T>
        {
            [JsonPropertyName("data")] public T? Data { get; set; }
        }

        public async Task<List<OrdenClienteViewModel>> ObtenerOrdenesClienteAsync(int clienteId)
        {
            try
            {
                var r = await _httpClient.GetFromJsonAsync<InventarioApiResponse<VentasOrdenesWrapper>>(
                    $"api/ventas/ordenes/cliente/{clienteId}");
                return r?.Data?.Ordenes ?? new List<OrdenClienteViewModel>();
            }
            catch
            {
                return new List<OrdenClienteViewModel>();
            }
        }

        public async Task<List<DireccionClienteViewModel>> ObtenerDireccionesAsync(int clienteId)
        {
            try
            {
                var r = await _httpClient.GetFromJsonAsync<ClienteDetalleApiResponse>(
                    $"api/Clientes/{clienteId}");
                return r?.Direcciones ?? new List<DireccionClienteViewModel>();
            }
            catch
            {
                return new List<DireccionClienteViewModel>();
            }
        }

        public async Task<(bool ok, string mensaje)> AgregarDireccionAsync(int clienteId, NuevaDireccionRequest req)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/Clientes/{clienteId}/direcciones", req);
                if (response.IsSuccessStatusCode) return (true, "Dirección guardada correctamente.");
                var body = await response.Content.ReadAsStringAsync();
                return (false, $"Error del servidor: {(int)response.StatusCode}");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene el envío más reciente asociado a una orden (para mostrar al cliente).
        /// Llama a api/Envios/orden/{ordenId} que devuelve { success, data: [...] }.
        /// </summary>
        public async Task<EnvioResumenClienteViewModel?> ObtenerEnvioPorOrdenAsync(int ordenId)
        {
            try
            {
                var text = await _httpClient.GetStringAsync($"api/Envios/orden/{ordenId}");
                if (string.IsNullOrWhiteSpace(text)) return null;

                var wrapped = JsonSerializer.Deserialize<AlpesDataWrapper<List<EnvioResumenClienteViewModel>>>(text, _jsonOpts);
                return wrapped?.Data?.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
