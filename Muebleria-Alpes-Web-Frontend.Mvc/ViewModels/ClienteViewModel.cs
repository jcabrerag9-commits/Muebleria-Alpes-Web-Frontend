using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels
{
    /// <summary>
    /// Mapea el modelo Cliente del backend (retornado directamente por api/clientes GET).
    /// Campos: id, codigo, tipoClienteId, tipoDocumentoId, numeroDocumento,
    ///         primerNombre, segundoNombre, primerApellido, segundoApellido,
    ///         fechaNacimiento, genero, estado.
    /// </summary>
    public class ClienteViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("primerNombre")]
        public string? PrimerNombre { get; set; }

        [JsonPropertyName("segundoNombre")]
        public string? SegundoNombre { get; set; }

        [JsonPropertyName("primerApellido")]
        public string? PrimerApellido { get; set; }

        [JsonPropertyName("segundoApellido")]
        public string? SegundoApellido { get; set; }

        [JsonPropertyName("numeroDocumento")]
        public string? NumeroDocumento { get; set; }

        [JsonPropertyName("tipoDocumentoId")]
        public int TipoDocumentoId { get; set; }

        [JsonPropertyName("genero")]
        public string? Genero { get; set; }

        [JsonPropertyName("fechaNacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        // Computed — no viene del backend
        public string NombreCompleto =>
            string.Join(" ", new[] { PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido }
                .Where(s => !string.IsNullOrWhiteSpace(s)));

        public string Iniciales =>
            string.Concat(new[] { PrimerNombre, PrimerApellido }
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s![0].ToString().ToUpper()));
    }

    public class CrearClienteViewModel
    {
        [Required]
        [JsonPropertyName("primerNombre")]
        public string PrimerNombre { get; set; } = string.Empty;

        [JsonPropertyName("segundoNombre")]
        public string? SegundoNombre { get; set; }

        [Required]
        [JsonPropertyName("primerApellido")]
        public string PrimerApellido { get; set; } = string.Empty;

        [JsonPropertyName("segundoApellido")]
        public string? SegundoApellido { get; set; }

        [JsonPropertyName("tipoDocumentoId")]
        public int TipoDocumentoId { get; set; } = 1;

        [Required]
        [JsonPropertyName("numeroDocumento")]
        public string NumeroDocumento { get; set; } = string.Empty;

        [JsonPropertyName("fechaNacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [JsonPropertyName("tipoClienteId")]
        public int TipoClienteId { get; set; } = 1;

        [JsonPropertyName("genero")]
        public string Genero { get; set; } = "M";
    }

    public class CambiarEstadoClienteViewModel
    {
        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;

        [JsonPropertyName("motivo")]
        public string Motivo { get; set; } = string.Empty;

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; } = 1;
    }
}
