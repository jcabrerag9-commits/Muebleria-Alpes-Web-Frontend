using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class BodegasController : Controller
    {
        private readonly InventarioApiService _service;

        public BodegasController(InventarioApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var bodegas = await _service.ListarBodegasAsync();
            return View(bodegas);
        }
    }
}
