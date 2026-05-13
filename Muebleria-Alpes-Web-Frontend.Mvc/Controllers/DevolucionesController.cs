using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class DevolucionesController : Controller
    {
        private readonly DevolucionApiService _service;

        public DevolucionesController(DevolucionApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var devoluciones = await _service.ListarAsync();
            return View(devoluciones);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearDevolucionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _service.CrearAsync(model);

            TempData[creado ? "Success" : "Error"] = creado
                ? "Devolución registrada correctamente."
                : "No se pudo registrar la devolución.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Aprobar(int id)
        {
            var aprobado = await _service.AprobarAsync(id);

            TempData[aprobado ? "Success" : "Error"] = aprobado
                ? "Devolución aprobada correctamente."
                : "No se pudo aprobar la devolución.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Rechazar(int id)
        {
            var rechazado = await _service.RechazarAsync(id);

            TempData[rechazado ? "Success" : "Error"] = rechazado
                ? "Devolución rechazada correctamente."
                : "No se pudo rechazar la devolución.";

            return RedirectToAction(nameof(Index));
        }
    }
}
