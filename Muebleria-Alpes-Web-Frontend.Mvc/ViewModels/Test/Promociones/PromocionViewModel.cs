namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Promociones
{
    public class PromocionViewModel
    {
        public long PrmPromocion { get; set; }
        public string PrmCodigo { get; set; } = string.Empty;
        public string PrmNombre { get; set; } = string.Empty;
        public string? PrmDescripcion { get; set; }
        public string PrmTipo { get; set; } = string.Empty;
        public decimal? PrmValor { get; set; }
        public DateTime PrmFechaInicio { get; set; }
        public DateTime PrmFechaFin { get; set; }
        public string PrmEstado { get; set; } = string.Empty;
        public bool EstaVigente { get; set; }
        public List<PromocionProductoViewModel> Productos { get; set; } = [];

        public string TipoDisplay => PrmTipo switch
        {
            "PORCENTAJE"    => $"{PrmValor}% OFF",
            "MONTO_FIJO"   => $"Q{PrmValor} OFF",
            "2X1"          => "2x1",
            "ENVIO_GRATIS" => "Envío gratis",
            "COMPRA_MINIMA"=> $"Compra mínima Q{PrmValor}",
            _              => PrmTipo
        };
    }

    public class PromocionProductoViewModel
    {
        public long PpoPromocionProducto { get; set; }
        public long? ProProducto { get; set; }
        public long? PvaProductoVariante { get; set; }
        public string PpoEstado { get; set; } = string.Empty;
    }

    public class PromocionCreateViewModel
    {
        public string PrmNombre { get; set; } = string.Empty;
        public string? PrmDescripcion { get; set; }
        public string PrmTipo { get; set; } = string.Empty;
        public decimal? PrmValor { get; set; }
        public DateTime PrmFechaInicio { get; set; } = DateTime.Today;
        public DateTime PrmFechaFin { get; set; } = DateTime.Today.AddMonths(1);
    }

    public class PromocionUpdateViewModel
    {
        public string? PrmNombre { get; set; }
        public string? PrmDescripcion { get; set; }
        public decimal? PrmValor { get; set; }
        public DateTime? PrmFechaInicio { get; set; }
        public DateTime? PrmFechaFin { get; set; }
        public string? PrmEstado { get; set; }
    }

    public class PromocionIndexViewModel
    {
        public List<PromocionViewModel> Promociones { get; set; } = [];
        public int TotalActivas { get; set; }
        public int TotalVigentes { get; set; }
        public int TotalInactivas { get; set; }
        public string? FiltroEstado { get; set; }
        public string? FiltroTipo { get; set; }
    }

    public class BannerViewModel
    {
        public long BanBanner { get; set; }
        public string BanTitulo { get; set; } = string.Empty;
        public string BanImagenUrl { get; set; } = string.Empty;
        public string? BanEnlace { get; set; }
        public string? BanPosicion { get; set; }
        public DateTime BanFechaInicio { get; set; }
        public DateTime? BanFechaFin { get; set; }
        public string BanEstado { get; set; } = string.Empty;
        public bool EstaVigente { get; set; }
        public int TotalClicks { get; set; }
    }

    public class BannerCreateViewModel
    {
        public string BanTitulo { get; set; } = string.Empty;
        public string BanImagenUrl { get; set; } = string.Empty;
        public string? BanEnlace { get; set; }
        public string? BanPosicion { get; set; }
        public DateTime BanFechaInicio { get; set; } = DateTime.Today;
        public DateTime? BanFechaFin { get; set; }
    }
}