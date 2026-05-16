using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels
{
    /// <summary>
    /// Mapea la respuesta de api/categorias.
    /// ALP_CATEGORIA no tiene columna de estado.
    /// Implementa Equals/GetHashCode para poder usarse como clave de Dictionary.
    /// </summary>
    public class TiendaCategoriaViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        public override bool Equals(object? obj) =>
            obj is TiendaCategoriaViewModel other && other.Id == Id;

        public override int GetHashCode() => Id.GetHashCode();
    }


    /// <summary>
    /// Mapea el modelo Producto del backend: { id, tipoMueble, sku, nombre, descripcionCorta, descripcionLarga, estado, publicado, ... }
    /// </summary>
    public class TiendaProductoViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sku")]
        public string? Sku { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcionCorta")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("descripcionLarga")]
        public string? DescripcionLarga { get; set; }

        [JsonPropertyName("tipoMueble")]
        public int TipoMuebleId { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("publicado")]
        public string? Publicado { get; set; }

        // Calculados / enriquecidos en el frontend
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public bool TienePromocion { get; set; }
        public decimal? PrecioConDescuento { get; set; }

        // Helper para mostrar en la tienda
        public string NombreMostrar => Nombre ?? "Producto sin nombre";
        public bool EsPublicado => Publicado == "S";
    }

    /// <summary>
    /// Mapea el detalle del carrito devuelto por el backend (ObtenerCarritoClienteDataDto.Detalles).
    /// El backend serializa camelCase: detalleId, productoId, nombreProducto, cantidad, precioUnitario, subtotal.
    /// </summary>
    public class CarritoDetalleViewModel
    {
        [JsonPropertyName("detalleId")]
        public int DetalleId { get; set; }

        [JsonPropertyName("productoId")]
        public int ProductoId { get; set; }

        [JsonPropertyName("nombreProducto")]
        public string? ProductoNombre { get; set; }

        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }

        [JsonPropertyName("precioUnitario")]
        public decimal PrecioUnitario { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }
    }

    /// <summary>
    /// Mapea la cabecera del carrito (ObtenerCarritoClienteDataDto.Cabecera).
    /// El backend serializa camelCase: carritoId, subtotal, fechaCreacion.
    /// </summary>
    public class CarritoCabeceraViewModel
    {
        [JsonPropertyName("carritoId")]
        public int CarritoId { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonPropertyName("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }
    }

    /// <summary>
    /// Mapea ObtenerCarritoClienteDataDto: { cabecera, detalles }.
    /// "detalles" (plural) es el nombre que el backend serializa.
    /// </summary>
    public class CarritoViewModel
    {
        [JsonPropertyName("cabecera")]
        public CarritoCabeceraViewModel? Cabecera { get; set; }

        [JsonPropertyName("detalles")]
        public List<CarritoDetalleViewModel> Detalle { get; set; } = new();
    }

    public class AgregarCarritoViewModel
    {
        public int ClienteId { get; set; } = 1;
        public int ProductoId { get; set; }
        public int Cantidad { get; set; } = 1;
    }

    public class TotalCarritoViewModel
    {
        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonPropertyName("impuestos")]
        public decimal Impuestos { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

    public class OrdenClienteViewModel
    {
        [JsonPropertyName("ordenId")]
        public int OrdenId { get; set; }

        [JsonPropertyName("numeroOrden")]
        public string? NumeroOrden { get; set; }

        [JsonPropertyName("fechaOrden")]
        public DateTime FechaOrden { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("estadoOrden")]
        public string? EstadoOrden { get; set; }
    }

    // ── Direcciones de entrega ──────────────────────────────────────────────

    public class DireccionClienteViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("clienteId")]
        public int ClienteId { get; set; }

        [JsonPropertyName("ciudadId")]
        public int CiudadId { get; set; }

        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; }

        [JsonPropertyName("direccionLinea1")]
        public string? DireccionLinea1 { get; set; }

        [JsonPropertyName("direccionLinea2")]
        public string? DireccionLinea2 { get; set; }

        [JsonPropertyName("codigoPostal")]
        public string? CodigoPostal { get; set; }

        [JsonPropertyName("referencia")]
        public string? Referencia { get; set; }

        [JsonPropertyName("esPrincipal")]
        public string? EsPrincipal { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        public bool EsActiva => Estado == "ACTIVO";
        public bool EsPrincipalBool => EsPrincipal == "S";
    }

    public class ClienteDetalleApiResponse
    {
        [JsonPropertyName("direcciones")]
        public List<DireccionClienteViewModel> Direcciones { get; set; } = new();
    }

    /// <summary>
    /// Resumen de envío que se muestra al cliente en Mis Pedidos.
    /// Mapea la respuesta de api/Envios/orden/{ordenId}.
    /// </summary>
    public class EnvioResumenClienteViewModel
    {
        [JsonPropertyName("envioId")]
        public int EnvioId { get; set; }

        [JsonPropertyName("numeroGuia")]
        public string? NumeroGuia { get; set; }

        [JsonPropertyName("transportista")]
        public string? Transportista { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("fechaEnvio")]
        public DateTime? FechaEnvio { get; set; }

        [JsonPropertyName("fechaEntregaEstimada")]
        public DateTime? FechaEntregaEstimada { get; set; }

        [JsonPropertyName("fechaEntregaReal")]
        public DateTime? FechaEntregaReal { get; set; }
    }

    public class NuevaDireccionRequest
    {
        public int ClienteId { get; set; }
        public int PaisId { get; set; }
        public int DepartamentoId { get; set; }
        public int CiudadId { get; set; }
        public string Tipo { get; set; } = "ENVIO";
        public string DireccionLinea1 { get; set; } = string.Empty;
        public string? DireccionLinea2 { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Referencia { get; set; }
        public string EsPrincipal { get; set; } = "N";
        public string Estado { get; set; } = "ACTIVO";
    }
}
