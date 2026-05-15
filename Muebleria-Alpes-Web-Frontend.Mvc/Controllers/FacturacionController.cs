using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    /// <summary>
    /// Proxy MVC → Backend para las llamadas AJAX de la vista Facturas/Index.
    /// Todas las rutas siguen el patrón que el JavaScript del lado del cliente espera:
    ///   GET  /Facturacion/listado
    ///   GET  /Facturacion/ordenes-pendientes
    ///   GET  /Facturacion/detalle/{id}
    ///   POST /Facturacion/generar
    ///   PUT  /Facturacion/anular/{id}
    /// </summary>
    [Route("Facturacion")]
    public class FacturacionController : Controller
    {
        private readonly HttpClient _backend;

        public FacturacionController(IHttpClientFactory factory)
        {
            _backend = factory.CreateClient("BackendApi");
        }

        // GET /Facturacion/listado[?estado=&clienteId=&nit=]
        [HttpGet("listado")]
        public async Task<IActionResult> Listado(
            [FromQuery] string? estado = null,
            [FromQuery] int? clienteId = null,
            [FromQuery] string? nit = null)
        {
            var qs = new List<string>();
            if (!string.IsNullOrWhiteSpace(estado))   qs.Add($"estado={Uri.EscapeDataString(estado)}");
            if (clienteId.HasValue)                    qs.Add($"clienteId={clienteId}");
            if (!string.IsNullOrWhiteSpace(nit))       qs.Add($"nit={Uri.EscapeDataString(nit)}");

            var url = "api/Facturacion/listado" + (qs.Count > 0 ? "?" + string.Join("&", qs) : "");
            var response = await _backend.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // GET /Facturacion/ordenes-pendientes
        [HttpGet("ordenes-pendientes")]
        public async Task<IActionResult> OrdenesPendientes()
        {
            var response = await _backend.GetAsync("api/Facturacion/ordenes-pendientes");
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // GET /Facturacion/detalle/{id}
        [HttpGet("detalle/{id:int}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var response = await _backend.GetAsync($"api/Facturacion/detalle/{id}");
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // POST /Facturacion/generar
        [HttpPost("generar")]
        public async Task<IActionResult> Generar()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _backend.PostAsync("api/Facturacion/generar", content);
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // PUT /Facturacion/anular/{id}
        [HttpPut("anular/{id:int}")]
        public async Task<IActionResult> Anular(int id)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _backend.PutAsync($"api/Facturacion/anular/{id}", content);
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
    }
}
