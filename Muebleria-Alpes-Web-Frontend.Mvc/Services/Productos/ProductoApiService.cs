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
                var response = await _httpClient.GetAsync("api/Producto");
                var raw = await response.Content.ReadAsStringAsync();

                System.Console.WriteLine($"[MVC HOTFIX] Status: {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine($"[MVC HOTFIX] ERROR BODY: {raw}");
                    return new List<ProductoViewModel>();
                }

                var result = System.Text.Json.JsonSerializer.Deserialize<BackendResponse<List<ProductoViewModel>>>(raw, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<ProductoViewModel>();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"[MVC HOTFIX] EXCEPTION: {ex.Message}");
                return new List<ProductoViewModel>();
            }
        }

        public async Task<ProductoViewModel?> ObtenerPorIdAsync(int id)
        {
            try 
            {
                var response = await _httpClient.GetAsync($"api/Producto/{id}");
                var raw = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine($"[MVC ApiService] ERROR ObtenerPorId({id}): {raw}");
                    return null;
                }

                var result = System.Text.Json.JsonSerializer.Deserialize<BackendResponse<ProductoViewModel>>(raw, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"[MVC ApiService] EXCEPTION ObtenerPorId({id}): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CrearAsync(CrearProductoViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Producto/crear", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<BackendResponse<object>>();
                return result != null && result.IsSuccess;
            }
            return false;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarProductoViewModel model)
        {
            System.Console.WriteLine($"[MVC ApiService] Enviando PUT a api/Producto/{id}");
            var response = await _httpClient.PutAsJsonAsync($"api/Producto/{id}", model);
            
            System.Console.WriteLine($"[MVC ApiService] Status Code del Backend: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<BackendResponse<object>>();
                System.Console.WriteLine($"[MVC ApiService] BackendResponse IsSuccess: {result?.IsSuccess}");
                return result != null && result.IsSuccess;
            }
            else
            {
                var errText = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine($"[MVC ApiService] ERROR del Backend: {errText}");
            }
            return false;
        }
    }
}
