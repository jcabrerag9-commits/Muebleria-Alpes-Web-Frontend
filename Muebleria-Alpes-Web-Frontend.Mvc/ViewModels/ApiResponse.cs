using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels
{
    /// <summary>
    /// Formato RRHH: { mensaje, resultado } — usado por api/rh/*
    /// </summary>
    public class ApiResponse<T>
    {
        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }

        [JsonPropertyName("resultado")]
        public T? Resultado { get; set; }
    }

    /// <summary>
    /// Formato usado por api/promociones, api/devoluciones: { success, data }
    /// </summary>
    public class SuccessDataResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    /// <summary>
    /// Formato InventarioResponse: { Resultado, Mensaje, Data } — usado por api/producto, api/inventario/*
    /// </summary>
    public class InventarioApiResponse<T>
    {
        [JsonPropertyName("resultado")]
        public string? Resultado { get; set; }

        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        public bool IsSuccess => Resultado == "EXITO";
    }
}
