using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Productos
{
    public class ProductosController : Controller
    {
        private readonly ProductoApiService _service;

        public ProductosController(ProductoApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string? estado = null)
        {
            var productos = await _service.ListarAsync(estado);
            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _service.ObtenerPorIdAsync(id);

            if (producto == null)
                return NotFound();

            return PartialView("_DetalleProductoModal", producto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _service.CrearAsync(model);

            TempData[creado ? "Success" : "Error"] = creado
                ? "Producto creado correctamente."
                : "No se pudo crear el producto.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(int id, ActualizarProductoViewModel model)
        {
            var actualizado = await _service.ActualizarAsync(id, model);

            TempData[actualizado ? "Success" : "Error"] = actualizado
                ? "Producto actualizado correctamente."
                : "No se pudo actualizar el producto.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estadoActual)
        {
            var nuevoEstado = estadoActual == "ACTIVO" ? "INACTIVO" : "ACTIVO";
            var actualizado = await _service.CambiarEstadoAsync(id, nuevoEstado);

            TempData[actualizado ? "Success" : "Error"] = actualizado
                ? "Estado actualizado correctamente."
                : "No se pudo cambiar el estado.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _service.EliminarAsync(id);

            TempData[eliminado ? "Success" : "Error"] = eliminado
                ? "Producto eliminado correctamente."
                : "No se pudo eliminar el producto.";

            return RedirectToAction(nameof(Index));
        }
    }
}
