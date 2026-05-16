namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Shared
{
    public class BackendResponse<T>
    {
        public bool Success { get; set; }
        public string? Resultado { get; set; }
        public string? Mensaje { get; set; }
        public T? Data { get; set; }
        public bool IsSuccess => Success || Resultado == "EXITO" || Resultado == "OK";
    }
}
