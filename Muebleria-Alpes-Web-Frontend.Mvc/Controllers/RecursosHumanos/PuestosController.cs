using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.RecursosHumanos
{
    public class PuestosController : Controller
    {
        private readonly PuestoApiService _puestoService;

        public PuestosController(PuestoApiService puestoService)
        {
            _puestoService = puestoService;
        }

        public async Task<IActionResult> Index()
        {
            var puestos = await _puestoService.ListarAsync();
            return View(puestos);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearPuestoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _puestoService.CrearAsync(model);

            TempData[creado ? "Success" : "Error"] = creado
                ? "Puesto creado correctamente."
                : "No se pudo crear el puesto.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(int id, ActualizarPuestoViewModel model)
        {
            var actualizado = await _puestoService.ActualizarAsync(id, model);

            TempData[actualizado ? "Success" : "Error"] = actualizado
                ? "Puesto actualizado correctamente."
                : "No se pudo actualizar el puesto.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estadoActual)
        {
            var nuevoEstado = estadoActual == "ACTIVO" ? "INACTIVO" : "ACTIVO";
            var actualizado = await _puestoService.CambiarEstadoAsync(id, nuevoEstado);

            TempData[actualizado ? "Success" : "Error"] = actualizado
                ? "Estado actualizado correctamente."
                : "No se pudo cambiar el estado.";

            return RedirectToAction(nameof(Index));
        }
    }
}
