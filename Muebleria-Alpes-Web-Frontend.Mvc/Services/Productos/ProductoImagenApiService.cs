using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Shared;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;
using System.Net.Http.Headers;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos
{
    public class ProductoImagenApiService
    {
        private readonly HttpClient _httpClient;

        public ProductoImagenApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductoImagenViewModel>> ListarPorProductoAsync(int productoId)
        {
            var response = await _httpClient.GetFromJsonAsync<BackendResponse<List<ProductoImagenViewModel>>>($"api/ProductoImagen/producto/{productoId}");
            return response?.Data ?? new List<ProductoImagenViewModel>();
        }

        public async Task<bool> SubirImagenAsync(UploadImagenViewModel model)
        {
            using var content = new MultipartFormDataContent();
            
            content.Add(new StringContent(model.ProductoId.ToString()), "ProductoId");
            content.Add(new StringContent(model.Orden.ToString()), "Orden");
            
            if (!string.IsNullOrEmpty(model.Tipo))
            {
                content.Add(new StringContent(model.Tipo), "Tipo");
            }

            var fileContent = new StreamContent(model.Archivo.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.Archivo.ContentType);
            
            content.Add(fileContent, "Archivo", model.Archivo.FileName);

            var response = await _httpClient.PostAsync("api/ProductoImagen/upload", content);
            var responseString = await response.Content.ReadAsStringAsync();
            
            System.Console.WriteLine($"[ProductoImagenApiService] Backend Status: {response.StatusCode}");
            System.Console.WriteLine($"[ProductoImagenApiService] Backend Response: {responseString}");

            if (response.IsSuccessStatusCode)
            {
                var result = System.Text.Json.JsonSerializer.Deserialize<BackendResponse<int>>(responseString, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result != null && result.IsSuccess;
            }
            
            throw new Exception($"HTTP {(int)response.StatusCode}: {responseString}");
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/ProductoImagen/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<(Stream? Stream, string? ContentType)> ObtenerPrincipalAsync(int productoId)
        {
            var response = await _httpClient.GetAsync($"api/ProductoImagen/producto/{productoId}/principal");
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType;
                return (stream, contentType);
            }
            return (null, null);
        }

        public async Task<(Stream? Stream, string? ContentType)> ObtenerImagenAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/ProductoImagen/{id}");
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType;
                return (stream, contentType);
            }
            return (null, null);
        }
    }
}
