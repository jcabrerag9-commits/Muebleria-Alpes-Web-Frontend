using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

public class EnvioModuloViewModel
{
    [JsonPropertyName("envioId")] public int EnvioId { get; set; }
    [JsonPropertyName("ordenVentaId")] public int OrdenVentaId { get; set; }
    [JsonPropertyName("clienteDireccionId")] public int ClienteDireccionId { get; set; }
    [JsonPropertyName("numeroGuia")] public string? NumeroGuia { get; set; }
    [JsonPropertyName("transportista")] public string? Transportista { get; set; }
    [JsonPropertyName("costoEnvio")] public decimal CostoEnvio { get; set; }
    [JsonPropertyName("fechaEnvio")] public DateTime? FechaEnvio { get; set; }
    [JsonPropertyName("fechaEntregaEstimada")] public DateTime? FechaEntregaEstimada { get; set; }
    [JsonPropertyName("fechaEntregaReal")] public DateTime? FechaEntregaReal { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
    [JsonPropertyName("observaciones")] public string? Observaciones { get; set; }
}
