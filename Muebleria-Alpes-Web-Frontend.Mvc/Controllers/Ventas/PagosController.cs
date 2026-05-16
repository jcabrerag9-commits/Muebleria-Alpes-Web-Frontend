using Microsoft.AspNetCore.Mvc;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Ventas
{
    // Ruta explícita para evitar colisión con Controllers.PagosController (mismo nombre de clase).
    [Route("Ventas/Pagos/[action]")]
    public class PagosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
