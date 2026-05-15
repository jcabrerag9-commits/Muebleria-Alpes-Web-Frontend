using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    // Ruta explícita para evitar colisión con Controllers.Inventario.InventarioController.
    // Este controlador legacy queda accesible en /Inventario-Legacy/... mientras se mantiene
    // el correcto en Controllers/Inventario/InventarioController.cs bajo /Inventario/...
    [Route("Inventario-Legacy/[action]")]
    public class InventarioController : Controller
    {
        private readonly InventarioApiService _service;

        public InventarioController(InventarioApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var existencias = await _service.ListarExistenciasAsync();
            var bodegas = await _service.ListarBodegasAsync();
            ViewBag.Bodegas = bodegas;
            return View(existencias);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarMovimiento(CrearMovimientoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var registrado = await _service.RegistrarMovimientoAsync(model);

            TempData[registrado ? "Success" : "Error"] = registrado
                ? "Movimiento registrado correctamente."
                : "No se pudo registrar el movimiento.";

            return RedirectToAction(nameof(Index));
        }
    }
}
