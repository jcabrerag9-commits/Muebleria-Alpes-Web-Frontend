using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels
{
    /// <summary>
    /// Mapea ProductoDTO de api/producto: { productoId, sku, nombre, descripcionCorta, estado, ... }
    /// Se usa en la vista de Inventario para mostrar el catálogo de productos disponibles.
    /// </summary>
    public class ExistenciaViewModel
    {
        [JsonPropertyName("productoId")]
        public int ProductoId { get; set; }

        [JsonPropertyName("sku")]
        public string? Sku { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcionCorta")]
        public string? DescripcionCorta { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("fechaRegistro")]
        public DateTime FechaRegistro { get; set; }

        // Estos campos vienen de api/inventario/existencia/{id} (consulta separada)
        public int CantidadDisponible { get; set; }
        public int CantidadReservada { get; set; }
        public string? BodegaNombre { get; set; }
    }

    public class BodegaViewModel
    {
        public int BodegaId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
    }

    public class MovimientoInventarioViewModel
    {
        public int ProductoId { get; set; }
        public int BodegaId { get; set; }
        public string? TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public string? Motivo { get; set; }
    }

    public class CrearMovimientoViewModel
    {
        [Required]
        public int ProductoId { get; set; }

        public int BodegaId { get; set; }

        [Required]
        public string? TipoMovimiento { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        public string? Motivo { get; set; }
    }

    public class EnvioViewModel
    {
        public int EnvioId { get; set; }
        public string? NumeroOrden { get; set; }
        public string? Transportista { get; set; }
        public string? Estado { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaEntregaEstimada { get; set; }
    }

    public class CrearEnvioViewModel
    {
        [Required]
        public int OrdenId { get; set; }

        [Required]
        public int DireccionId { get; set; }

        public string? Transportista { get; set; }
        public decimal CostoEnvio { get; set; }
        public string Estado { get; set; } = "PREPARANDO";
    }

    public class CortesCajaViewModel
    {
        public int CorteId { get; set; }
        public DateTime FechaCorte { get; set; }
        public decimal MontoInicial { get; set; }
        public decimal MontoFinal { get; set; }
        public decimal TotalVentas { get; set; }
        public string? Estado { get; set; }
    }

    public class AbrirCajaViewModel
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El monto inicial no puede ser negativo.")]
        public decimal MontoInicial { get; set; }
    }

    public class CerrarCajaViewModel
    {
        [Required]
        public int CorteId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El monto final no puede ser negativo.")]
        public decimal MontoFinal { get; set; }

        public string? Observacion { get; set; }
    }
}
