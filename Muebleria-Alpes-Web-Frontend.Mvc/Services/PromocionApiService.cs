using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Promociones;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class PromocionApiService
    {
        private readonly HttpClient _httpClient;

        public PromocionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PromocionViewModel>> GetAllAsync(string? estado = null, string? tipo = null)
        {
            try
            {
                var query = string.Empty;
                if (!string.IsNullOrEmpty(estado)) query += $"estado={estado}&";
                if (!string.IsNullOrEmpty(tipo)) query += $"tipo={tipo}&";
                
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<PromocionViewModel>>>($"api/promociones?{query}");
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<List<PromocionViewModel>> GetVigentesAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<PromocionViewModel>>>("api/promociones/vigentes");
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<PromocionViewModel?> GetByIdAsync(long id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<PromocionViewModel>>($"api/promociones/{id}");
                return result?.Data;
            }
            catch { return null; }
        }

        public async Task<(bool Ok, string Mensaje)> CreateAsync(CrearPromocionViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/promociones", model);
                var body = await response.Content.ReadFromJsonAsync<PromocionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<(bool Ok, string Mensaje)> UpdateAsync(long id, PromocionUpdateViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/promociones/{id}", model);
                var body = await response.Content.ReadFromJsonAsync<PromocionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<(bool Ok, string Mensaje)> DeleteAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/promociones/{id}");
                var body = await response.Content.ReadFromJsonAsync<PromocionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        // Banners
        public async Task<List<BannerViewModel>> GetBannersAsync(string? estado = null)
        {
            try
            {
                var url = string.IsNullOrEmpty(estado) ? "api/promociones/banners" : $"api/promociones/banners?estado={estado}";
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<BannerViewModel>>>(url);
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        public async Task<(bool Ok, string Mensaje)> CreateBannerAsync(BannerCreateViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/promociones/banners", model);
                var body = await response.Content.ReadFromJsonAsync<PromocionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<(bool Ok, string Mensaje)> DeleteBannerAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/promociones/banners/{id}");
                var body = await response.Content.ReadFromJsonAsync<PromocionErrorResponse>();
                return (response.IsSuccessStatusCode, body?.Message ?? (response.IsSuccessStatusCode ? "Éxito" : "Error"));
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        private sealed class PromocionErrorResponse
        {
            [JsonPropertyName("message")]
            public string? Message { get; set; }
        }
    }
}
