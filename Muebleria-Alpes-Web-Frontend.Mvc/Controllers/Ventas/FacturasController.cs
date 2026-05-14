using Microsoft.AspNetCore.Mvc;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Ventas
{
    public class FacturasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
