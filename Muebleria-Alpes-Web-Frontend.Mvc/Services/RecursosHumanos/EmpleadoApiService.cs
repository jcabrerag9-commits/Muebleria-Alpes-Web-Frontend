using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos
{
    public class EmpleadoApiService
    {
        private readonly HttpClient _httpClient;

        public EmpleadoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EmpleadoViewModel>> ListarAsync(string? estado = null)
        {
            var url = "api/rh/empleados";

            if (!string.IsNullOrWhiteSpace(estado))
                url += $"?estado={estado}";

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<EmpleadoViewModel>>>(url);

            return response?.Resultado ?? new List<EmpleadoViewModel>();
        }

        public async Task<EmpleadoViewModel?> ObtenerPorIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<EmpleadoViewModel>>($"api/rh/empleados/{id}");
            return response?.Resultado;
        }

        public async Task<bool> CrearAsync(CrearEmpleadoViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/rh/empleados", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarEmpleadoViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/rh/empleados/{id}", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CambiarEstadoAsync(int id, CambiarEstadoEmpleadoViewModel model)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/rh/empleados/{id}/estado", model);
            return response.IsSuccessStatusCode;
        }
    }
}
