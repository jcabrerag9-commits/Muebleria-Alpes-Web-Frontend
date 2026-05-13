using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class ReportesApiService
    {
        private readonly HttpClient _httpClient;

        public ReportesApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Dashboard data: fetch recent orders to show recent activity
        public async Task<List<OrdenRecienteViewModel>> ObtenerOrdenesRecientesAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<InventarioApiResponse<AdminOrdenesDataViewModel>>("api/admin/ordenes");
                return result?.Data?.Ordenes?.Take(5).Select(o => new OrdenRecienteViewModel
                {
                    NumeroOrden = o.NumeroOrden,
                    Cliente = o.Cliente,
                    Total = o.Total,
                    Estado = o.Estado,
                    Fecha = o.FechaOrden
                }).ToList() ?? new List<OrdenRecienteViewModel>();
            }
            catch
            {
                return new List<OrdenRecienteViewModel>();
            }
        }
    }
}
