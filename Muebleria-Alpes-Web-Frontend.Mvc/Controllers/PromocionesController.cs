using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class PromocionesController : Controller
    {
        private readonly PromocionApiService _service;

        public PromocionesController(PromocionApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var promociones = await _service.ListarAsync();
            return View(promociones);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearPromocionViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Nombre) || string.IsNullOrWhiteSpace(model.Tipo))
            {
                TempData["Error"] = "Nombre y Tipo son requeridos.";
                return RedirectToAction(nameof(Index));
            }

            var (ok, mensaje) = await _service.CrearAsync(model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(long id, CrearPromocionViewModel model)
        {
            var (ok, mensaje) = await _service.ActualizarAsync(id, model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(long id)
        {
            var (ok, mensaje) = await _service.EliminarAsync(id);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
    }
}
