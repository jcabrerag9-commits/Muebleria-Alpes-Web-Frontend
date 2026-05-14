using Microsoft.AspNetCore.Mvc;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Reportes";
            return View();
        }
    }
}
