using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Promociones;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    /// <summary>
    /// Servicio para api/promociones.
    /// Respuesta estándar: { success: true, data: ... }
    /// </summary>
    public class PromocionApiService
    {
        private readonly HttpClient _httpClient;

        public PromocionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ── Promociones ───────────────────────────────────────────────────────

        // GET api/promociones?estado=...&tipo=...
        public async Task<List<PromocionViewModel>> GetAllAsync(string? estado = null, string? tipo = null)
        {
            try
            {
                var query = new List<string>();
                if (!string.IsNullOrWhiteSpace(estado)) query.Add($"estado={Uri.EscapeDataString(estado)}");
                if (!string.IsNullOrWhiteSpace(tipo))   query.Add($"tipo={Uri.EscapeDataString(tipo)}");
                var url = "api/promociones" + (query.Count > 0 ? "?" + string.Join("&", query) : "");

                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<PromocionViewModel>>>(url);
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        // GET api/promociones/vigentes
        public async Task<List<PromocionViewModel>> GetVigentesAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<PromocionViewModel>>>("api/promociones/vigentes");
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        // GET api/promociones/{id}
        public async Task<PromocionViewModel?> GetByIdAsync(long id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<PromocionViewModel>>($"api/promociones/{id}");
                return result?.Data;
            }
            catch { return null; }
        }

        // POST api/promociones
        public async Task<(bool Ok, string Mensaje)> CreateAsync(PromocionCreateViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/promociones", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Promoción creada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode}");
            }
            catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
        }

        // PUT api/promociones/{id}
        public async Task<(bool Ok, string Mensaje)> UpdateAsync(long id, PromocionUpdateViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/promociones/{id}", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Promoción actualizada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode}");
            }
            catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
        }

        // DELETE api/promociones/{id}
        public async Task<(bool Ok, string Mensaje)> DeleteAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/promociones/{id}");
                if (response.IsSuccessStatusCode)
                    return (true, "Promoción eliminada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode}");
            }
            catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
        }

        // ── Banners (no implementados en backend aún — stubs seguros) ─────────

        public Task<List<BannerViewModel>> GetBannersAsync(string? estado = null)
            => Task.FromResult(new List<BannerViewModel>());

        public Task<(bool Ok, string Mensaje)> CreateBannerAsync(BannerCreateViewModel model)
            => Task.FromResult((false, "La gestión de banners aún no está disponible."));

        public Task<(bool Ok, string Mensaje)> DeleteBannerAsync(long id)
            => Task.FromResult((false, "La gestión de banners aún no está disponible."));

        // ── DTO interno ───────────────────────────────────────────────────────
        private sealed class ApiErrorResponse
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("message")]
            public string? Message { get; set; }
        }
    }
}
