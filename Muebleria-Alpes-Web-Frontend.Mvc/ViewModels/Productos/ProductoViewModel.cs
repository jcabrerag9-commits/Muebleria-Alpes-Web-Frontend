namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos
{
    public class ProductoViewModel
    {
        public int ProductoId { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string DescripcionCorta { get; set; } = string.Empty;
        public string DescripcionLarga { get; set; } = string.Empty;
        public decimal? Peso { get; set; }
        public string EsConfigurable { get; set; } = "N";
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        
        public int? TipoMuebleId { get; set; }
        public string? TipoMuebleNombre { get; set; }
        public string? ImagenUrl { get; set; }
    }

    public class CrearProductoViewModel
    {
        public int TipoMueble { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? DescripcionCorta { get; set; }
        public string? DescripcionLarga { get; set; }
        public decimal? Peso { get; set; }
        public string EsConfigurable { get; set; } = "N";
    }

    public class ActualizarProductoViewModel
    {
        public int TipoMueble { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? DescripcionCorta { get; set; }
        public string? DescripcionLarga { get; set; }
        public decimal? Peso { get; set; }
        public string EsConfigurable { get; set; } = "N";
    }

    public class ColorViewModel
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? HexColor { get; set; }
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = "ACTIVO";
    }

    public class CrearColorViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string? HexColor { get; set; }
        public string? Descripcion { get; set; }
    }

    public class ActualizarColorViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string? HexColor { get; set; }
        public string? Descripcion { get; set; }
    }

    public class MaterialViewModel
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = "ACTIVO";
    }

    public class CrearMaterialViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class ActualizarMaterialViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class CategoriaViewModel
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class CrearCategoriaViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class ActualizarCategoriaViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
