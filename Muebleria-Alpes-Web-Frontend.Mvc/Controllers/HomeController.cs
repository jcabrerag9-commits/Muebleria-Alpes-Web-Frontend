using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Models;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;
using System.Diagnostics;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    // Solo usuarios autenticados que NO sean clientes pueden entrar al dashboard
    [Authorize(Policy = "SoloAdmin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdminApiService _adminService;
        private readonly ClienteApiService _clienteService;
        private readonly ProductoApiService _productoService;

        public HomeController(
            ILogger<HomeController> logger,
            AdminApiService adminService,
            ClienteApiService clienteService,
            ProductoApiService productoService)
        {
            _logger        = logger;
            _adminService  = adminService;
            _clienteService = clienteService;
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            // Lanzar las 3 peticiones al backend en paralelo
            var taskOrdenes   = _adminService.ListarOrdenesAsync();
            var taskClientes  = _clienteService.ListarAsync();
            var taskProductos = _productoService.ListarAsync();

            await Task.WhenAll(taskOrdenes, taskClientes, taskProductos);

            var ordenes   = taskOrdenes.Result;
            var clientes  = taskClientes.Result;
            var productos = taskProductos.Result;

            // VentasHoy: solo órdenes entregadas hoy
            var ventasHoy = ordenes
                .Where(o => o.FechaOrden.Date == DateTime.Today
                         && string.Equals(o.Estado, "Entregado", StringComparison.OrdinalIgnoreCase))
                .Sum(o => o.Total);

            var dashboard = new DashboardViewModel
            {
                TotalOrdenes   = ordenes.Count,
                TotalClientes  = clientes.Count,
                TotalProductos = productos.Count,
                VentasHoy      = ventasHoy,
                OrdeneRecientes = ordenes
                    .OrderByDescending(o => o.FechaOrden)
                    .Take(5)
                    .Select(o => new OrdenRecienteViewModel
                    {
                        NumeroOrden = o.NumeroOrden,
                        Cliente     = o.Cliente,
                        Total       = o.Total,
                        Estado      = o.Estado,
                        Fecha       = o.FechaOrden
                    }).ToList()
            };

            ViewData["Title"] = "Dashboard";
            return View(dashboard);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
