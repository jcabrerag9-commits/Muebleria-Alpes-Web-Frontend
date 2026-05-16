using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Productos
{
    public class CategoriasController : Controller
    {
        private readonly CatalogoApiService _service;

        public CategoriasController(CatalogoApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _service.ListarCategoriasAsync();
            return View(categorias);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearCategoriaViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Nombre))
            {
                TempData["Error"] = "El nombre de la categoría es requerido.";
                return RedirectToAction(nameof(Index));
            }

            var (ok, mensaje) = await _service.CrearCategoriaAsync(model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(int id, ActualizarCategoriaViewModel model)
        {
            var (ok, mensaje) = await _service.ActualizarCategoriaAsync(id, model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (ok, mensaje) = await _service.EliminarCategoriaAsync(id);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
    }
}
