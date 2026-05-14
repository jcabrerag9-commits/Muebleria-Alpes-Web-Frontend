using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos
{
    public class AsistenciaApiService
    {
        private readonly HttpClient _httpClient;

        public AsistenciaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AsistenciaViewModel>> ListarAsync(int? empleadoId = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var query = new List<string>();
                if (empleadoId.HasValue) query.Add($"empleadoId={empleadoId}");
                if (fechaInicio.HasValue) query.Add($"fechaInicio={fechaInicio.Value:yyyy-MM-dd}");
                if (fechaFin.HasValue) query.Add($"fechaFin={fechaFin.Value:yyyy-MM-dd}");

                var url = "api/rh/asistencia";
                if (query.Count > 0) url += "?" + string.Join("&", query);

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<AsistenciaViewModel>>>(url);
                return response?.Resultado ?? new List<AsistenciaViewModel>();
            }
            catch
            {
                return new List<AsistenciaViewModel>();
            }
        }

        public async Task<bool> RegistrarAsync(CrearAsistenciaViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/rh/asistencia", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
