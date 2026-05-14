using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers;

[Authorize(Policy = "SoloAdmin")]
public class SeguridadController : Controller
{
    private readonly SeguridadAlvaroApiService _service;
    public SeguridadController(SeguridadAlvaroApiService service) => _service = service;

    public async Task<IActionResult> Usuarios(string estado = "ACTIVO")
    {
        ViewData["Title"] = "Usuarios";
        ViewBag.Estado = estado;
        return View(await _service.ListarUsuariosAsync(estado));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearUsuario(string username, string passwordPlano, string estado = "ACTIVO")
    {
        var r = await _service.CrearUsuarioAsync(username, passwordPlano, estado);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Usuarios));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarUsuario(int usuarioId, string username, string estado)
    {
        var r = await _service.ActualizarUsuarioAsync(usuarioId, username, estado);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Usuarios), new { estado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarPassword(int usuarioId, string passwordPlano)
    {
        var r = await _service.CambiarPasswordAsync(usuarioId, passwordPlano);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Usuarios));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Bloquear(int usuarioId)
    {
        var r = await _service.BloquearUsuarioAsync(usuarioId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Usuarios));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desbloquear(int usuarioId)
    {
        var r = await _service.DesbloquearUsuarioAsync(usuarioId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Usuarios), new { estado = "INACTIVO" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InactivarUsuario(int usuarioId)
    {
        var r = await _service.InactivarUsuarioAsync(usuarioId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Usuarios));
    }

    public async Task<IActionResult> Roles(string estado = "ACTIVO")
    {
        ViewData["Title"] = "Roles";
        ViewBag.Estado = estado;
        return View(await _service.ListarRolesAsync(estado));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearRol(string nombre, string descripcion, string estado = "ACTIVO")
    {
        var r = await _service.CrearRolAsync(nombre, descripcion, estado);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Roles));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarRol(int rolId, string nombre, string descripcion, string estado)
    {
        var r = await _service.ActualizarRolAsync(rolId, nombre, descripcion, estado);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Roles), new { estado });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> InactivarRol(int rolId)
    {
        var r = await _service.InactivarRolAsync(rolId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Roles));
    }

    public async Task<IActionResult> Permisos(string estado = "ACTIVO")
    {
        ViewData["Title"] = "Permisos";
        ViewBag.Estado = estado;
        return View(await _service.ListarPermisosAsync(estado));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearPermiso(string nombre, string descripcion, string estado = "ACTIVO")
    {
        var r = await _service.CrearPermisoAsync(nombre, descripcion, estado);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Permisos));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarPermiso(int permisoId, string nombre, string descripcion, string estado)
    {
        var r = await _service.ActualizarPermisoAsync(permisoId, nombre, descripcion, estado);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Permisos), new { estado });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> InactivarPermiso(int permisoId)
    {
        var r = await _service.InactivarPermisoAsync(permisoId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Permisos));
    }

    public async Task<IActionResult> Asignaciones()
    {
        ViewData["Title"] = "Asignaciones";
        ViewBag.Usuarios = await _service.ListarUsuariosAsync("ACTIVO");
        ViewBag.Roles = await _service.ListarRolesAsync("ACTIVO");
        ViewBag.Permisos = await _service.ListarPermisosAsync("ACTIVO");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarRolUsuario(int usuarioId, int rolId)
    {
        var r = await _service.AsignarRolUsuarioAsync(usuarioId, rolId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Asignaciones));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarRolUsuario(int usuarioRolId)
    {
        var r = await _service.QuitarRolUsuarioAsync(usuarioRolId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Asignaciones));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarPermisoRol(int rolId, int permisoId)
    {
        var r = await _service.AsignarPermisoRolAsync(rolId, permisoId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Asignaciones));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarPermisoRol(int rolPermisoId)
    {
        var r = await _service.QuitarPermisoRolAsync(rolPermisoId);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Asignaciones));
    }

    public IActionResult Bitacora()
    {
        ViewData["Title"] = "Bitácora";
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrarBitacora(int usuarioId, string username, string resultado, string detalle)
    {
        var r = await _service.RegistrarBitacoraAsync(usuarioId, username, resultado, detalle);
        TempData[r.ok ? "Success" : "Error"] = r.message;
        return RedirectToAction(nameof(Bitacora));
    }
}
