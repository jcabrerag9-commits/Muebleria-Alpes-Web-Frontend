using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers;

[Authorize(Policy = "SoloAdmin")]
public class EnviosController : Controller
{
    private readonly EnviosModuloApiService _service;
    public EnviosController(EnviosModuloApiService service) => _service = service;

    public async Task<IActionResult> Index(string estado = "PREPARANDO")
    {
        ViewData["Title"] = "Envíos";
        ViewBag.Estado = estado;
        return View(await _service.ListarPorEstadoAsync(estado));
    }

    public async Task<IActionResult> Crear()
    {
        ViewData["Title"] = "Crear envío";
        ViewBag.Ordenes = await _service.OrdenesDisponiblesAsync();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(int ordenVentaId, int clienteDireccionId, string numeroGuia, string transportista, decimal costoEnvio, string estado = "PREPARANDO")
    {
        var usuarioId = int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var r = await _service.CrearAsync(ordenVentaId, clienteDireccionId, numeroGuia, transportista, costoEnvio, estado, usuarioId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int envioId, string estado)
    {
        var usuarioId = int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var r = await _service.CambiarEstadoAsync(envioId, estado, usuarioId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Index), new { estado });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarEntrega(int envioId)
    {
        var usuarioId = int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var r = await _service.ConfirmarEntregaAsync(envioId, usuarioId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Index), new { estado = "ENTREGADO" });
    }

    [HttpGet]
    public async Task<IActionResult> DireccionesCliente(int clienteId)
    {
        var direcciones = await _service.DireccionesClienteAsync(clienteId);
        return Json(direcciones);
    }
}
