using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos
{
    public class VacacionesApiService
    {
        private readonly HttpClient _httpClient;

        public VacacionesApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<VacacionViewModel>> ListarAsync(int? empleadoId = null, string? estado = null)
        {
            try
            {
                var query = new List<string>();
                if (empleadoId.HasValue) query.Add($"empleadoId={empleadoId}");
                if (!string.IsNullOrWhiteSpace(estado)) query.Add($"estado={estado}");

                var url = "api/rh/vacaciones";
                if (query.Count > 0) url += "?" + string.Join("&", query);

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<VacacionViewModel>>>(url);
                return response?.Resultado ?? new List<VacacionViewModel>();
            }
            catch
            {
                return new List<VacacionViewModel>();
            }
        }

        public async Task<bool> SolicitarAsync(CrearVacacionViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/rh/vacaciones", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AprobarAsync(int id)
        {
            try
            {
                var model = new AprobarRechazarVacacionViewModel { UsuarioId = 1 };
                var response = await _httpClient.PatchAsJsonAsync($"api/rh/vacaciones/{id}/aprobar", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RechazarAsync(int id, string motivo)
        {
            try
            {
                var model = new AprobarRechazarVacacionViewModel { Motivo = motivo, UsuarioId = 1 };
                var response = await _httpClient.PatchAsJsonAsync($"api/rh/vacaciones/{id}/rechazar", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
