using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Security.Claims;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class CuentaController : Controller
    {
        private readonly VentasApiService _ventasService;

        public CuentaController(VentasApiService ventasService)
        {
            _ventasService = ventasService;
        }

        /// <summary>
        /// Obtiene el CLI_CLIENTE del usuario autenticado.
        /// Prioridad: claim "clienteId" → claim NameIdentifier → 0
        /// </summary>
        private int GetClienteId()
        {
            var cliClaim = User.FindFirst("clienteId")?.Value;
            if (int.TryParse(cliClaim, out var cid) && cid > 0) return cid;

            var usuClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(usuClaim, out var uid) && uid > 0) return uid;

            return 0;
        }

        public async Task<IActionResult> MisPedidos()
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToAction("Login", "Login");

            var clienteId = GetClienteId();
            if (clienteId == 0)
            {
                TempData["Error"] = "No se pudo identificar tu cuenta. Por favor cierra sesión e inicia de nuevo.";
                return View(new List<OrdenClienteViewModel>());
            }

            var ordenes = await _ventasService.ObtenerOrdenesClienteAsync(clienteId);
            ViewData["Title"] = "Mis Pedidos";
            return View(ordenes);
        }
    }
}
