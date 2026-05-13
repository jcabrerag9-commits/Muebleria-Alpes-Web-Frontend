using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.RecursosHumanos
{
    public class VacacionesController : Controller
    {
        private readonly VacacionesApiService _vacacionesService;

        public VacacionesController(VacacionesApiService vacacionesService)
        {
            _vacacionesService = vacacionesService;
        }

        public async Task<IActionResult> Index(int? empleadoId = null, string? estado = null)
        {
            var vacaciones = await _vacacionesService.ListarAsync(empleadoId, estado);
            ViewBag.EmpleadoId = empleadoId;
            ViewBag.Estado = estado;
            return View(vacaciones);
        }

        [HttpPost]
        public async Task<IActionResult> Solicitar(CrearVacacionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var solicitado = await _vacacionesService.SolicitarAsync(model);

            TempData[solicitado ? "Success" : "Error"] = solicitado
                ? "Solicitud de vacaciones enviada correctamente."
                : "No se pudo enviar la solicitud de vacaciones.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Aprobar(int id)
        {
            var aprobado = await _vacacionesService.AprobarAsync(id);

            TempData[aprobado ? "Success" : "Error"] = aprobado
                ? "Vacaciones aprobadas correctamente."
                : "No se pudo aprobar las vacaciones.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Rechazar(int id, string motivo)
        {
            var rechazado = await _vacacionesService.RechazarAsync(id, motivo);

            TempData[rechazado ? "Success" : "Error"] = rechazado
                ? "Vacaciones rechazadas."
                : "No se pudo rechazar las vacaciones.";

            return RedirectToAction(nameof(Index));
        }
    }
}
