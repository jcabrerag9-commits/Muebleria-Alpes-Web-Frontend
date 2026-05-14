using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    /// <summary>
    /// Servicio para api/devoluciones — retorna { success: true, data: [...] }.
    /// </summary>
    public class DevolucionApiService
    {
        private readonly HttpClient _httpClient;

        public DevolucionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/devoluciones?estado=&clienteId= → { success: true, data: [...] }
        public async Task<List<DevolucionViewModel>> ListarAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<DevolucionViewModel>>>("api/devoluciones");
                return result?.Data ?? new List<DevolucionViewModel>();
            }
            catch
            {
                return new List<DevolucionViewModel>();
            }
        }

        public async Task<bool> CrearAsync(CrearDevolucionViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/devoluciones", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // PATCH api/devoluciones/{id}/aprobar
        public async Task<bool> AprobarAsync(int id)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync($"api/devoluciones/{id}/aprobar", new { });
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // PATCH api/devoluciones/{id}/rechazar
        public async Task<bool> RechazarAsync(int id)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync($"api/devoluciones/{id}/rechazar", new { });
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
