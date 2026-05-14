using System.ComponentModel.DataAnnotations;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos
{
    public class AsistenciaViewModel
    {
        public int AsistenciaId { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime Fecha { get; set; }
        public string? HoraEntrada { get; set; }
        public string? HoraSalida { get; set; }
        public string? Estado { get; set; }
    }

    public class CrearAsistenciaViewModel
    {
        [Required]
        public int EmpleadoId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string? HoraEntrada { get; set; }
        public string? HoraSalida { get; set; }
        public string Estado { get; set; } = "PRESENTE";
        public int UsuarioId { get; set; } = 1;
    }
}
