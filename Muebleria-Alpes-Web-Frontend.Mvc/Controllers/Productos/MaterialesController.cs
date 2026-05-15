using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Productos
{
    public class MaterialesController : Controller
    {
        private readonly CatalogoApiService _service;

        public MaterialesController(CatalogoApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var materiales = await _service.ListarMaterialesAsync();
            return View(materiales);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearMaterialViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Nombre))
            {
                TempData["Error"] = "El nombre del material es requerido.";
                return RedirectToAction(nameof(Index));
            }

            var (ok, mensaje) = await _service.CrearMaterialAsync(model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(int id, ActualizarMaterialViewModel model)
        {
            var (ok, mensaje) = await _service.ActualizarMaterialAsync(id, model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (ok, mensaje) = await _service.EliminarMaterialAsync(id);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
    }
}
