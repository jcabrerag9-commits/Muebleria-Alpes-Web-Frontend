using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Inventario;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Inventario
{
    public class InventarioController : Controller
    {
        private readonly ProductoApiService _productoService;
        private readonly InventarioApiService _inventarioService;

        public InventarioController(ProductoApiService productoService, InventarioApiService inventarioService)
        {
            _productoService = productoService;
            _inventarioService = inventarioService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ListarAsync();
            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> GetPanelExistencias(int productoId)
        {
            var producto = await _productoService.ObtenerPorIdAsync(productoId);
            if (producto == null) return NotFound("Producto no encontrado");

            var existencias = await _inventarioService.ObtenerExistenciasAsync(productoId);
            var reservas    = await _inventarioService.ObtenerReservasAsync(productoId);

            var viewModel = new PanelExistenciasViewModel
            {
                ProductoId      = productoId,
                ProductoNombre  = producto.Nombre,
                Existencias     = existencias,
                Reservas        = reservas
            };

            return PartialView("_PanelExistencias", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetKardex(int productoId)
        {
            var kardex = await _inventarioService.ObtenerKardexAsync(productoId);
            return PartialView("_KardexProducto", kardex);
        }

        /// <summary>
        /// Devuelve catálogo de bodegas como JSON para poblar selectores dinámicos en la UI.
        /// Si Oracle no tiene la tabla, el service retorna el fallback hardcoded.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBodegas()
        {
            var bodegas = await _inventarioService.ObtenerBodegasAsync();
            return Json(bodegas);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarMovimiento([FromForm] MovimientoInventarioViewModel model)
        {
            Console.WriteLine($"[MVC] RegistrarMovimiento: Prod={model.ProductoId}, Bod={model.BodegaId}, Cant={model.Cantidad}, Tipo={model.TipoMovimiento}");
            if (!ModelState.IsValid) 
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine($"[MVC ERROR] ModelState inválido: {errors}");
                return BadRequest($"Datos inválidos: {errors}");
            }

            bool success;
            string errorMsg = "Error desconocido";
            if (model.TipoMovimiento == "ENTRADA")
            {
                success = await _inventarioService.RegistrarEntradaAsync(model);
                errorMsg = _inventarioService.LastErrorMessage ?? "Error al registrar entrada";
            }
            else
            {
                success = await _inventarioService.RegistrarSalidaAsync(model);
                errorMsg = _inventarioService.LastErrorMessage ?? "Error al registrar salida";
            }

            if (!success) return BadRequest(errorMsg);
            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ReservarStock([FromForm] ReservaStockViewModel model)
        {
            Console.WriteLine($"[MVC] ReservarStock: Prod={model.ProductoId}, Bod={model.BodegaId}, Cant={model.Cantidad}");
            if (!ModelState.IsValid) 
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine($"[MVC ERROR] ModelState inválido: {errors}");
                return BadRequest($"Datos inválidos: {errors}");
            }

            var success = await _inventarioService.ReservarStockAsync(model);
            if (!success) return BadRequest(_inventarioService.LastErrorMessage ?? "Error al realizar la reserva");
            return Ok(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> LiberarReserva(int reservaId)
        {
            var result = await _inventarioService.LiberarReservaAsync(reservaId);
            
            if (!result.success)
            {
                // Devolvemos el mensaje exacto del backend (ej: "Esta reserva está ligada a una Orden...")
                return BadRequest(new { message = result.message });
            }

            return Ok(new { success = true });
        }

        public async Task<IActionResult> Bodegas()
        {
            var bodegas = await _inventarioService.ListarBodegasAsync();
            return View(bodegas);
        }

        [HttpPost]
        public async Task<IActionResult> CrearBodega([FromForm] BodegaViewModel model)
        {
            Console.WriteLine($"[MVC] CrearBodega recibido: Nombre={model.Nombre}, Tipo={model.Tipo}, PermiteReserva={model.PermiteReserva}");
            if (!ModelState.IsValid) 
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine($"[MVC ERROR] ModelState inválido en CrearBodega: {errors}");
                return BadRequest($"Datos inválidos: {errors}");
            }
            var success = await _inventarioService.CrearBodegaAsync(model);
            if (!success) 
            {
                var errorMsg = _inventarioService.LastErrorMessage ?? "Error al crear la bodega en Oracle.";
                Console.WriteLine($"[MVC] Error al crear bodega: {errorMsg}");
                return BadRequest(errorMsg);
            }
            return RedirectToAction(nameof(Bodegas));
        }

        public async Task<IActionResult> Movimientos(int? bodegaId, string? desde, string? hasta, string? tipoMovimiento)
        {
            var movs = await _inventarioService.ObtenerMovimientosGlobalesAsync(bodegaId, desde, hasta, tipoMovimiento);
            var bodegas = await _inventarioService.ListarBodegasAsync();
            
            var model = new MovimientoGlobalViewModel
            {
                Movimientos = movs,
                Bodegas = bodegas,
                BodegaId = bodegaId,
                FechaDesde = desde,
                FechaHasta = hasta,
                TipoMovimiento = tipoMovimiento
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditarBodega([FromForm] BodegaViewModel model)
        {
            Console.WriteLine($"[MVC] EditarBodega recibido: ID={model.BodegaId}, Nombre={model.Nombre}, Tipo={model.Tipo}");
            if (!ModelState.IsValid) 
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine($"[MVC ERROR] ModelState inválido en EditarBodega: {errors}");
                return BadRequest($"Datos inválidos: {errors}");
            }
            var success = await _inventarioService.ActualizarBodegaAsync(model);
            if (!success) 
            {
                var errorMsg = _inventarioService.LastErrorMessage ?? "Error al actualizar la bodega.";
                Console.WriteLine($"[MVC] Error al actualizar bodega ID: {model.BodegaId} -> {errorMsg}");
                return BadRequest(errorMsg);
            }
            return RedirectToAction(nameof(Bodegas));
        }

        [HttpPost]
        public async Task<IActionResult> InactivarBodega(int id)
        {
            var success = await _inventarioService.InactivarBodegaAsync(id);
            if (!success) 
            {
                var errorMsg = _inventarioService.LastErrorMessage ?? "No se puede desactivar la bodega porque tiene stock o reservas activas.";
                return BadRequest(new { message = errorMsg });
            }
            return Ok(new { success = true });
        }

        public async Task<IActionResult> Dashboard()
        {
            var dashboard = await _inventarioService.ObtenerDashboardAsync();
            return View(dashboard ?? new InventarioDashboardViewModel());
        }
    }
}
