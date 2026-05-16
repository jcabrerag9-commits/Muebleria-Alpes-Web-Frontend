using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Security.Claims;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class CarritoController : Controller
    {
        private readonly CarritoApiService _carritoService;
        private readonly AuthApiService    _authService;

        public CarritoController(CarritoApiService carritoService, AuthApiService authService)
        {
            _carritoService = carritoService;
            _authService    = authService;
        }

        /// <summary>
        /// Obtiene el CLI_CLIENTE.
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

        private string GetUsername() =>
            User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

        /// <summary>
        /// Actualiza la cookie de autenticación para persistir el CLI_CLIENTE correcto.
        /// Solo se llama cuando el valor del claim es distinto al actual.
        /// </summary>
        private async Task ActualizarClaimClienteId(int clienteId)
        {
            var actual = User.FindFirst("clienteId")?.Value;
            if (actual == clienteId.ToString()) return;   // Ya está correcto

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? ""),
                new(ClaimTypes.Name,           User.FindFirst(ClaimTypes.Name)?.Value           ?? ""),
                new(ClaimTypes.GivenName,      User.FindFirst(ClaimTypes.GivenName)?.Value      ?? ""),
                new(ClaimTypes.Role,           User.FindFirst(ClaimTypes.Role)?.Value            ?? ""),
                new("clienteId",               clienteId.ToString())
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        // ─── Endpoints ──────────────────────────────────────────────────────────

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToAction("Login", "Login");

            var clienteId = GetClienteId();
            var carrito   = await _carritoService.ObtenerCarritoAsync(clienteId);
            ViewData["Title"] = "Mi Carrito";
            return View(carrito);
        }

        /// <summary>
        /// GET /Carrito/Count → { count: N }
        /// Devuelve el número de productos distintos en el carrito.
        /// Usado por el badge del ícono de carrito en el navbar.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Count()
        {
            if (User.Identity?.IsAuthenticated != true)
                return Json(new { count = 0 });

            var clienteId = GetClienteId();
            if (clienteId == 0)
                return Json(new { count = 0 });

            var carrito = await _carritoService.ObtenerCarritoAsync(clienteId);
            // Suma de cantidades (no solo items distintos)
            var total = carrito.Detalle.Sum(d => d.Cantidad);
            return Json(new { count = total });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(int productoId, int cantidad = 1)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                TempData["Error"] = "Debes iniciar sesión para agregar productos al carrito.";
                return RedirectToAction("Login", "Login");
            }

            var username  = GetUsername();
            var clienteId = GetClienteId();

            // EnsureCliente: busca o crea ALP_CLIENTE para este usuario
            if (!string.IsNullOrEmpty(username))
            {
                var clienteIdReal = await _authService.EnsureClienteAsync(username);
                if (clienteIdReal > 0)
                {
                    clienteId = clienteIdReal;
                    // Persistir el clienteId correcto en la cookie de auth
                    await ActualizarClaimClienteId(clienteId);
                }
            }

            if (clienteId == 0)
            {
                TempData["Error"] = "No se pudo inicializar tu cuenta de cliente. Cierra sesión e inicia de nuevo.";
                var refUri = Request.Headers["Referer"].ToString();
                return !string.IsNullOrEmpty(refUri) ? Redirect(refUri) : RedirectToAction("Index", "Tienda");
            }

            var model = new AgregarCarritoViewModel
            {
                ClienteId  = clienteId,
                ProductoId = productoId,
                Cantidad   = cantidad > 0 ? cantidad : 1
            };

            var (ok, mensaje) = await _carritoService.AgregarProductoAsync(model);

            if (!ok)
                TempData["Error"] = mensaje;
            else
                TempData["Success"] = mensaje;

            var referer = Request.Headers["Referer"].ToString();
            return !string.IsNullOrEmpty(referer) ? Redirect(referer) : RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarCantidad(int detalleId, int cantidad)
        {
            await _carritoService.ActualizarCantidadAsync(detalleId, cantidad);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int detalleId)
        {
            await _carritoService.EliminarProductoAsync(detalleId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vaciar(int carritoId)
        {
            await _carritoService.VaciarCarritoAsync(carritoId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToAction("Login", "Login");

            var clienteId = GetClienteId();
            var carrito   = await _carritoService.ObtenerCarritoAsync(clienteId);
            ViewData["Title"] = "Checkout";
            return View(carrito);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarOrden(int carritoId)
        {
            var (ok, mensaje) = await _carritoService.ConvertirOrdenAsync(carritoId, canalVentaId: 1);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction("MisPedidos", "Cuenta");
        }
    }
}
