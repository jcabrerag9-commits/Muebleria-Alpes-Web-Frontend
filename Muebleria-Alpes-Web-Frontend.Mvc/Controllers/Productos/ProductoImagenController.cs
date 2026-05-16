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

        [HttpGet]
        public async Task<IActionResult> Galeria(int productoId)
        {
            var imagenes = await _imagenService.ListarPorProductoAsync(productoId);
            ViewBag.ProductoId = productoId;
            return PartialView("~/Views/Productos/_GaleriaModal.cshtml", imagenes);
        }

    [HttpPost]
public async Task<IActionResult> Upload(UploadImagenViewModel model)
{
    System.Console.WriteLine($"[ProductoImagenController] Upload requested. ProductoId: {model.ProductoId}, Tipo: {model.Tipo}, Archivo nulo?: {model.Archivo == null}");

    if (model.Archivo == null || model.Archivo.Length == 0)
    {
        System.Console.WriteLine("[ProductoImagenController] Error: Archivo vacío.");
        return BadRequest("Archivo vacío.");
    }

    try
    {
        var (ok, mensaje) = await _imagenService.SubirImagenAsync(model);

        if (ok)
        {
            System.Console.WriteLine("[ProductoImagenController] Upload exitoso.");
            return Ok(new { success = true });
        }

        System.Console.WriteLine("[ProductoImagenController] Error lógico en backend.");
        return BadRequest(mensaje);
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"[ProductoImagenController] Excepción: {ex.Message}");
        return BadRequest(ex.Message);
    }
}
[HttpDelete]
public async Task<IActionResult> Eliminar(int id)
{
    var success = await _imagenService.EliminarAsync(id);

    if (success)
        return Ok(new { success = true });

    return BadRequest("Error al eliminar la imagen.");
}

        [HttpGet("ProductoImagen/GetPrincipal/{productoId}")]
        public async Task<IActionResult> GetPrincipal(int productoId)
        {
            try 
            {
                var (stream, contentType) = await _imagenService.ObtenerPrincipalAsync(productoId);
                if (stream == null) return NotFound();

                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // REQUERIDO para que el FileResult pueda leerlo

                return File(memoryStream, contentType ?? "image/jpeg");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[MVC-IMG] ERROR en GetPrincipal({productoId}): {ex.Message}");
                return NotFound();
            }
        }

        [HttpGet("ProductoImagen/GetImage/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            try 
            {
                var (stream, contentType) = await _imagenService.ObtenerImagenAsync(id);
                if (stream == null) return NotFound();

                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // REQUERIDO

                return File(memoryStream, contentType ?? "image/jpeg");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[MVC-IMG] ERROR en GetImage({id}): {ex.Message}");
                return NotFound();
            }
        }
    }
}
