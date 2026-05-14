using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class CajaController : Controller
    {
        private readonly CajaApiService _service;

        public CajaController(CajaApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var cortes = await _service.ListarCortesAsync();
            return View(cortes);
        }

        [HttpPost]
        public async Task<IActionResult> AbrirCaja(AbrirCajaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var (exitoso, mensaje, _) = await _service.AbrirCajaAsync(model);
            TempData[exitoso ? "Success" : "Error"] = exitoso ? "Caja abierta correctamente." : $"Error: {mensaje}";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CerrarCaja(CerrarCajaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var (exitoso, mensaje) = await _service.CerrarCajaAsync(model);
            TempData[exitoso ? "Success" : "Error"] = exitoso ? "Caja cerrada correctamente." : $"Error: {mensaje}";
            return RedirectToAction(nameof(Index));
        }
    }
}
