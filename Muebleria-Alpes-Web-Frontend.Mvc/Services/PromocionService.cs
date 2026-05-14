using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Promociones;
using System.Text;
using System.Text.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class PromocionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        public PromocionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient Client => _httpClientFactory.CreateClient("BackendApi");

        public async Task<List<PromocionViewModel>> GetAllAsync(string? estado = null, string? tipo = null)
        {
            var url = "api/promociones";
            var query = new List<string>();
            if (!string.IsNullOrEmpty(estado)) query.Add($"estado={estado}");
            if (!string.IsNullOrEmpty(tipo)) query.Add($"tipo={tipo}");
            if (query.Any()) url += "?" + string.Join("&", query);

            var response = await Client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return [];
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<List<PromocionViewModel>>>(json, _json);
            return result?.Data ?? [];
        }

        public async Task<PromocionViewModel?> GetByIdAsync(long id)
        {
            var response = await Client.GetAsync($"api/promociones/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<PromocionViewModel>>(json, _json);
            return result?.Data;
        }

        public async Task<List<PromocionViewModel>> GetVigentesAsync()
        {
            var response = await Client.GetAsync("api/promociones/vigentes");
            if (!response.IsSuccessStatusCode) return [];
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<List<PromocionViewModel>>>(json, _json);
            return result?.Data ?? [];
        }

        public async Task<(bool Ok, string Mensaje)> CreateAsync(PromocionCreateViewModel dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/promociones", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al crear.");
        }

        public async Task<(bool Ok, string Mensaje)> UpdateAsync(long id, PromocionUpdateViewModel dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await Client.PutAsync($"api/promociones/{id}", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al actualizar.");
        }

        public async Task<(bool Ok, string Mensaje)> DeleteAsync(long id)
        {
            var response = await Client.DeleteAsync($"api/promociones/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al eliminar.");
        }

        public async Task<List<BannerViewModel>> GetBannersAsync(string? estado = null)
        {
            var url = string.IsNullOrEmpty(estado) ? "api/banners" : $"api/banners?estado={estado}";
            var response = await Client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return [];
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<List<BannerViewModel>>>(json, _json);
            return result?.Data ?? [];
        }

        public async Task<(bool Ok, string Mensaje)> CreateBannerAsync(BannerCreateViewModel dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/banners", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al crear banner.");
        }

        public async Task<(bool Ok, string Mensaje)> DeleteBannerAsync(long id)
        {
            var response = await Client.DeleteAsync($"api/banners/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al eliminar.");
        }

        private class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public T? Data { get; set; }
        }

        private class ApiResponseBase
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
        }
    }
}