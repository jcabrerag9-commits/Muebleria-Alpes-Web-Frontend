using Microsoft.AspNetCore.Mvc;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Ventas
{
    // Ruta explícita para evitar colisión con Controllers.FacturasController (mismo nombre de clase).
    [Route("Ventas/Facturas/[action]")]
    public class FacturasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
