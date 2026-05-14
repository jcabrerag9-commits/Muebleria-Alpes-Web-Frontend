using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    /// <summary>
    /// Servicio para api/clientes — retorna lista directa (sin ApiResponse wrapper).
    /// </summary>
    public class ClienteApiService
    {
        private readonly HttpClient _httpClient;

        public ClienteApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/clientes → retorna IEnumerable<Cliente> directamente
        public async Task<List<ClienteViewModel>> ListarAsync(string? estado = null)
        {
            try
            {
                var url = "api/clientes";
                var lista = await _httpClient.GetFromJsonAsync<List<ClienteViewModel>>(url);
                return lista ?? new List<ClienteViewModel>();
            }
            catch
            {
                return new List<ClienteViewModel>();
            }
        }

        // GET api/clientes/{id} → retorna objeto directamente
        public async Task<ClienteViewModel?> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ClienteViewModel>($"api/clientes/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool Ok, string Mensaje)> CrearAsync(CrearClienteViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/clientes", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Cliente creado correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ClienteErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al crear el cliente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Ok, string Mensaje)> CambiarEstadoAsync(int id, string estado)
        {
            try
            {
                var response = await _httpClient.PatchAsync(
                    $"api/clientes/{id}/estado?estado={estado}&motivo=Cambio desde sistema&usuarioId=1",
                    null);
                if (response.IsSuccessStatusCode)
                    return (true, "Estado actualizado correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ClienteErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al cambiar el estado.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Ok, string Mensaje)> EliminarAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/clientes/{id}");
                if (response.IsSuccessStatusCode)
                    return (true, "Cliente eliminado correctamente.");

                var body = await response.Content.ReadFromJsonAsync<ClienteErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al eliminar el cliente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        private sealed class ClienteErrorResponse
        {
            [JsonPropertyName("mensaje")]
            public string? Mensaje { get; set; }

            [JsonPropertyName("message")]
            public string? Message { get; set; }
        }
    }
}
