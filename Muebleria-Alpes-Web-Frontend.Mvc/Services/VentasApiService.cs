using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class VentasApiService
    {
        private readonly HttpClient _httpClient;

        public VentasApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private class VentasOrdenesWrapper
        {
            [JsonPropertyName("ordenes")]
            public List<OrdenClienteViewModel> Ordenes { get; set; } = new();
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
    }
}
