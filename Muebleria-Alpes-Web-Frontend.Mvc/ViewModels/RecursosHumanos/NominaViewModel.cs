using System.ComponentModel.DataAnnotations;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos
{
    public class NominaViewModel
    {
        public int NominaId { get; set; }
        public string? Periodo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? Estado { get; set; }
    }

    public class NominaDetalleViewModel
    {
        public int DetalleId { get; set; }
        public string? EmpCodigo { get; set; }
        public string? Nombre { get; set; }
        public decimal SalarioBase { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal SalarioNeto { get; set; }
        public string? Estado { get; set; }
    }

    public class CrearNominaViewModel
    {
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        public int UsuarioId { get; set; } = 1;
    }

    public class CambiarEstadoNominaViewModel
    {
        public string Estado { get; set; } = string.Empty;
        public int UsuarioId { get; set; } = 1;
    }
}
