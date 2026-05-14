using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class CarritoApiService
    {
        private readonly HttpClient _httpClient;

        public CarritoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// GET api/carrito/cliente/{clienteId}
        /// Respuesta: InventarioApiResponse&lt;ObtenerCarritoClienteDataDto&gt;
        /// DataDto tiene { cabecera: {...}, detalles: [...] } → se mapea a CarritoViewModel.
        /// </summary>
        public async Task<CarritoViewModel> ObtenerCarritoAsync(int clienteId)
        {
            try
            {
                var r = await _httpClient.GetFromJsonAsync<InventarioApiResponse<CarritoViewModel>>(
                    $"api/carrito/cliente/{clienteId}");
                return r?.Data ?? new CarritoViewModel();
            }
            catch
            {
                return new CarritoViewModel();
            }
        }

        /// <summary>
        /// POST api/carrito/agregar
        /// Devuelve (bool exitoso, string mensaje) para mostrar el error del SP al usuario.
        /// El SP ya valida stock en ALP_EXISTENCIA — si falta stock, devuelve mensaje descriptivo.
        /// </summary>
        public async Task<(bool Ok, string Mensaje)> AgregarProductoAsync(AgregarCarritoViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/carrito/agregar", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Producto agregado al carrito correctamente.");

                // Leer el mensaje de error del SP (stock insuficiente, precio no configurado, etc.)
                var body = await response.Content.ReadFromJsonAsync<CarritoSpResponse>();
                return (false, body?.Mensaje ?? "No se pudo agregar el producto al carrito.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<bool> ActualizarCantidadAsync(int detalleId, int cantidad)
        {
            try
            {
                var payload = new { DetalleId = detalleId, NuevaCantidad = cantidad };
                var response = await _httpClient.PutAsJsonAsync("api/carrito/actualizar-cantidad", payload);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarProductoAsync(int detalleId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/carrito/eliminar/{detalleId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> VaciarCarritoAsync(int carritoId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/carrito/vaciar/{carritoId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TotalCarritoViewModel?> ObtenerTotalAsync(int carritoId)
        {
            try
            {
                var r = await _httpClient.GetFromJsonAsync<InventarioApiResponse<TotalCarritoViewModel>>(
                    $"api/carrito/total/{carritoId}");
                return r?.Data;
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool Ok, string Mensaje)> ConvertirOrdenAsync(int carritoId, int canalVentaId = 1)
        {
            try
            {
                var payload = new { CarritoId = carritoId, CanalVenta = canalVentaId };
                var response = await _httpClient.PostAsJsonAsync("api/carrito/convertir-orden", payload);
                if (response.IsSuccessStatusCode)
                    return (true, "¡Orden realizada con éxito!");

                var body = await response.Content.ReadFromJsonAsync<CarritoSpResponse>();
                return (false, body?.Mensaje ?? "Error al procesar la orden. Intenta de nuevo.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        // DTO para leer el mensaje de error de la respuesta del SP
        private sealed class CarritoSpResponse
        {
            [JsonPropertyName("resultado")]
            public string? Resultado { get; set; }

            [JsonPropertyName("mensaje")]
            public string? Mensaje { get; set; }
        }
    }
}
