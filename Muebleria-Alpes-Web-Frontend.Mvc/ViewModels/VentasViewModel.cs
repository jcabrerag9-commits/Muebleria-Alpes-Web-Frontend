using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels
{
    public class OrdenViewModel
    {
        [JsonPropertyName("ordenId")]
        public int OrdenId { get; set; }

        [JsonPropertyName("numeroOrden")]
        public string? NumeroOrden { get; set; }

        [JsonPropertyName("fechaOrden")]
        public DateTime FechaOrden { get; set; }

        [JsonPropertyName("cliente")]
        public string? Cliente { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }
    }

    public class PagoViewModel
    {
        [JsonPropertyName("pagoId")]
        public int PagoId { get; set; }

        [JsonPropertyName("numeroOrden")]
        public string? NumeroOrden { get; set; }

        [JsonPropertyName("metodo")]
        public string? Metodo { get; set; }

        [JsonPropertyName("monto")]
        public decimal Monto { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("fechaPago")]
        public DateTime FechaPago { get; set; }
    }

    public class FacturaViewModel
    {
        [JsonPropertyName("facturaId")]
        public int FacturaId { get; set; }

        [JsonPropertyName("numero")]
        public string? Numero { get; set; }

        [JsonPropertyName("serie")]
        public string? Serie { get; set; }

        [JsonPropertyName("numeroOrden")]
        public string? NumeroOrden { get; set; }

        [JsonPropertyName("cliente")]
        public string? Cliente { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("fechaEmision")]
        public DateTime FechaEmision { get; set; }
    }

    public class AdminOrdenesDataViewModel
    {
        [JsonPropertyName("ordenes")]
        public List<OrdenViewModel> Ordenes { get; set; } = new();
    }

    public class AdminPagosDataViewModel
    {
        [JsonPropertyName("pagos")]
        public List<PagoViewModel> Pagos { get; set; } = new();
    }

    public class AdminFacturasDataViewModel
    {
        [JsonPropertyName("facturas")]
        public List<FacturaViewModel> Facturas { get; set; } = new();
    }

    public class ApiResultViewModel<T>
    {
        [JsonPropertyName("exitoso")]
        public bool Exitoso { get; set; }

        [JsonPropertyName("resultado")]
        public T? Resultado { get; set; }

        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }
    }
}
