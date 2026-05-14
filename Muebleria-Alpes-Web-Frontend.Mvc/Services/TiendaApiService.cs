using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    /// <summary>
    /// Servicio de tienda virtual — api/productos retorna lista directa (sin wrapper).
    /// El precio se obtiene por separado desde api/precios/vigente/{id}.
    /// </summary>
    public class TiendaApiService
    {
        private readonly HttpClient _httpClient;

        public TiendaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/productos → bare List<Producto>, luego enriquece con precios en paralelo
        public async Task<List<TiendaProductoViewModel>> ObtenerProductosAsync()
        {
            try
            {
                var lista = await _httpClient.GetFromJsonAsync<List<TiendaProductoViewModel>>("api/productos");
                if (lista == null || lista.Count == 0)
                    return new List<TiendaProductoViewModel>();

                await EnriquecerConPreciosAsync(lista);
                AsignarUrlImagenes(lista);
                return lista;
            }
            catch
            {
                return new List<TiendaProductoViewModel>();
            }
        }

        // GET api/productos/{id} → bare Producto, luego enriquece con precio e imagen
        public async Task<TiendaProductoViewModel?> ObtenerProductoAsync(int id)
        {
            try
            {
                var producto = await _httpClient.GetFromJsonAsync<TiendaProductoViewModel>($"api/productos/{id}");
                if (producto != null)
                {
                    await EnriquecerConPreciosAsync(new List<TiendaProductoViewModel> { producto });
                    AsignarUrlImagenes(new List<TiendaProductoViewModel> { producto });
                }
                return producto;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene el precio vigente de cada producto en paralelo.
        /// GET api/precios/vigente/{productoId} → { productoId, monedaId, precio }
        /// </summary>
        private async Task EnriquecerConPreciosAsync(List<TiendaProductoViewModel> productos)
        {
            var tareas = productos.Select(async p =>
            {
                try
                {
                    var resp = await _httpClient.GetFromJsonAsync<PrecioVigenteResponse>(
                        $"api/precios/vigente/{p.Id}");
                    if (resp != null)
                        p.Precio = resp.Precio ?? 0m;
                }
                catch
                {
                    // Si falla el precio de un producto, lo dejamos en 0 sin romper los demás
                }
            });

            await Task.WhenAll(tareas);
        }

        // GET api/categorias → bare List<Categoria>
        public async Task<List<TiendaCategoriaViewModel>> ObtenerCategoriasAsync()
        {
            try
            {
                var lista = await _httpClient.GetFromJsonAsync<List<TiendaCategoriaViewModel>>("api/categorias");
                return lista ?? new List<TiendaCategoriaViewModel>();
            }
            catch
            {
                return new List<TiendaCategoriaViewModel>();
            }
        }

        // GET api/categorias/{id}/productos → IDs de productos en esa categoría
        public async Task<List<int>> ObtenerProductoIdsDeCategoriaAsync(int categoriaId)
        {
            try
            {
                var items = await _httpClient.GetFromJsonAsync<List<ProductoEnCategoriaResponse>>(
                    $"api/categorias/{categoriaId}/productos");
                return items?.Select(x => x.ProductoId).ToList() ?? new List<int>();
            }
            catch
            {
                return new List<int>();
            }
        }

        /// <summary>
        /// Apunta ImagenUrl al proxy local del frontend (/Tienda/Imagen/{id}).
        /// Esto evita problemas de TLS/CORS cuando el browser carga imágenes directamente del backend.
        /// </summary>
        private void AsignarUrlImagenes(List<TiendaProductoViewModel> productos)
        {
            foreach (var p in productos)
                p.ImagenUrl = $"/Tienda/Imagen/{p.Id}";
        }

        /// <summary>
        /// Obtiene los bytes de la imagen principal de un producto desde el backend.
        /// Usado por el proxy de imágenes del frontend (TiendaController.Imagen).
        /// </summary>
        public async Task<(byte[] Bytes, string ContentType)?> ObtenerImagenBytesAsync(int productoId)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"api/productoImagen/producto/{productoId}/principal");
                if (!response.IsSuccessStatusCode) return null;

                var bytes = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
                return (bytes, contentType);
            }
            catch
            {
                return null;
            }
        }

        // DTO interno para api/categorias/{id}/productos
        private sealed class ProductoEnCategoriaResponse
        {
            [JsonPropertyName("productoId")]
            public int ProductoId { get; set; }
        }

        // DTO interno para deserializar la respuesta de api/precios/vigente/{id}
        private sealed class PrecioVigenteResponse
        {
            [JsonPropertyName("productoId")]
            public int ProductoId { get; set; }

            [JsonPropertyName("monedaId")]
            public int MonedaId { get; set; }

            // El backend devuelve null si no hay precio registrado
            [JsonPropertyName("precio")]
            public decimal? Precio { get; set; }
        }
    }
}
