namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos
{
    public class EmpleadoViewModel
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string? Estado { get; set; }
    }

    public class CrearEmpleadoViewModel
    {
        public string Codigo { get; set; } = string.Empty;
        public int TipoDocumentoId { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public string PrimerNombre { get; set; } = string.Empty;
        public string? SegundoNombre { get; set; }
        public string PrimerApellido { get; set; } = string.Empty;
        public string? SegundoApellido { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
    }

    public class ActualizarEmpleadoViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }

    public class CambiarEstadoEmpleadoViewModel
    {
        public string Estado { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public int UsuarioId { get; set; } = 1;
    }
}
