namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels
{
    public class ApiResponse<T>
    {
        public string? Mensaje { get; set; }
        public T? Resultado { get; set; }
    }
}
