using System.ComponentModel.DataAnnotations;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos
{
    public class EvaluacionViewModel
    {
        public int EvaluacionId { get; set; }
        public int EmpleadoId { get; set; }
        public string? NombreEmpleado { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public int Calificacion { get; set; }
        public string? Comentarios { get; set; }
    }

    public class CrearEvaluacionViewModel
    {
        [Required]
        public int EmpleadoId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        [Required]
        [Range(0, 100)]
        public int Calificacion { get; set; }
        public string? Comentarios { get; set; }
        public int UsuarioId { get; set; } = 1;
    }
}
