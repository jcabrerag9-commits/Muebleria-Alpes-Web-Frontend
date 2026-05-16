using System.ComponentModel.DataAnnotations;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos
{
    public class VacacionViewModel
    {
        public int VacacionId { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasSolicitados { get; set; }
        public string? Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CrearVacacionViewModel
    {
        [Required]
        public int EmpleadoId { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public int Dias { get; set; }
        public int UsuarioId { get; set; } = 1;
    }

    public class AprobarRechazarVacacionViewModel
    {
        public string? Motivo { get; set; }
        public int UsuarioId { get; set; } = 1;
    }
}
