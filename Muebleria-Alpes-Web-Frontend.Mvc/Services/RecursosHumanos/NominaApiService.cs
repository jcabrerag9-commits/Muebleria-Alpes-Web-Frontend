using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos
{
    public class NominaApiService
    {
        private readonly HttpClient _httpClient;

        public NominaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NominaViewModel>> ListarAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<NominaViewModel>>>("api/rh/nominas");
                return response?.Resultado ?? new List<NominaViewModel>();
            }
            catch
            {
                return new List<NominaViewModel>();
            }
        }

        public async Task<bool> CrearAsync(CrearNominaViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/rh/nominas", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            try
            {
                var model = new CambiarEstadoNominaViewModel { Estado = estado, UsuarioId = 1 };
                var response = await _httpClient.PatchAsJsonAsync($"api/rh/nominas/{id}/estado", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<NominaDetalleViewModel>> ListarDetalleAsync(int nominaId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<NominaDetalleViewModel>>>($"api/rh/nominas/{nominaId}/detalle");
                return response?.Resultado ?? new List<NominaDetalleViewModel>();
            }
            catch
            {
                return new List<NominaDetalleViewModel>();
            }
        }
    }
}
