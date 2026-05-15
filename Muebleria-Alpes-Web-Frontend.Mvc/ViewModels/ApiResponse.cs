namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels
{
    /// <summary>
    /// Wrapper universal para respuestas de la API ERP (Versión Frontend)
    /// </summary>
    /// <typeparam name="T">Tipo de dato contenido en Data</typeparam>
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        // --- ALIAS DE COMPATIBILIDAD (H.4) ---
        public bool IsSuccess => Success;
        public string Mensaje { get => Message; set => Message = value; }
        public T? Resultado { get => Data; set => Data = value; }
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
