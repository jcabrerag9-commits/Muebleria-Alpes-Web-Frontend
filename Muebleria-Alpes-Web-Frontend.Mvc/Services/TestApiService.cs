using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Test;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    public class TestApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TestApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TestConexionViewModel> ProbarConexionAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BackendApi");

                var response = await client.GetAsync("api/test/db");

                if (!response.IsSuccessStatusCode)
                {
                    return new TestConexionViewModel
                    {
                        Exitoso = false,
                        Mensaje = $"Error HTTP: {(int)response.StatusCode} - {response.ReasonPhrase}",
                        Resultado = null
                    };
                }

                var data = await response.Content.ReadFromJsonAsync<TestConexionResponseDto>();

                return new TestConexionViewModel
                {
                    Exitoso = true,
                    Mensaje = data?.Mensaje ?? "Respuesta recibida correctamente",
                    Resultado = data?.Resultado
                };
            }
            catch (Exception ex)
            {
                return new TestConexionViewModel
                {
                    Exitoso = false,
                    Mensaje = $"Error al consumir la API: {ex.Message}",
                    Resultado = null
                };
            }
        }

        private class TestConexionResponseDto
        {
            public string? Mensaje { get; set; }
            public string? Resultado { get; set; }
        }
    }
}
