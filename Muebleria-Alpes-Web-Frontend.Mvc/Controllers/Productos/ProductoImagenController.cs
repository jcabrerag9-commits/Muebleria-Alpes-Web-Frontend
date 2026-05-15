using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Productos
{
    public class ProductoImagenController : Controller
    {
        private readonly ProductoImagenApiService _imagenService;

        public ProductoImagenController(ProductoImagenApiService imagenService)
        {
            _imagenService = imagenService;
        }

        // ── Gallery ───────────────────────────────────────────────────────────────

        [HttpGet("ProductoImagen/Galeria")]
        public async Task<IActionResult> Galeria(int productoId)
        {
            try
            {
                var imagenes = await _imagenService.ListarPorProductoAsync(productoId);
                ViewBag.ProductoId = productoId;
                return PartialView("~/Views/Productos/_GaleriaModal.cshtml", imagenes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoImagenController] Galeria ERROR: {ex.Message}");
                ViewBag.ProductoId = productoId;
                return PartialView("~/Views/Productos/_GaleriaModal.cshtml", new List<ProductoImagenViewModel>());
            }
        }

        // ── Upload ────────────────────────────────────────────────────────────────

        [HttpPost("ProductoImagen/Upload")]
        [RequestSizeLimit(15 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 15 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] UploadImagenViewModel model)
        {
            Console.WriteLine($"[ProductoImagenController] Upload -> ProductoId:{model.ProductoId}, Tipo:{model.Tipo}, Archivo:{model.Archivo?.FileName ?? "null"}");

            if (model.Archivo == null || model.Archivo.Length == 0)
                return BadRequest(new { error = "Archivo vacío o no seleccionado." });

            var (success, mensaje) = await _imagenService.SubirImagenAsync(model);

            if (success)
            {
                Console.WriteLine("[ProductoImagenController] Upload exitoso.");
                return Ok(new { success = true });
            }

            Console.WriteLine($"[ProductoImagenController] Upload fallido: {mensaje}");
            return BadRequest(new { error = mensaje });
        }

        // ── Delete ────────────────────────────────────────────────────────────────

        [HttpDelete("ProductoImagen/Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var success = await _imagenService.EliminarAsync(id);
            if (success)
                return Ok(new { success = true });

            return BadRequest(new { error = "Error al eliminar la imagen." });
        }

        // ── Image proxy ───────────────────────────────────────────────────────────

        [HttpGet("ProductoImagen/GetPrincipal/{productoId:int}")]
        public async Task<IActionResult> GetPrincipal(int productoId)
        {
            var result = await _imagenService.ObtenerPrincipalAsync(productoId);
            if (result.Stream == null) return NotFound();
            return File(result.Stream, result.ContentType ?? "image/jpeg");
        }

        [HttpGet("ProductoImagen/GetImage/{id:int}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var result = await _imagenService.ObtenerImagenAsync(id);
            if (result.Stream == null) return NotFound();
            return File(result.Stream, result.ContentType ?? "image/jpeg");
        }
    }
}
