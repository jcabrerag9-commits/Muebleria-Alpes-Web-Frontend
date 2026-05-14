using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos
{
    public class EvaluacionApiService
    {
        private readonly HttpClient _httpClient;

        public EvaluacionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EvaluacionViewModel>> ListarAsync(int? empleadoId = null)
        {
            try
            {
                var url = empleadoId.HasValue
                    ? $"api/rh/evaluaciones/empleado/{empleadoId}"
                    : "api/rh/evaluaciones/reporte?fechaInicio=2000-01-01&fechaFin=2099-12-31";

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<EvaluacionViewModel>>>(url);
                return response?.Resultado ?? new List<EvaluacionViewModel>();
            }
            catch
            {
                return new List<EvaluacionViewModel>();
            }
        }

        public async Task<bool> CrearAsync(CrearEvaluacionViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/rh/evaluaciones", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
