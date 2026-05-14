using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Productos
{
    public class ProductosController : Controller
    {
        private readonly ProductoApiService _productoService;
        private readonly Muebleria_Alpes_Web_Frontend.Mvc.Services.Catalogos.CatalogoApiService _catalogoService;

        public ProductosController(ProductoApiService productoService, Muebleria_Alpes_Web_Frontend.Mvc.Services.Catalogos.CatalogoApiService catalogoService)
        {
            _productoService = productoService;
            _catalogoService = catalogoService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ListarAsync();
            return View("~/Views/Productos/Index.cshtml", productos);
        }

        [HttpGet]
        public async Task<IActionResult> GetTiposMueble()
        {
            var tipos = await _catalogoService.ListarTiposMuebleAsync();
            return Json(tipos);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);

            if (producto == null)
                return NotFound();

            return PartialView("~/Views/Productos/_DetalleProductoModal.cshtml", producto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _productoService.CrearAsync(model);

            TempData[creado ? "Success" : "Error"] = creado
                ? "Producto creado correctamente."
                : "No se pudo crear el producto. Revisa los datos o la conexión con el backend.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(int id, ActualizarProductoViewModel model)
        {
            System.Console.WriteLine($"[MVC MVC] RECIBIDO PUT para ProductoId: {id}");
            System.Console.WriteLine($"[MVC MVC] Modelo recibido -> Nombre: {model?.Nombre}, Peso: {model?.Peso}");

            if (!ModelState.IsValid)
            {
                System.Console.WriteLine($"[MVC MVC] ModelState Invalido");
                return BadRequest("Verifica los datos ingresados.");
            }

            var actualizado = await _productoService.ActualizarAsync(id, model);

            if (!actualizado)
            {
                System.Console.WriteLine($"[MVC MVC] Falla en ApiService al actualizar");
                return BadRequest("Error al actualizar el producto en el backend.");
            }

            System.Console.WriteLine($"[MVC MVC] Éxito al actualizar ProductoId: {id}");
            return Ok(new { success = true, message = "Producto actualizado correctamente." });
        }
    }
}
