using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos
{
    public class DepartamentoApiService
    {
        private readonly HttpClient _httpClient;

        public DepartamentoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DepartamentoViewModel>> ListarAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DepartamentoViewModel>>>("api/rh/departamentos");
            return response?.Resultado ?? new List<DepartamentoViewModel>();
        }

        public async Task<DepartamentoViewModel?> ObtenerPorIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<DepartamentoViewModel>>($"api/rh/departamentos/{id}");
            return response?.Resultado;
        }

        public async Task<bool> CrearAsync(CrearDepartamentoViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/rh/departamentos", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarDepartamentoViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/rh/departamentos/{id}", model);
            return response.IsSuccessStatusCode;
        }
    }
}
