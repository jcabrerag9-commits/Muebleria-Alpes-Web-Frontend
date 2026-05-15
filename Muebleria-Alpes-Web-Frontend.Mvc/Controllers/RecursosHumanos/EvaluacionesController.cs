using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.RecursosHumanos
{
    public class EvaluacionesController : Controller
    {
        private readonly EvaluacionApiService _evaluacionService;

        public EvaluacionesController(EvaluacionApiService evaluacionService)
        {
            _evaluacionService = evaluacionService;
        }

        public async Task<IActionResult> Index(int? empleadoId = null)
        {
            var evaluaciones = await _evaluacionService.ListarAsync(empleadoId);
            ViewBag.EmpleadoId = empleadoId;
            return View(evaluaciones);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearEvaluacionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _evaluacionService.CrearAsync(model);

            TempData[creado ? "Success" : "Error"] = creado
                ? "Evaluación registrada correctamente."
                : "No se pudo registrar la evaluación.";

            return RedirectToAction(nameof(Index));
        }
    }
}
