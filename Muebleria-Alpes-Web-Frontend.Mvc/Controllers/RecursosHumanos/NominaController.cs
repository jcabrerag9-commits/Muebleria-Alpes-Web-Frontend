using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.RecursosHumanos
{
    public class NominaController : Controller
    {
        private readonly NominaApiService _nominaService;

        public NominaController(NominaApiService nominaService)
        {
            _nominaService = nominaService;
        }

        public async Task<IActionResult> Index()
        {
            var nominas = await _nominaService.ListarAsync();
            return View(nominas);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearNominaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _nominaService.CrearAsync(model);

            TempData[creado ? "Success" : "Error"] = creado
                ? "Nómina creada correctamente."
                : "No se pudo crear la nómina.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            var actualizado = await _nominaService.CambiarEstadoAsync(id, estado);

            TempData[actualizado ? "Success" : "Error"] = actualizado
                ? "Estado de nómina actualizado correctamente."
                : "No se pudo cambiar el estado de la nómina.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var detalle = await _nominaService.ListarDetalleAsync(id);
            ViewBag.NominaId = id;
            return View(detalle);
        }
    }
}
