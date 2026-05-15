using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class PagosController : Controller
    {
        private readonly AdminApiService _adminService;

        public PagosController(AdminApiService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            var pagos = await _adminService.ListarPagosAsync();
            return View(pagos);
        }
    }
}
