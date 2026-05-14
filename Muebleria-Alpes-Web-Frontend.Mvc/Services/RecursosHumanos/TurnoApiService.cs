using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos
{
    public class TurnoApiService
    {
        private readonly HttpClient _httpClient;

        public TurnoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TurnoViewModel>> ListarAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<TurnoViewModel>>>("api/rh/turnos");
                return response?.Resultado ?? new List<TurnoViewModel>();
            }
            catch
            {
                return new List<TurnoViewModel>();
            }
        }

        public async Task<bool> CrearAsync(CrearTurnoViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/rh/turnos", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
