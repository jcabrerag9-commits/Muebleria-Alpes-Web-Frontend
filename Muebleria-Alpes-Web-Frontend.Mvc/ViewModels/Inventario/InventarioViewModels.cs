using System;
using System.Collections.Generic;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Inventario
{
    public class ExistenciaViewModel
    {
        public int ProductoId { get; set; }
        public int? VarianteId { get; set; }
        public int BodegaId { get; set; }
        public string NombreBodega { get; set; } = string.Empty;
        /// <summary>Cantidad libre para prometer a nuevas ventas (ATP)</summary>
        public int CantidadDisponible { get; set; }
        /// <summary>Suma agregada de reservas activas. No individual, solo el total por bodega.</summary>
        public int CantidadReservada { get; set; }
        /// <summary>Total fisico = Disponible + Reservado</summary>
        public int TotalFisico => CantidadDisponible + CantidadReservada;
        public DateTime UltimaActualizacion { get; set; }
    }

    /// <summary>
    /// Reserva individual con clasificacion por Motivo.
    /// Motivos validos: CARRITO | APARTADO_MANUAL | ORDEN_CONFIRMADA | RESERVA_TEMPORAL
    /// </summary>
    public class ReservaViewModel
    {
        public int ReservaId { get; set; }
        public int ProductoId { get; set; }
        public string? NombreProducto { get; set; }
        public int? VarianteId { get; set; }
        public int BodegaId { get; set; }
        public string? NombreBodega { get; set; }
        public int? ClienteId { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = "RESERVA_TEMPORAL";
        public string Estado { get; set; } = "ACTIVO";
        public DateTime? Expiracion { get; set; }
        public DateTime FechaCreacion { get; set; }

        /// <summary>CSS class para el badge de Bootstrap segun el motivo</summary>
        public string BadgeClass => Motivo switch
        {
            "CARRITO"           => "bg-warning text-dark",
            "APARTADO_MANUAL"   => "bg-info text-dark",
            "ORDEN_CONFIRMADA"  => "bg-danger",
            _                   => "bg-secondary"
        };

        public string MotivoLabel => Motivo switch
        {
            "CARRITO"           => "Carrito",
            "APARTADO_MANUAL"   => "Apartado Manual",
            "ORDEN_CONFIRMADA"  => "Orden Confirmada",
            "RESERVA_TEMPORAL"  => "Temporal",
            _                   => Motivo
        };

        /// <summary>
        /// Regla ERP: solo APARTADO_MANUAL y RESERVA_TEMPORAL pueden liberarse manualmente.
        /// CARRITO es administrado por el sistema (job de expiracion).
        /// ORDEN_CONFIRMADA es stock comprometido comercialmente — intocable hasta despacho.
        /// </summary>
        public bool EsLiberableManualmente => Motivo?.ToUpper() switch
        {
            "APARTADO_MANUAL"  => true,
            "RESERVA_TEMPORAL" => true,
            _                  => false
        };

        /// <summary>Tooltip descriptivo para motivos bloqueados.</summary>
        public string TooltipBloqueo => Motivo?.ToUpper() switch
        {
            "ORDEN_CONFIRMADA" => "Stock comprometido por una Orden de Venta. Se libera al completar el despacho.",
            "CARRITO"          => "Reserva temporal de carrito. El sistema la libera automaticamente al expirar.",
            _                  => "Esta reserva es administrada automaticamente por el sistema."
        };

        // Nuevos campos de auditoría de liberación
        public string? UsuarioLiberacion { get; set; }
        public DateTime? FechaLiberacion { get; set; }
        public string? ObservacionLiberacion { get; set; }
        public int? OrdenVentaId { get; set; }
    }

    /// <summary>
    /// Modelo para el historial de movimientos (Kardex)
    /// </summary>
    public class KardexMovimientoViewModel
    {
        public int MovimientoId { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int? StockAnterior { get; set; }
        public string? Sku { get; set; }
        public string? NombreProducto { get; set; }
        public int? StockNuevo { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? Observacion { get; set; }
        public string? NombreBodega { get; set; }
        public int? BodegaId { get; set; }
        public int? UsuarioId { get; set; }
        public int? OrdenVentaId { get; set; }

        public string BadgeClass => TipoMovimiento.ToUpper() switch
        {
            "ENTRADA"    => "bg-success",
            "SALIDA"     => "bg-danger",
            "RESERVA"    => "bg-warning text-dark",
            "LIBERACION" => "bg-info text-dark",
            "AJUSTE"     => "bg-dark",
            _            => "bg-secondary"
        };

        public string Icon => TipoMovimiento.ToUpper() switch
        {
            "ENTRADA"    => "bi-box-arrow-in-down",
            "SALIDA"     => "bi-box-arrow-up",
            "RESERVA"    => "bi-lock",
            "LIBERACION" => "bi-unlock",
            "AJUSTE"     => "bi-wrench-adjustable",
            _            => "bi-arrow-left-right"
        };
    }

    public class MovimientoGlobalViewModel
    {
        public List<KardexMovimientoViewModel> Movimientos { get; set; } = new();
        public List<BodegaViewModel> Bodegas { get; set; } = new();
        public List<dynamic> TiposMovimiento { get; set; } = new(); // Para el filtro
        
        // Filtros actuales
        public int? BodegaId { get; set; }
        public string? FechaDesde { get; set; }
        public string? FechaHasta { get; set; }
        public string? TipoMovimiento { get; set; }
    }

    public class BodegaViewModel
    {
        public int? BodegaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = "ACTIVO";

        // Nuevos campos ERP
        public string Tipo { get; set; } = "FISICA"; // FISICA | VIRTUAL
        public int? CanalVentaId { get; set; }
        public string? CanalVentaNombre { get; set; }
        public string PermiteReserva { get; set; } = "S";
        public string PermiteVenta { get; set; } = "S";
        public string ManejaDespacho { get; set; } = "S";
        public string? MotivoCierre { get; set; }

        // Estadísticas
        public int StockTotal { get; set; }
        public int ReservasActivas { get; set; }
        public string? Ubicacion { get; set; }
    }

    /// <summary>ViewModel combinado que pasa al partial _PanelExistencias</summary>
    public class PanelExistenciasViewModel
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public List<ExistenciaViewModel> Existencias { get; set; } = new();
        public List<ReservaViewModel> Reservas { get; set; } = new();
    }

    public class MovimientoInventarioViewModel
    {
        public int ProductoId { get; set; }
        public int? VarianteId { get; set; }
        public int BodegaId { get; set; } = 1;
        public int Cantidad { get; set; }
        public decimal? CostoUnitario { get; set; }
        public int? OrdenVentaId { get; set; }
        public string? Observacion { get; set; }
        public int? UsuarioId { get; set; }
        public string TipoMovimiento { get; set; } = "ENTRADA";
    }

    public class ReservaStockViewModel
    {
        public int ProductoId { get; set; }
        public int? VarianteId { get; set; }
        public int BodegaId { get; set; } = 1;
        public int? ClienteId { get; set; } = 1;
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = "APARTADO_MANUAL";
        public DateTime? Expiracion { get; set; }
        public int? UsuarioId { get; set; }
    }

    public class InventarioDashboardViewModel
    {
        public int ProductosStockBajo { get; set; }
        public int AtpTotal { get; set; }
        public int ReservasActivasCount { get; set; }
        public int MovimientosHoyCount { get; set; }
        public List<BodegaOcupacionViewModel> OcupacionBodegas { get; set; } = new();
    }

    public class BodegaOcupacionViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public int Porcentaje { get; set; }
        public int Cantidad { get; set; }
    }
}
