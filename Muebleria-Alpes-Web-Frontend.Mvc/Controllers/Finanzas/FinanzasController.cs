using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Finanzas;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Finanzas;
using System;
using System.Threading.Tasks;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.Finanzas
{
    public class FinanzasController : Controller
    {
        private readonly FinanzasApiService _finanzasApiService;

        public FinanzasController(FinanzasApiService finanzasApiService)
        {
            _finanzasApiService = finanzasApiService;
        }

        public async Task<IActionResult> Historial(DateTime? fechaInicio, DateTime? fechaFin, string tipoMovimiento, int? facturaId, int? clienteId)
        {
            var movimientos = await _finanzasApiService.ObtenerHistorialAsync(fechaInicio, fechaFin, tipoMovimiento, facturaId, clienteId);
            
            var viewModel = new HistorialFinancieroViewModel
            {
                Movimientos = movimientos,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                TipoMovimiento = tipoMovimiento,
                FacturaId = facturaId,
                ClienteId = clienteId
            };

            return View(viewModel);
        }
    }
}
