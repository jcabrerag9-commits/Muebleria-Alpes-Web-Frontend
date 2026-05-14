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
}
