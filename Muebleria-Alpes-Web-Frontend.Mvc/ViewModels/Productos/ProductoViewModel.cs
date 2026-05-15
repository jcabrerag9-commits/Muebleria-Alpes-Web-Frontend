using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos
{
    public class ProductoViewModel
    {
        [JsonPropertyName("id")]
        public int ProductoId { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string DescripcionCorta { get; set; } = string.Empty;
        public string DescripcionLarga { get; set; } = string.Empty;
        public decimal? Peso { get; set; }
        public string EsConfigurable { get; set; } = "N";
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        
        [JsonPropertyName("tipoMueble")]
        public int? TipoMuebleId { get; set; }
        public string? TipoMuebleNombre { get; set; }
        public string? ImagenUrl { get; set; }
    }

    public class CrearProductoViewModel
    {
        [JsonPropertyName("tipoMueble")]
        public int TipoMueble { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("descripcionCorta")]
        public string? DescripcionCorta { get; set; }

        [JsonPropertyName("descripcionLarga")]
        public string? DescripcionLarga { get; set; }

        [JsonPropertyName("peso")]
        public decimal? Peso { get; set; }

        [JsonPropertyName("esConfigurable")]
        public string EsConfigurable { get; set; } = "N";
    }

    public class ActualizarProductoViewModel
    {
        [JsonPropertyName("tipoMueble")]
        public int TipoMueble { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("descripcionCorta")]
        public string? DescripcionCorta { get; set; }

        [JsonPropertyName("descripcionLarga")]
        public string? DescripcionLarga { get; set; }

        [JsonPropertyName("peso")]
        public decimal? Peso { get; set; }

        [JsonPropertyName("esConfigurable")]
        public string EsConfigurable { get; set; } = "N";
    }
}
