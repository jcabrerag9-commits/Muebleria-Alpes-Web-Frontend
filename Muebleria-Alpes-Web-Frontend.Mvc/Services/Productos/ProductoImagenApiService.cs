using System.Net.Http.Headers;
using System.Text.Json;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Shared;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos
{
    public class ProductoImagenApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        public ProductoImagenApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ── List images for a product ─────────────────────────────────────────────

        public async Task<List<ProductoImagenViewModel>> ListarPorProductoAsync(int productoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ProductoImagen/producto/{productoId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[ProductoImagenApiService] ListarPorProducto({productoId}) -> {response.StatusCode}");
                    return new List<ProductoImagenViewModel>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BackendResponse<List<ProductoImagenViewModel>>>(json, _json);
                return result?.Data ?? new List<ProductoImagenViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoImagenApiService] ListarPorProducto EXCEPTION: {ex.Message}");
                return new List<ProductoImagenViewModel>();
            }
        }

        // ── Upload ────────────────────────────────────────────────────────────────

        public async Task<(bool Ok, string Mensaje)> SubirImagenAsync(UploadImagenViewModel model)
        {
            try
            {
                using var content = new MultipartFormDataContent();

                content.Add(new StringContent(model.ProductoId.ToString()), "ProductoId");
                content.Add(new StringContent(model.Orden.ToString()), "Orden");

                if (!string.IsNullOrEmpty(model.Tipo))
                    content.Add(new StringContent(model.Tipo), "Tipo");

                var fileContent = new StreamContent(model.Archivo.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.Archivo.ContentType);
                content.Add(fileContent, "Archivo", model.Archivo.FileName);

                var response = await _httpClient.PostAsync("api/ProductoImagen/upload", content);
                var body = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[ProductoImagenApiService] Upload -> {response.StatusCode}: {body}");

                if (response.IsSuccessStatusCode)
                    return (true, "Imagen subida correctamente.");

                // Try to extract backend error message
                try
                {
                    using var doc = JsonDocument.Parse(body);
                    if (doc.RootElement.TryGetProperty("message", out var msg) ||
                        doc.RootElement.TryGetProperty("Message", out msg) ||
                        doc.RootElement.TryGetProperty("mensaje", out msg))
                        return (false, msg.GetString() ?? $"Error {(int)response.StatusCode}");
                }
                catch { }

                return (false, $"Error {(int)response.StatusCode}: {body}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoImagenApiService] SubirImagen EXCEPTION: {ex.Message}");
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        // ── Delete ────────────────────────────────────────────────────────────────

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ProductoImagen/{id}");
                Console.WriteLine($"[ProductoImagenApiService] Eliminar({id}) -> {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoImagenApiService] Eliminar EXCEPTION: {ex.Message}");
                return false;
            }
        }

        // ── Image streams ─────────────────────────────────────────────────────────

        public async Task<(Stream? Stream, string? ContentType)> ObtenerPrincipalAsync(int productoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ProductoImagen/producto/{productoId}/principal");
                if (!response.IsSuccessStatusCode) return (null, null);

                var stream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType;
                return (stream, contentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoImagenApiService] ObtenerPrincipal EXCEPTION: {ex.Message}");
                return (null, null);
            }
        }

        public async Task<(Stream? Stream, string? ContentType)> ObtenerImagenAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ProductoImagen/{id}");
                if (!response.IsSuccessStatusCode) return (null, null);

                var stream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType;
                return (stream, contentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoImagenApiService] ObtenerImagen EXCEPTION: {ex.Message}");
                return (null, null);
            }
        }
    }
}
