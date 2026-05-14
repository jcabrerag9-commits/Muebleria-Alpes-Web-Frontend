using System.ComponentModel.DataAnnotations;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos
{
    public class PuestoViewModel
    {
        public int PuestoId { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
    }

    public class CrearPuestoViewModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int UsuarioId { get; set; } = 1;
    }

    public class ActualizarPuestoViewModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int UsuarioId { get; set; } = 1;
    }

    public class CambiarEstadoPuestoViewModel
    {
        public string Estado { get; set; } = string.Empty;
        public int UsuarioId { get; set; } = 1;
    }
}
