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

            // Fetch envíos for each order in parallel so the client can see shipping status
            var envioTasks = ordenes.Select(o => _ventasService.ObtenerEnvioPorOrdenAsync(o.OrdenId));
            var envios = await Task.WhenAll(envioTasks);
            var enviosPorOrden = ordenes
                .Zip(envios, (o, e) => (o.OrdenId, Envio: e))
                .Where(x => x.Envio != null)
                .ToDictionary(x => x.OrdenId, x => x.Envio!);

            ViewBag.EnviosPorOrden = enviosPorOrden;
            ViewData["Title"] = "Mis Pedidos";
            return View(ordenes);
        }

        public async Task<IActionResult> MisDirecciones()
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToAction("Login", "Login");

            var clienteId = GetClienteId();
            if (clienteId == 0)
            {
                TempData["Error"] = "No se pudo identificar tu cuenta.";
                return View(new List<DireccionClienteViewModel>());
            }

            var direcciones = await _ventasService.ObtenerDireccionesAsync(clienteId);
            ViewData["Title"] = "Mis Direcciones";
            return View(direcciones.Where(d => d.EsActiva).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDireccion(
            int paisId, int departamentoId, int ciudadId,
            string tipo, string direccionLinea1, string? direccionLinea2,
            string? codigoPostal, string? referencia, bool esPrincipal)
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToAction("Login", "Login");

            var clienteId = GetClienteId();
            if (clienteId == 0)
            {
                TempData["Error"] = "No se pudo identificar tu cuenta.";
                return RedirectToAction(nameof(MisDirecciones));
            }

            var req = new NuevaDireccionRequest
            {
                ClienteId    = clienteId,
                PaisId       = paisId,
                DepartamentoId = departamentoId,
                CiudadId     = ciudadId,
                Tipo         = tipo,
                DireccionLinea1 = direccionLinea1,
                DireccionLinea2 = direccionLinea2,
                CodigoPostal = codigoPostal,
                Referencia   = referencia,
                EsPrincipal  = esPrincipal ? "S" : "N",
                Estado       = "ACTIVO"
            };

            var (ok, mensaje) = await _ventasService.AgregarDireccionAsync(clienteId, req);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(MisDirecciones));
        }
    }
}
