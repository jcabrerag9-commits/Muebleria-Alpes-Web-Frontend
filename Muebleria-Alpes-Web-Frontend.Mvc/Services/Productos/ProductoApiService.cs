using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Shared;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos
{
    public class ProductoApiService
    {
        private readonly HttpClient _httpClient;

        public ProductoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductoViewModel>> ListarAsync()
        {
            try 
            {
                var response = await _httpClient.GetAsync("api/Productos");
                var raw = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine($"[MVC API] Error Listar: {raw}");
                    return new List<ProductoViewModel>();
                }

                // Intentar deserializar como lista directa (estándar Backend actual)
                try {
                    var directList = System.Text.Json.JsonSerializer.Deserialize<List<ProductoViewModel>>(raw, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (directList != null) return directList;
                } catch { /* No es lista directa, probar wrapper */ }

                var result = System.Text.Json.JsonSerializer.Deserialize<BackendResponse<List<ProductoViewModel>>>(raw, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<ProductoViewModel>();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"[MVC API] Exception Listar: {ex.Message}");
                return new List<ProductoViewModel>();
            }
        }

        public async Task<ProductoViewModel?> ObtenerPorIdAsync(int id)
        {
            try 
            {
                var response = await _httpClient.GetAsync($"api/Productos/{id}");
                var raw = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode) return null;

                try {
                    var direct = System.Text.Json.JsonSerializer.Deserialize<ProductoViewModel>(raw, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (direct != null && direct.ProductoId > 0) return direct;
                } catch { }

                var result = System.Text.Json.JsonSerializer.Deserialize<BackendResponse<ProductoViewModel>>(raw, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"[MVC API] Exception GetById: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CrearAsync(CrearProductoViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Productos", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarProductoViewModel model)
        {
            System.Console.WriteLine($"[API-SERVICE] PUT a api/Productos/{id}");
            var response = await _httpClient.PutAsJsonAsync($"api/Productos/{id}", model);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine($"[API-SERVICE] ERROR {response.StatusCode}: {errorBody}");
            }
            
            return response.IsSuccessStatusCode;
        }
    }
}
