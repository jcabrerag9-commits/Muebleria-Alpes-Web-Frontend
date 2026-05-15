using System.Text.Json;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Shared;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos
{
    // Local DTO that mirrors the backend Producto domain model field names.
    file record ProductoDto(
        int Id,
        int TipoMueble,
        string? Sku,
        string Nombre,
        string? DescripcionCorta,
        string? DescripcionLarga,
        decimal? Peso,
        string EsConfigurable,
        string Estado
    );

    public class ProductoApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

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
                    Console.WriteLine($"[ProductoApiService] ListarAsync ERROR {response.StatusCode}: {raw}");
                    return new List<ProductoViewModel>();
                }

                // Backend returns a plain array: [{...}, {...}]
                var dtos = JsonSerializer.Deserialize<List<ProductoDto>>(raw, _json);
                if (dtos == null) return new List<ProductoViewModel>();

                return dtos.Select(d => new ProductoViewModel
                {
                    ProductoId       = d.Id,
                    Sku              = d.Sku ?? string.Empty,
                    Nombre           = d.Nombre,
                    DescripcionCorta = d.DescripcionCorta ?? string.Empty,
                    DescripcionLarga = d.DescripcionLarga ?? string.Empty,
                    Peso             = d.Peso,
                    EsConfigurable   = d.EsConfigurable,
                    Estado           = d.Estado,
                    TipoMuebleId     = d.TipoMueble,
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoApiService] ListarAsync EXCEPTION: {ex.Message}");
                return new List<ProductoViewModel>();
            }
        }

        public async Task<ProductoViewModel?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Productos/{id}");
                var raw = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[ProductoApiService] ObtenerPorId({id}) ERROR {response.StatusCode}: {raw}");
                    return null;
                }

                var dto = JsonSerializer.Deserialize<ProductoDto>(raw, _json);
                if (dto == null) return null;

                return new ProductoViewModel
                {
                    ProductoId       = dto.Id,
                    Sku              = dto.Sku ?? string.Empty,
                    Nombre           = dto.Nombre,
                    DescripcionCorta = dto.DescripcionCorta ?? string.Empty,
                    DescripcionLarga = dto.DescripcionLarga ?? string.Empty,
                    Peso             = dto.Peso,
                    EsConfigurable   = dto.EsConfigurable,
                    Estado           = dto.Estado,
                    TipoMuebleId     = dto.TipoMueble,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoApiService] ObtenerPorId({id}) EXCEPTION: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CrearAsync(CrearProductoViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Productos", model);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoApiService] CrearAsync EXCEPTION: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarProductoViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Productos/{id}", model);
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ProductoApiService] ActualizarAsync({id}) ERROR {response.StatusCode}: {err}");
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoApiService] ActualizarAsync({id}) EXCEPTION: {ex.Message}");
                return false;
            }
        }
    }
}
