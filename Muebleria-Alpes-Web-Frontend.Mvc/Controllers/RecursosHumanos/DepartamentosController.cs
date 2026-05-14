using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.RecursosHumanos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers.RecursosHumanos
{
    public class DepartamentosController : Controller
    {
        private readonly DepartamentoApiService _departamentoService;

        public DepartamentosController(DepartamentoApiService departamentoService)
        {
            _departamentoService = departamentoService;
        }

        public async Task<IActionResult> Index()
        {
            var departamentos = await _departamentoService.ListarAsync();
            return View(departamentos);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var departamento = await _departamentoService.ObtenerPorIdAsync(id);

            if (departamento == null)
                return NotFound();

            return PartialView("_DetalleDepartamentoModal", departamento);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearDepartamentoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _departamentoService.CrearAsync(model);

            TempData[creado ? "Success" : "Error"] = creado
                ? "Departamento creado correctamente."
                : "No se pudo crear el departamento.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(int id, ActualizarDepartamentoViewModel model)
        {
            var actualizado = await _departamentoService.ActualizarAsync(id, model);

            TempData[actualizado ? "Success" : "Error"] = actualizado
                ? "Departamento actualizado correctamente."
                : "No se pudo actualizar el departamento.";

            return RedirectToAction(nameof(Index));
        }
    }
}
