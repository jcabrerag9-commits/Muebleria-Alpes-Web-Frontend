using System.ComponentModel.DataAnnotations;
namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [MinLength(4, ErrorMessage = "Mínimo 4 caracteres")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirma tu contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string ConfirmarPassword { get; set; } = string.Empty;

        public string? Error { get; set; }
    }
}
