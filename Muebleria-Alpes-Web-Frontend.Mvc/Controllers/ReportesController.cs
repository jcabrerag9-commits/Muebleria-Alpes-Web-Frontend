using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers;

[Authorize(Policy = "SoloAdmin")]
public class ReportesController : Controller
{
    private readonly ReportesModuloApiService _service;
    public ReportesController(ReportesModuloApiService service) => _service = service;

    public IActionResult Index()
    {
        ViewData["Title"] = "Reportes";
        return View();
    }

    public async Task<IActionResult> Cliente()
    {
        ViewData["Title"] = "Reportes de clientes";
        ViewBag.Clientes = await _service.ClientesAsync();
        return View();
    }

    public async Task<IActionResult> Ventas()
    {
        ViewData["Title"] = "Reportes de ventas";
        ViewBag.Canales = await _service.CanalesAsync();
        ViewBag.Ciudades = await _service.CiudadesAsync();
        return View();
    }

    public async Task<IActionResult> Caja()
    {
        ViewData["Title"] = "Reportes de caja";
        ViewBag.Cortes = await _service.CortesAsync();
        return View();
    }

    public async Task<IActionResult> Marketing()
    {
        ViewData["Title"] = "Reportes de marketing";
        ViewBag.Clientes = await _service.ClientesAsync();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ClienteResultado(int clienteId, DateTime fechaInicio, DateTime fechaFin)
    {
        var total = await _service.ObtenerTextoAsync($"api/ReportesCliente/total-compras?clienteId={clienteId}&fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        var ltv = await _service.ObtenerTextoAsync($"api/ReportesCliente/ltv?clienteId={clienteId}&fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        var ticket = await _service.ObtenerTextoAsync($"api/ReportesCliente/ticket-promedio?clienteId={clienteId}&fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        return Json(new { total, ltv, ticket });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> VentasResultado(DateTime fechaInicio, DateTime fechaFin, int ciudadId, int canalVentaId)
    {
        var total = await _service.ObtenerTextoAsync($"api/ReportesVentas/total-rango?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        var ciudad = await _service.ObtenerTextoAsync($"api/ReportesVentas/total-por-ciudad?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}&ciudadId={ciudadId}");
        var canal = await _service.ObtenerTextoAsync($"api/ReportesVentas/ingresos-por-canal?canalVentaId={canalVentaId}&fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        return Json(new { total, ciudad, canal });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CajaResultado(DateTime fechaInicio, DateTime fechaFin, string estado, int corteCajaId)
    {
        var total = await _service.ObtenerTextoAsync($"api/ReportesCaja/total-ventas-corte?corteCajaId={corteCajaId}");
        var diferencia = await _service.ObtenerTextoAsync($"api/ReportesCaja/diferencia-corte?corteCajaId={corteCajaId}");
        var reporte = await _service.PostTextoAsync("api/ReportesCaja/reporte-corte-caja", new { fechaInicio, fechaFin, estado, usuarioId = int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0 });
        return Json(new { total, diferencia, reporte });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MarketingResultado(int clienteId, DateTime fechaInicio, DateTime fechaFin)
    {
        var ltv = await _service.ObtenerTextoAsync($"api/ReportesMarketing/ltv-cliente?clienteId={clienteId}&fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        var retencion = await _service.ObtenerTextoAsync($"api/ReportesMarketing/tasa-retencion?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        var conversion = await _service.ObtenerTextoAsync($"api/ReportesMarketing/tasa-conversion?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        return Json(new { ltv, retencion, conversion });
    }
}
