using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos
{
    public class PuestoApiService
    {
        private readonly HttpClient _httpClient;

        public PuestoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PuestoViewModel>> ListarAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PuestoViewModel>>>("api/rh/puestos");
                return response?.Resultado ?? new List<PuestoViewModel>();
            }
            catch
            {
                return new List<PuestoViewModel>();
            }
        }

        public async Task<bool> CrearAsync(CrearPuestoViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/rh/puestos", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarPuestoViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/rh/puestos/{id}", model);
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
                var model = new CambiarEstadoPuestoViewModel { Estado = estado, UsuarioId = 1 };
                var response = await _httpClient.PatchAsJsonAsync($"api/rh/puestos/{id}/estado", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
