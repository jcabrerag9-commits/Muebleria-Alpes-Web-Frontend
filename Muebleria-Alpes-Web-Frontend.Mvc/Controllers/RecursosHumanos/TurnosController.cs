using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.RecursosHumanos
{
    public class TurnosController : Controller
    {
        private readonly TurnoApiService _turnoService;

        public TurnosController(TurnoApiService turnoService)
        {
            _turnoService = turnoService;
        }

        public async Task<IActionResult> Index()
        {
            var turnos = await _turnoService.ListarAsync();
            return View(turnos);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearTurnoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _turnoService.CrearAsync(model);

            TempData[creado ? "Success" : "Error"] = creado
                ? "Turno creado correctamente."
                : "No se pudo crear el turno.";

            return RedirectToAction(nameof(Index));
        }
    }
}
