using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class VentasController : Controller
    {
        private readonly AdminApiService _adminService;

        public VentasController(AdminApiService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = await _adminService.ListarOrdenesAsync();
            return View(ordenes);
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarOrden(int ordenId)
        {
            var usuarioId = int.TryParse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                out var id) ? id : 0;

            var (exitoso, mensaje) = await _adminService.ActualizarEstadoOrdenAsync(
                ordenId, nuevoEstado: 2, usuarioId, comentario: "Marcada como Procesando desde admin");

            return Json(new { exitoso, mensaje });
        }
    }
}
