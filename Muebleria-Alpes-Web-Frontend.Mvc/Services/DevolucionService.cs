using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Devoluciones;
using System.Text;
using System.Text.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class DevolucionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        public DevolucionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient Client => _httpClientFactory.CreateClient("BackendApi");

        // ── Categorías ────────────────────────────────────────────────────────

        public async Task<List<CategoriaDevolucionViewModel>> GetCategoriasAsync(string? estado = null)
        {
            var url = string.IsNullOrEmpty(estado)
                ? "api/devoluciones/categorias"
                : $"api/devoluciones/categorias?estado={estado}";

            var response = await Client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return [];

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<List<CategoriaDevolucionViewModel>>>(json, _json);
            return result?.Data ?? [];
        }

        public async Task<(bool Ok, string Mensaje)> CreateCategoriaAsync(CategoriaDevolucionViewModel dto)
        {
            var payload = new { dto.CtdNombre, dto.CtdDescripcion };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/devoluciones/categorias", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al crear categoría.");
        }

        public async Task<(bool Ok, string Mensaje)> UpdateCategoriaAsync(long id, CategoriaDevolucionViewModel dto)
        {
            var payload = new { dto.CtdNombre, dto.CtdDescripcion, dto.CtdEstado };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await Client.PutAsync($"api/devoluciones/categorias/{id}", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al actualizar.");
        }

        public async Task<(bool Ok, string Mensaje)> DeleteCategoriaAsync(long id)
        {
            var response = await Client.DeleteAsync($"api/devoluciones/categorias/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al eliminar.");
        }

        // ── Devoluciones ──────────────────────────────────────────────────────

        public async Task<List<DevolucionViewModel>> GetAllAsync(string? estado = null, long? clienteId = null)
        {
            var url = "api/devoluciones";
            var query = new List<string>();
            if (!string.IsNullOrEmpty(estado)) query.Add($"estado={estado}");
            if (clienteId.HasValue) query.Add($"clienteId={clienteId}");
            if (query.Any()) url += "?" + string.Join("&", query);

            var response = await Client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return [];

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<List<DevolucionViewModel>>>(json, _json);
            return result?.Data ?? [];
        }

        public async Task<DevolucionViewModel?> GetByIdAsync(long id)
        {
            var response = await Client.GetAsync($"api/devoluciones/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<DevolucionViewModel>>(json, _json);
            return result?.Data;
        }

        public async Task<(bool Ok, string Mensaje)> CreateAsync(DevolucionCreateViewModel dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/devoluciones", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al crear devolución.");
        }

        public async Task<(bool Ok, string Mensaje)> CambiarEstadoAsync(long id, string nuevoEstado)
        {
            var payload = new { DevEstado = nuevoEstado };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await Client.PatchAsync($"api/devoluciones/{id}/estado", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al cambiar estado.");
        }

        public async Task<(bool Ok, string Mensaje)> DeleteAsync(long id)
        {
            var response = await Client.DeleteAsync($"api/devoluciones/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseBase>(json, _json);
            return (response.IsSuccessStatusCode, result?.Message ?? "Error al eliminar.");
        }

        // ── Helpers ───────────────────────────────────────────────────────────

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
