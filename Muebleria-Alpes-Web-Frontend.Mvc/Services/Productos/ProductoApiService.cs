using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;
using System.Net.Http.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos
{
    /// <summary>
    /// Servicio para api/productos y api/categorias — retornan lista directa (sin ApiResponse wrapper).
    /// </summary>
    public class ProductoApiService
    {
        private readonly HttpClient _httpClient;

        public ProductoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ─── CATEGORÍAS ───────────────────────────────────────────────────────────

        // GET api/categorias → bare List<Categoria>
        public async Task<List<CategoriaViewModel>> ListarCategoriasAsync()
        {
            try
            {
                var lista = await _httpClient.GetFromJsonAsync<List<CategoriaViewModel>>("api/categorias");
                return lista ?? new List<CategoriaViewModel>();
            }
            catch
            {
                return new List<CategoriaViewModel>();
            }
        }

        // GET api/categorias/producto/{productoId} → categorías de un producto
        public async Task<List<CategoriaViewModel>> ObtenerCategoriasPorProductoAsync(int productoId)
        {
            try
            {
                var lista = await _httpClient.GetFromJsonAsync<List<CategoriaViewModel>>(
                    $"api/categorias/producto/{productoId}");
                return lista ?? new List<CategoriaViewModel>();
            }
            catch
            {
                return new List<CategoriaViewModel>();
            }
        }

        // ─── PRODUCTOS ────────────────────────────────────────────────────────────

        // GET api/productos → retorna IEnumerable<Producto> directamente (sin wrapper)
        public async Task<List<ProductoViewModel>> ListarAsync(string? estado = null)
        {
            try
            {
                var url = "api/productos";
                if (!string.IsNullOrWhiteSpace(estado))
                    url += $"?estado={estado}";

                var lista = await _httpClient.GetFromJsonAsync<List<ProductoViewModel>>(url);
                return lista ?? new List<ProductoViewModel>();
            }
            catch
            {
                return new List<ProductoViewModel>();
            }
        }

        // GET api/productos/{id} → retorna Producto directamente
        public async Task<ProductoViewModel?> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ProductoViewModel>($"api/productos/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CrearAsync(CrearProductoViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/productos", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarProductoViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/productos/{id}", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"api/productos/{id}/estado?estado={estado}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/productos/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
