using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Devoluciones;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    /// <summary>
    /// Servicio para api/devoluciones y api/devoluciones/categorias.
    /// Respuesta estándar: { success: true, data: ... }
    /// </summary>
    public class DevolucionApiService
    {
        private readonly HttpClient _httpClient;

        public DevolucionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ── Devoluciones ──────────────────────────────────────────────────────

        // GET api/devoluciones?estado=...
        public async Task<List<DevolucionViewModel>> GetAllAsync(string? estado = null)
        {
            try
            {
                var url = "api/devoluciones";
                if (!string.IsNullOrWhiteSpace(estado))
                    url += $"?estado={Uri.EscapeDataString(estado)}";

                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<DevolucionViewModel>>>(url);
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        // GET api/devoluciones/{id}
        public async Task<DevolucionViewModel?> GetByIdAsync(long id)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<DevolucionViewModel>>($"api/devoluciones/{id}");
                return result?.Data;
            }
            catch { return null; }
        }

        // POST api/devoluciones
        public async Task<(bool Ok, string Mensaje)> CreateAsync(DevolucionCreateViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/devoluciones", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Devolución registrada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode}");
            }
            catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
        }

        // PATCH api/devoluciones/{id}/estado
        public async Task<(bool Ok, string Mensaje)> CambiarEstadoAsync(long id, string nuevoEstado)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync(
                    $"api/devoluciones/{id}/estado",
                    new { NuevoEstado = nuevoEstado });

                if (response.IsSuccessStatusCode)
                    return (true, $"Estado actualizado a '{nuevoEstado}'.");

                var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode}");
            }
            catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
        }

        // DELETE api/devoluciones/{id}
        public async Task<(bool Ok, string Mensaje)> DeleteAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/devoluciones/{id}");
                if (response.IsSuccessStatusCode)
                    return (true, "Devolución eliminada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode}");
            }
            catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
        }

        // ── Categorías de devolución ──────────────────────────────────────────

        // GET api/devoluciones/categorias?estado=...
        public async Task<List<CategoriaDevolucionViewModel>> GetCategoriasAsync(string? estado = null)
        {
            try
            {
                var url = "api/devoluciones/categorias";
                if (!string.IsNullOrWhiteSpace(estado))
                    url += $"?estado={Uri.EscapeDataString(estado)}";

                var result = await _httpClient.GetFromJsonAsync<SuccessDataResponse<List<CategoriaDevolucionViewModel>>>(url);
                return result?.Data ?? [];
            }
            catch { return []; }
        }

        // POST api/devoluciones/categorias
        public async Task<(bool Ok, string Mensaje)> CreateCategoriaAsync(CategoriaDevolucionViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/devoluciones/categorias", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Categoría creada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode}");
            }
            catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
        }

        // PUT api/devoluciones/categorias/{id}
        public async Task<(bool Ok, string Mensaje)> UpdateCategoriaAsync(long id, CategoriaDevolucionViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/devoluciones/categorias/{id}", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Categoría actualizada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode}");
            }
            catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
        }

        // DELETE api/devoluciones/categorias/{id}
        public async Task<(bool Ok, string Mensaje)> DeleteCategoriaAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/devoluciones/categorias/{id}");
                if (response.IsSuccessStatusCode)
                    return (true, "Categoría eliminada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, body?.Message ?? $"Error {(int)response.StatusCode}");
            }
            catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
        }

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
