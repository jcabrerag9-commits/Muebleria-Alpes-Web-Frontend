using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class VentasController : Controller
    {
        private readonly AdminApiService _adminService;

        public VentasController(AdminApiService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = await _adminService.ListarOrdenesAsync();
            return View(ordenes);
        }
    }
}
