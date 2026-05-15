using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteApiService _clienteService;

        public ClientesController(ClienteApiService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index(string? estado = null)
        {
            var clientes = await _clienteService.ListarAsync(estado);
            return View(clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            return PartialView("_DetalleClienteModal", cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearClienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var (ok, mensaje) = await _clienteService.CrearAsync(model);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estadoActual)
        {
            var nuevoEstado = estadoActual == "ACTIVO" ? "INACTIVO" : "ACTIVO";
            var (ok, mensaje) = await _clienteService.CambiarEstadoAsync(id, nuevoEstado);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (ok, mensaje) = await _clienteService.EliminarAsync(id);
            TempData[ok ? "Success" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
    }
}
