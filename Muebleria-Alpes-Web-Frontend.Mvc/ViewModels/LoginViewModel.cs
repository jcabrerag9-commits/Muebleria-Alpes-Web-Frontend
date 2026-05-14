using System.ComponentModel.DataAnnotations;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "El usuario es requerido")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "La contraseña es requerida")]
    public string Password { get; set; } = "";

    public string? ReturnUrl { get; set; }
    public string? Error { get; set; }
}
