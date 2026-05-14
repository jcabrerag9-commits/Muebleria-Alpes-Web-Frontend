using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class EnviosController : Controller
    {
        private readonly LogisticaApiService _service;

        public EnviosController(LogisticaApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var envios = await _service.ListarEnviosAsync();
            return View(envios);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            var actualizado = await _service.CambiarEstadoEnvioAsync(id, estado);

            TempData[actualizado ? "Success" : "Error"] = actualizado
                ? "Estado del envío actualizado correctamente."
                : "No se pudo actualizar el estado del envío.";

            return RedirectToAction(nameof(Index));
        }
    }
}
