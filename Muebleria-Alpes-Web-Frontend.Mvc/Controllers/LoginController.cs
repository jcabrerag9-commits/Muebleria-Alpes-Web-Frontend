using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Security.Claims;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers;

public class LoginController : Controller
{
    private readonly AuthApiService _authService;

    public LoginController(AuthApiService authService) => _authService = authService;

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var rol = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            return rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase)
                ? RedirectToAction("Index", "Tienda")
                : RedirectToAction("Index", "Home");
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _authService.LoginAsync(model.Username, model.Password);

        if (!result.Exitoso)
        {
            model.Error = result.Mensaje;
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.Id.ToString()),
            new(ClaimTypes.Name,           result.Username),
            new(ClaimTypes.GivenName,      result.NombreCompleto),
            new(ClaimTypes.Role,           result.Rol),
            // CLI_CLIENTE para el carrito de compra
            new("clienteId",               result.ClienteId.ToString()),
            new("tokenSesion",            result.TokenSesion),
            new("sesionId",               result.SesionId.ToString()),
            new("clienteId",               result.ClienteId.ToString())
        };

        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        if (result.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
            return RedirectToAction("Index", "Tienda");

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Registro()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var rol = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            return rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase)
                ? RedirectToAction("Index", "Tienda")
                : RedirectToAction("Index", "Home");
        }

        return View(new RegistroViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(RegistroViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _authService.RegistrarAsync(model.Username, model.Password);

        if (!result.Exitoso)
        {
            model.Error = result.Mensaje;
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.Id.ToString()),
            new(ClaimTypes.Name,           result.Username),
            new(ClaimTypes.GivenName,      result.NombreCompleto),
            new(ClaimTypes.Role,           result.Rol),
            new("clienteId",               result.ClienteId.ToString())
        };

        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Tienda");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        var tokenSesion = User.FindFirst("tokenSesion")?.Value;
        await _authService.CerrarSesionAsync(tokenSesion);

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Login");
    }
}
