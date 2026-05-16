using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    /// <summary>
    /// Proxy MVC → Backend para las llamadas AJAX de la vista Pagos/Index.
    /// Todas las rutas siguen el patrón que el JavaScript del lado del cliente espera:
    ///   GET  /Pago/listado
    ///   GET  /Pago/factura/{facturaId}
    ///   GET  /Pago/ordenes-pendientes
    ///   POST /Pago/procesar
    ///   PUT  /Pago/anular/{id}
    /// </summary>
    [Route("Pago")]
    public class PagoController : Controller
    {
        private readonly HttpClient _backend;

        public PagoController(IHttpClientFactory factory)
        {
            _backend = factory.CreateClient("BackendApi");
        }

        // GET /Pago/listado
        [HttpGet("listado")]
        public async Task<IActionResult> Listado()
        {
            var response = await _backend.GetAsync("api/Pago/listado");
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // GET /Pago/factura/{facturaId}
        [HttpGet("factura/{facturaId:int}")]
        public async Task<IActionResult> PorFactura(int facturaId)
        {
            var response = await _backend.GetAsync($"api/Pago/factura/{facturaId}");
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // GET /Pago/ordenes-pendientes
        [HttpGet("ordenes-pendientes")]
        public async Task<IActionResult> OrdenesPendientes()
        {
            var response = await _backend.GetAsync("api/Pago/ordenes-pendientes");
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // POST /Pago/procesar
        [HttpPost("procesar")]
        public async Task<IActionResult> Procesar()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _backend.PostAsync("api/Pago/procesar", content);
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // PUT /Pago/anular/{id}
        [HttpPut("anular/{id:int}")]
        public async Task<IActionResult> Anular(int id)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _backend.PutAsync($"api/Pago/anular/{id}", content);
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
    }
}
