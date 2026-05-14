namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Devoluciones
{
    public class DevolucionViewModel
    {
        public long DevDevolucion { get; set; }
        public long VenOrdenVenta { get; set; }
        public long CliCliente { get; set; }
        public long CtdCategoriaTipoDev { get; set; }
        public string? NombreCategoria { get; set; }
        public string DevNumeroRma { get; set; } = string.Empty;
        public string DevMotivo { get; set; } = string.Empty;
        public decimal DevMontoTotal { get; set; }
        public string DevEstado { get; set; } = string.Empty;
        public DateTime DevFechaCreacion { get; set; }
        public List<DevolucionDetalleViewModel> Detalles { get; set; } = [];

        public string EstadoBadgeClass => DevEstado switch
        {
            "SOLICITADA"  => "badge-warning",
            "EN_REVISION" => "badge-info",
            "APROBADA"    => "badge-success",
            "RECHAZADA"   => "badge-danger",
            "COMPLETADA"  => "badge-secondary",
            _             => "badge-light"
        };

        public string[] EstadosSiguientes => DevEstado switch
        {
            "SOLICITADA"  => ["EN_REVISION", "RECHAZADA"],
            "EN_REVISION" => ["APROBADA", "RECHAZADA"],
            "APROBADA"    => ["COMPLETADA"],
            _             => []
        };
    }

    public class DevolucionDetalleViewModel
    {
        public long DdeDevolucionDetalle { get; set; }
        public long VdeOrdenVentaDetalle { get; set; }
        public decimal DdeCantidad { get; set; }
        public decimal DdeMonto { get; set; }
        public string DdeEstado { get; set; } = string.Empty;
        public DateTime DdeFechaCreacion { get; set; }
    }

    public class CategoriaDevolucionViewModel
    {
        public long CtdCategoriaTipoDev { get; set; }
        public string CtdCodigo { get; set; } = string.Empty;
        public string CtdNombre { get; set; } = string.Empty;
        public string? CtdDescripcion { get; set; }
        public string CtdEstado { get; set; } = string.Empty;
    }

    public class DevolucionCreateViewModel
    {
        public long VenOrdenVenta { get; set; }
        public long CliCliente { get; set; }
        public long CtdCategoriaTipoDev { get; set; }
        public string DevMotivo { get; set; } = string.Empty;
        public List<DetalleCreateViewModel> Detalles { get; set; } = [new()];
    }

    public class DetalleCreateViewModel
    {
        public long VdeOrdenVentaDetalle { get; set; }
        public decimal DdeCantidad { get; set; }
        public decimal DdeMonto { get; set; }
    }

    public class DevolucionIndexViewModel
    {
        public List<DevolucionViewModel> Devoluciones { get; set; } = [];
        public List<CategoriaDevolucionViewModel> Categorias { get; set; } = [];
        public int TotalSolicitadas { get; set; }
        public int TotalAprobadas { get; set; }
        public int TotalRechazadas { get; set; }
        public int TotalCompletadas { get; set; }
        public string? FiltroEstado { get; set; }
    }

    public class CategoriaIndexViewModel
    {
        public List<CategoriaDevolucionViewModel> Categorias { get; set; } = [];
    }
}