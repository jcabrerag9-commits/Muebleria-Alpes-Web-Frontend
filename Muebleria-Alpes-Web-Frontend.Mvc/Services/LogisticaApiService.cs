using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    /// <summary>
    /// Servicio para api/logistica.
    /// Endpoints disponibles: POST api/logistica/envio, PUT api/logistica/envio/estado.
    /// No existe GET para listar envíos en el backend actual.
    /// </summary>
    public class LogisticaApiService
    {
        private readonly HttpClient _httpClient;

        public LogisticaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // No existe endpoint GET en el backend — retorna lista vacía sin error
        public Task<List<EnvioViewModel>> ListarEnviosAsync()
        {
            return Task.FromResult(new List<EnvioViewModel>());
        }

        // POST api/logistica/envio
        public async Task<bool> CrearEnvioAsync(CrearEnvioViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/logistica/envio", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // PUT api/logistica/envio/estado
        public async Task<bool> CambiarEstadoEnvioAsync(int id, string estado)
        {
            try
            {
                var payload = new
                {
                    EnvioId = id,
                    NuevoEstado = estado,
                    NumeroGuia = (string?)null
                };
                var response = await _httpClient.PutAsJsonAsync("api/logistica/envio/estado", payload);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
