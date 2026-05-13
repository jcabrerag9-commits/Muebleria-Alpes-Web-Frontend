using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class FacturasController : Controller
    {
        private readonly AdminApiService _adminService;

        public FacturasController(AdminApiService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            var facturas = await _adminService.ListarFacturasAsync();
            return View(facturas);
        }
    }
}
