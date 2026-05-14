using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Finanzas;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.Finanzas
{
    public class FinanzasApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public FinanzasApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Aseguramos que termine con /api/
            var configUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7015/";
            _baseUrl = configUrl.EndsWith("/") ? $"{configUrl}api/" : $"{configUrl}/api/";
        }

        public async Task<List<MovimientoFinancieroViewModel>> ObtenerHistorialAsync(
            DateTime? fechaInicio = null, 
            DateTime? fechaFin = null, 
            string tipoMovimiento = null, 
            int? facturaId = null, 
            int? clienteId = null)
        {
            try 
            {
                var query = $"?fechaInicio={fechaInicio?.ToString("yyyy-MM-dd")}" +
                            $"&fechaFin={fechaFin?.ToString("yyyy-MM-dd")}" +
                            $"&tipoMovimiento={tipoMovimiento}" +
                            $"&facturaId={facturaId}" +
                            $"&clienteId={clienteId}";

                var response = await _httpClient.GetAsync($"{_baseUrl}Finanzas/historial{query}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    
                    // Deserializamos el wrapper ApiResponse
                    var apiResponse = JsonSerializer.Deserialize<ApiWrapper<List<MovimientoFinancieroViewModel>>>(content, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return apiResponse?.Data ?? new List<MovimientoFinancieroViewModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FinanzasApiService Error] {ex.Message}");
            }

            return new List<MovimientoFinancieroViewModel>();
        }

        // Clase interna temporal para deserialización o mover a un archivo común
        private class ApiWrapper<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }
    }
}
