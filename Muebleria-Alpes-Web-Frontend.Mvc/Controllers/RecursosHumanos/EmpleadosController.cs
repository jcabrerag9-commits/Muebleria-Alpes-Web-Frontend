using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.RecursosHumanos
{
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoApiService _empleadoService;

        public EmpleadosController(EmpleadoApiService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        public async Task<IActionResult> Index(string? estado = null)
        {
            var empleados = await _empleadoService.ListarAsync(estado);
            return View(empleados);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var empleado = await _empleadoService.ObtenerPorIdAsync(id);

            if (empleado == null)
                return NotFound();

            return PartialView("_DetalleEmpleadoModal", empleado);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearEmpleadoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _empleadoService.CrearAsync(model);

            TempData[creado ? "Success" : "Error"] = creado
                ? "Empleado creado correctamente."
                : "No se pudo crear el empleado.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(int id, ActualizarEmpleadoViewModel model)
        {
            var actualizado = await _empleadoService.ActualizarAsync(id, model);

            TempData[actualizado ? "Success" : "Error"] = actualizado
                ? "Empleado actualizado correctamente."
                : "No se pudo actualizar el empleado.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estadoActual)
        {
            var nuevoEstado = estadoActual == "ACTIVO" ? "INACTIVO" : "ACTIVO";

            var model = new CambiarEstadoEmpleadoViewModel
            {
                Estado = nuevoEstado,
                Motivo = "Cambio de estado desde frontend MVC",
                UsuarioId = 1
            };

            var actualizado = await _empleadoService.CambiarEstadoAsync(id, model);

            TempData[actualizado ? "Success" : "Error"] = actualizado
                ? "Estado actualizado correctamente."
                : "No se pudo cambiar el estado.";

            return RedirectToAction(nameof(Index));
        }
    }
}
