using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services
{
    /// <summary>
    /// Servicio para api/caja.
    /// Endpoints disponibles: POST api/caja/abrir, PUT api/caja/cerrar, PUT api/caja/conciliar.
    /// No existe GET para listar cortes en el backend actual.
    /// </summary>
    public class CajaApiService
    {
        private readonly HttpClient _httpClient;

        public CajaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // El backend no tiene endpoint GET para listar cortes — retorna lista vacía
        public Task<List<CortesCajaViewModel>> ListarCortesAsync()
        {
            return Task.FromResult(new List<CortesCajaViewModel>());
        }

        // POST api/caja/abrir → { Resultado, Mensaje, Exitoso, Data: { CorteId } }
        public async Task<(bool Exitoso, string Mensaje, int CorteId)> AbrirCajaAsync(AbrirCajaViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/caja/abrir", new
                {
                    montoInicial = model.MontoInicial
                });

                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadFromJsonAsync<ApiResultViewModel<object>>();
                    return (false, err?.Mensaje ?? "Error al abrir caja", 0);
                }

                var result = await response.Content.ReadFromJsonAsync<CajaAbrirResponse>();
                return (result?.Exitoso ?? false, result?.Mensaje ?? "Caja abierta", result?.Data?.CorteId ?? 0);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }

        // PUT api/caja/cerrar → { Resultado, Mensaje, Exitoso }
        public async Task<(bool Exitoso, string Mensaje)> CerrarCajaAsync(CerrarCajaViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/caja/cerrar", new
                {
                    corteId = model.CorteId,
                    montoFinal = model.MontoFinal,
                    observacion = model.Observacion
                });

                var result = await response.Content.ReadFromJsonAsync<ApiResultViewModel<object>>();
                return (response.IsSuccessStatusCode, result?.Mensaje ?? (response.IsSuccessStatusCode ? "Caja cerrada" : "Error al cerrar caja"));
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }

    // Helpers internos de deserialización de respuesta de Caja
    internal class CajaAbrirData
    {
        public int CorteId { get; set; }
    }

    internal class CajaAbrirResponse
    {
        public string? Resultado { get; set; }
        public string? Mensaje { get; set; }
        public bool Exitoso { get; set; }
        public CajaAbrirData? Data { get; set; }
    }
}
