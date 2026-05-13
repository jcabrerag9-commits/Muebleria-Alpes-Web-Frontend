using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    /// <summary>
    /// Servicio de inventario.
    /// - api/producto (GET) → InventarioResponse<IEnumerable<ProductoDTO>> = { Resultado, Mensaje, Data }
    /// - api/inventario/existencia/{productoId} → InventarioResponse<IEnumerable<ExistenciaDTO>>
    /// - api/inventario/entrada|salida (POST) → InventarioResponse
    /// </summary>
    public class InventarioApiService
    {
        private readonly HttpClient _httpClient;

        public InventarioApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Usa api/producto (sin 's') que lista todos los productos con su info de inventario
        public async Task<List<ExistenciaViewModel>> ListarExistenciasAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<InventarioApiResponse<List<ExistenciaViewModel>>>("api/producto");
                return result?.Data ?? new List<ExistenciaViewModel>();
            }
            catch
            {
                return new List<ExistenciaViewModel>();
            }
        }

        // api/inventario/existencia/{productoId} — existencias de un producto específico
        public async Task<List<ExistenciaViewModel>> ObtenerExistenciaPorProductoAsync(int productoId)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<InventarioApiResponse<List<ExistenciaViewModel>>>($"api/inventario/existencia/{productoId}");
                return result?.Data ?? new List<ExistenciaViewModel>();
            }
            catch
            {
                return new List<ExistenciaViewModel>();
            }
        }

        // Bodegas: no hay endpoint en el backend actual — retorna lista vacía
        public Task<List<BodegaViewModel>> ListarBodegasAsync()
        {
            return Task.FromResult(new List<BodegaViewModel>());
        }

        // Registrar entrada: api/inventario/entrada
        public async Task<bool> RegistrarEntradaAsync(CrearMovimientoViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/inventario/entrada", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Registrar salida: api/inventario/salida
        public async Task<bool> RegistrarSalidaAsync(CrearMovimientoViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/inventario/salida", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Mantiene compatibilidad con el método anterior
        public async Task<bool> RegistrarMovimientoAsync(CrearMovimientoViewModel model)
        {
            // Decide según TipoMovimiento
            if (model.TipoMovimiento?.ToUpper() == "SALIDA")
                return await RegistrarSalidaAsync(model);
            return await RegistrarEntradaAsync(model);
        }
    }
}
