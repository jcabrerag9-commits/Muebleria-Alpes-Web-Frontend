using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Promociones;
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

        // GET /Promociones
        public async Task<IActionResult> Index(string? estado, string? tipo)
        {
            var promociones = await _service.GetAllAsync(estado, tipo);
            var vigentes    = await _service.GetVigentesAsync();

            var model = new PromocionIndexViewModel
            {
                Promociones    = promociones,
                TotalActivas   = promociones.Count(p => p.PrmEstado == "ACTIVO"),
                TotalVigentes  = vigentes.Count,
                TotalInactivas = promociones.Count(p => p.PrmEstado == "INACTIVO"),
                FiltroEstado   = estado,
                FiltroTipo     = tipo
            };

            return View(model);
        }

        // GET /Promociones/Detalle/5
        public async Task<IActionResult> Detalle(long id)
        {
            var promo = await _service.GetByIdAsync(id);
            if (promo is null) return NotFound();
            return View(promo);
        }

        // GET /Promociones/Crear
        public IActionResult Crear() => View(new PromocionCreateViewModel());

        // POST /Promociones/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(PromocionCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var (ok, mensaje) = await _service.CreateAsync(model);
            if (ok)
            {
                TempData["Success"] = mensaje;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = mensaje;
            return View(model);
        }

        // GET /Promociones/Editar/5
        public async Task<IActionResult> Editar(long id)
        {
            var promo = await _service.GetByIdAsync(id);
            if (promo is null) return NotFound();

            var model = new PromocionUpdateViewModel
            {
                PrmNombre      = promo.PrmNombre,
                PrmDescripcion = promo.PrmDescripcion,
                PrmValor       = promo.PrmValor,
                PrmFechaInicio = promo.PrmFechaInicio,
                PrmFechaFin    = promo.PrmFechaFin,
                PrmEstado      = promo.PrmEstado
            };

            ViewBag.PromocionId = id;
            ViewBag.Codigo      = promo.PrmCodigo;
            ViewBag.Tipo        = promo.PrmTipo;
            return View(model);
        }

        // POST /Promociones/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(long id, PromocionUpdateViewModel model)
        {
            var (ok, mensaje) = await _service.UpdateAsync(id, model);
            if (ok)
            {
                TempData["Success"] = mensaje;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"]   = mensaje;
            ViewBag.PromocionId = id;
            return View(model);
        }

        // POST /Promociones/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(long id)
        {
            var (ok, mensaje) = await _service.DeleteAsync(id);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        // ── Banners ───────────────────────────────────────────────────────────

        // GET /Promociones/Banners
        public async Task<IActionResult> Banners(string? estado)
        {
            var banners = await _service.GetBannersAsync(estado);
            ViewBag.FiltroEstado = estado;
            return View(banners);
        }

        // GET /Promociones/CrearBanner
        public IActionResult CrearBanner() => View(new BannerCreateViewModel());

        // POST /Promociones/CrearBanner
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearBanner(BannerCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var (ok, mensaje) = await _service.CreateBannerAsync(model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return ok ? RedirectToAction(nameof(Banners)) : View(model);
        }

        // POST /Promociones/EliminarBanner/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarBanner(long id)
        {
            var (ok, mensaje) = await _service.DeleteBannerAsync(id);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Banners));
        }
    }
}
