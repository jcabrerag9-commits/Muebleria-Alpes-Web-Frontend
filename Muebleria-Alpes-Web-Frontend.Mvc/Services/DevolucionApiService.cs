using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Devoluciones;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class DevolucionApiService
    {
        private readonly HttpClient _httpClient;

        public DevolucionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DevolucionViewModel>> GetAllAsync(string? estado = null)
        {
            try
            {
                var url = string.IsNullOrEmpty(estado) ? "api/devoluciones" : $"api/devoluciones?estado={estado}";
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<DevolucionViewModel>>>(url);
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<DevolucionViewModel?> GetByIdAsync(long id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<DevolucionViewModel>>($"api/devoluciones/{id}");
                return result?.Data;
            }
            catch { return null; }
        }

        public async Task<(bool Ok, string Mensaje)> CreateAsync(DevolucionCreateViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/devoluciones", model);
                var body = await response.Content.ReadFromJsonAsync<DevolucionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<(bool Ok, string Mensaje)> CambiarEstadoAsync(long id, string nuevoEstado)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync($"api/devoluciones/{id}/estado", new { nuevoEstado });
                var body = await response.Content.ReadFromJsonAsync<DevolucionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<(bool Ok, string Mensaje)> DeleteAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/devoluciones/{id}");
                var body = await response.Content.ReadFromJsonAsync<DevolucionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        // Categorías
        public async Task<List<CategoriaDevolucionViewModel>> GetCategoriasAsync(string? estado = null)
        {
            try
            {
                var url = string.IsNullOrEmpty(estado) ? "api/devoluciones/categorias" : $"api/devoluciones/categorias?estado={estado}";
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<CategoriaDevolucionViewModel>>>(url);
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<(bool Ok, string Mensaje)> CreateCategoriaAsync(CategoriaDevolucionViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/devoluciones/categorias", model);
                var body = await response.Content.ReadFromJsonAsync<DevolucionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<(bool Ok, string Mensaje)> UpdateCategoriaAsync(long id, CategoriaDevolucionViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/devoluciones/categorias/{id}", model);
                var body = await response.Content.ReadFromJsonAsync<DevolucionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<(bool Ok, string Mensaje)> DeleteCategoriaAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/devoluciones/categorias/{id}");
                var body = await response.Content.ReadFromJsonAsync<DevolucionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        private sealed class DevolucionErrorResponse
        {
            [JsonPropertyName("message")]
            public string? Message { get; set; }
        }
    }
}
