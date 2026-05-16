using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class AdminApiService
    {
        private readonly HttpClient _httpClient;

        public AdminApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrdenViewModel>> ListarOrdenesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<InventarioApiResponse<AdminOrdenesDataViewModel>>("api/admin/ordenes");
                return response?.Data?.Ordenes ?? new List<OrdenViewModel>();
            }
            catch
            {
                return new List<OrdenViewModel>();
            }
        }

        public async Task<List<PagoViewModel>> ListarPagosAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<InventarioApiResponse<AdminPagosDataViewModel>>("api/admin/pagos");
                return response?.Data?.Pagos ?? new List<PagoViewModel>();
            }
            catch
            {
                return new List<PagoViewModel>();
            }
        }

        public async Task<List<FacturaViewModel>> ListarFacturasAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<InventarioApiResponse<AdminFacturasDataViewModel>>("api/admin/facturas");
                return response?.Data?.Facturas ?? new List<FacturaViewModel>();
            }
            catch
            {
                return new List<FacturaViewModel>();
            }
        }

        public async Task<(bool Exitoso, string Mensaje)> ActualizarEstadoOrdenAsync(int ordenId, int nuevoEstado, int usuarioId, string comentario = "")
        {
            try
            {
                var body = new { OrdenId = ordenId, NuevoEstado = nuevoEstado, UsuarioId = usuarioId, Comentario = comentario };
                var response = await _httpClient.PutAsJsonAsync("api/ventas/orden/estado", body);
                var result = await response.Content.ReadFromJsonAsync<InventarioApiResponse<object>>();
                var exitoso = result?.Resultado?.ToUpper() == "EXITO" || response.IsSuccessStatusCode;
                return (exitoso, result?.Mensaje ?? "Sin respuesta del servidor");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
