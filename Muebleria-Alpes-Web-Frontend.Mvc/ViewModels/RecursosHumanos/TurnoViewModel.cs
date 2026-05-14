using System.ComponentModel.DataAnnotations;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos
{
    public class TurnoViewModel
    {
        public int TurnoId { get; set; }
        public string? Nombre { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }
        public string? Estado { get; set; }
    }

    public class CrearTurnoViewModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }
        public int UsuarioId { get; set; } = 1;
    }
}
