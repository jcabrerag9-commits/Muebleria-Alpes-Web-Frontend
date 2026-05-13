using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
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
