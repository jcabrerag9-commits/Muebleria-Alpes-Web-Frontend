using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Devoluciones;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class DevolucionesController : Controller
    {
        private readonly DevolucionService _service;

        public DevolucionesController(DevolucionService service)
        {
            _service = service;
        }

        // GET /Devoluciones
        public async Task<IActionResult> Index(string? estado)
        {
            var devoluciones = await _service.GetAllAsync(estado);
            var categorias   = await _service.GetCategoriasAsync("ACTIVO");

            var model = new DevolucionIndexViewModel
            {
                Devoluciones     = devoluciones,
                Categorias       = categorias,
                TotalSolicitadas = devoluciones.Count(d => d.DevEstado == "SOLICITADA"),
                TotalAprobadas   = devoluciones.Count(d => d.DevEstado == "APROBADA"),
                TotalRechazadas  = devoluciones.Count(d => d.DevEstado == "RECHAZADA"),
                TotalCompletadas = devoluciones.Count(d => d.DevEstado == "COMPLETADA"),
                FiltroEstado     = estado
            };

            return View(model);
        }

        // GET /Devoluciones/Detalle/5
        public async Task<IActionResult> Detalle(long id)
        {
            var dev = await _service.GetByIdAsync(id);
            if (dev is null) return NotFound();
            return View(dev);
        }

        // GET /Devoluciones/Crear
        public async Task<IActionResult> Crear()
        {
            var categorias = await _service.GetCategoriasAsync("ACTIVO");
            ViewBag.Categorias = categorias;
            return View(new DevolucionCreateViewModel());
        }

        // POST /Devoluciones/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DevolucionCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await _service.GetCategoriasAsync("ACTIVO");
                return View(model);
            }

            var (ok, mensaje) = await _service.CreateAsync(model);
            if (ok)
            {
                TempData["Success"] = mensaje;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"]  = mensaje;
            ViewBag.Categorias = await _service.GetCategoriasAsync("ACTIVO");
            return View(model);
        }

        // POST /Devoluciones/CambiarEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(long id, string nuevoEstado)
        {
            var (ok, mensaje) = await _service.CambiarEstadoAsync(id, nuevoEstado);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Detalle), new { id });
        }

        // POST /Devoluciones/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(long id)
        {
            var (ok, mensaje) = await _service.DeleteAsync(id);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        // ── Categorías ────────────────────────────────────────────────────────

        // GET /Devoluciones/Categorias
        public async Task<IActionResult> Categorias()
        {
            var categorias = await _service.GetCategoriasAsync();
            return View(new CategoriaIndexViewModel { Categorias = categorias });
        }

        // POST /Devoluciones/CrearCategoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCategoria(CategoriaDevolucionViewModel model)
        {
            var (ok, mensaje) = await _service.CreateCategoriaAsync(model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Categorias));
        }

        // POST /Devoluciones/EditarCategoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCategoria(long id, CategoriaDevolucionViewModel model)
        {
            var (ok, mensaje) = await _service.UpdateCategoriaAsync(id, model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Categorias));
        }

        // POST /Devoluciones/EliminarCategoria/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarCategoria(long id)
        {
            var (ok, mensaje) = await _service.DeleteCategoriaAsync(id);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Categorias));
        }
    }
}
