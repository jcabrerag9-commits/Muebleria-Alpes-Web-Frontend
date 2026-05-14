using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Shared;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.Catalogos
{
    public class CatalogoItemViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class CatalogoApiService
    {
        private readonly HttpClient _httpClient;

        public CatalogoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatalogoItemViewModel>> ListarTiposMuebleAsync()
        {
            try
            {
                System.Console.WriteLine("[CatalogoApiService] Fetching api/Catalogos/tipos-mueble...");
                var response = await _httpClient.GetFromJsonAsync<BackendResponse<List<CatalogoItemViewModel>>>("api/Catalogos/tipos-mueble");
                
                if (response != null && response.IsSuccess)
                {
                    System.Console.WriteLine($"[CatalogoApiService] Éxito: {response.Data?.Count ?? 0} tipos recibidos.");
                    return response.Data ?? new List<CatalogoItemViewModel>();
                }
                
                System.Console.WriteLine($"[CatalogoApiService] Backend devolvió error: {response?.Mensaje}");
                return new List<CatalogoItemViewModel>();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"[CatalogoApiService] EXCEPCIÓN: {ex.Message}");
                return new List<CatalogoItemViewModel>();
            }
        }
    }
}
