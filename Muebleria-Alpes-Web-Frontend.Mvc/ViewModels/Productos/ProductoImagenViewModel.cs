namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos
{
    public class ProductoImagenViewModel
    {
        public int ImagenId { get; set; }
        public int ProductoId { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Tamanio { get; set; }
        public string? Url { get; set; }
        public string? Tipo { get; set; }
        public int Orden { get; set; }
        public DateTime FechaCarga { get; set; }
        public bool EsPrincipal => Tipo == "PRINCIPAL";
    }

    public class UploadImagenViewModel
    {
        public int ProductoId { get; set; }
        public IFormFile Archivo { get; set; } = null!;
        public string? Tipo { get; set; }
        public int Orden { get; set; } = 1;
    }
}
