using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Productos
{
    public class ColoresController : Controller
    {
        private readonly CatalogoApiService _service;

        public ColoresController(CatalogoApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var colores = await _service.ListarColoresAsync();
            return View(colores);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearColorViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Nombre))
            {
                TempData["Error"] = "El nombre del color es requerido.";
                return RedirectToAction(nameof(Index));
            }

            var (ok, mensaje) = await _service.CrearColorAsync(model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(int id, ActualizarColorViewModel model)
        {
            var (ok, mensaje) = await _service.ActualizarColorAsync(id, model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (ok, mensaje) = await _service.EliminarColorAsync(id);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
    }
}
