using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.RecursosHumanos
{
    public class AsistenciasController : Controller
    {
        private readonly AsistenciaApiService _asistenciaService;

        public AsistenciasController(AsistenciaApiService asistenciaService)
        {
            _asistenciaService = asistenciaService;
        }

        public async Task<IActionResult> Index(int? empleadoId = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var asistencias = await _asistenciaService.ListarAsync(empleadoId, fechaInicio, fechaFin);
            ViewBag.EmpleadoId = empleadoId;
            ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");
            return View(asistencias);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(CrearAsistenciaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var registrado = await _asistenciaService.RegistrarAsync(model);

            TempData[registrado ? "Success" : "Error"] = registrado
                ? "Asistencia registrada correctamente."
                : "No se pudo registrar la asistencia.";

            return RedirectToAction(nameof(Index));
        }
    }
}
