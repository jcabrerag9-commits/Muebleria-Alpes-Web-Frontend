using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    /// <summary>
    /// Servicio para api/promociones — respuesta GET: { success: true, data: [PromocionListDto...] }.
    /// Los DTOs del backend usan nombres "prmPromocion", "prmNombre", etc. (camelCase de PrmNombre).
    /// </summary>
    public class PromocionApiService
    {
        private readonly HttpClient _httpClient;

        public PromocionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/promociones → { success: true, data: [PromocionListDto...] }
        public async Task<List<PromocionViewModel>> ListarAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<PromocionViewModel>>>("api/promociones");
                return result?.Data ?? new List<PromocionViewModel>();
            }
            catch
            {
                return new List<PromocionViewModel>();
            }
        }

        // POST api/promociones — body mapea a PromocionCreateDto (prmNombre, prmTipo, etc.)
        public async Task<(bool Ok, string Mensaje)> CrearAsync(CrearPromocionViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/promociones", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Promoción creada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<PromocionErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode} al crear la promoción.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        // PUT api/promociones/{id} — body mapea a PromocionUpdateDto
        public async Task<(bool Ok, string Mensaje)> ActualizarAsync(long id, CrearPromocionViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/promociones/{id}", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Promoción actualizada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<PromocionErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode} al actualizar la promoción.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        // DELETE api/promociones/{id}
        public async Task<(bool Ok, string Mensaje)> EliminarAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/promociones/{id}");
                if (response.IsSuccessStatusCode)
                    return (true, "Promoción eliminada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<PromocionErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode} al eliminar la promoción.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        // DTO interno para leer mensajes de error del backend
        private sealed class PromocionErrorResponse
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("message")]
            public string? Message { get; set; }
        }
    }
}
