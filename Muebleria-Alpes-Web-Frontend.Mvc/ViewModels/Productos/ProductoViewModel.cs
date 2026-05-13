using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos
{
    /// <summary>
    /// Mapea el modelo Producto del backend: { id, tipoMueble, sku, nombre, descripcionCorta, estado, publicado, ... }
    /// El backend serializa en camelCase por defecto.
    /// </summary>
    public class ProductoViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("tipoMueble")]
        public int TipoMuebleId { get; set; }

        [JsonPropertyName("sku")]
        public string? Sku { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcionCorta")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("descripcionLarga")]
        public string? DescripcionLarga { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("publicado")]
        public string? Publicado { get; set; }

        [JsonPropertyName("esConfigurable")]
        public string? EsConfigurable { get; set; }
    }

    public class CrearProductoViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcionCorta")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("sku")]
        public string? Sku { get; set; }

        [JsonPropertyName("tipoMueble")]
        public int TipoMueble { get; set; } = 1;

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = "BORRADOR";
    }

    public class ActualizarProductoViewModel
    {
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcionCorta")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("sku")]
        public string? Sku { get; set; }
    }

    public class CategoriaViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }
    }

    public class CrearCategoriaViewModel
    {
        // Codigo se genera automáticamente en el backend a partir del Nombre
        [Required]
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
    }

    public class ActualizarCategoriaViewModel
    {
        [Required]
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
    }

    public class ColorViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("hexColor")]
        public string? HexColor { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }
    }

    public class CrearColorViewModel
    {
        [Required]
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("hexColor")]
        public string? HexColor { get; set; }
    }

    public class ActualizarColorViewModel
    {
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("hexColor")]
        public string? HexColor { get; set; }
    }

    public class MaterialViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }
    }

    public class CrearMaterialViewModel
    {
        [Required]
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
    }

    public class ActualizarMaterialViewModel
    {
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
    }
}
