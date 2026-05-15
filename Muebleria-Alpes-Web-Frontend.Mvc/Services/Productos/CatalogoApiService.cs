using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels.Productos;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos
{
    public class CatalogoApiService
    {
        private readonly HttpClient _httpClient;

        public CatalogoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ── CATEGORIAS ───────────────────────────────────────────────────────────

        // ── CATEGORIAS ─────────────────────────────────────────────────────────────
        // El backend expone api/categorias como bare list (sin ApiResponse wrapper).

        public async Task<List<CategoriaViewModel>> ListarCategoriasAsync()
        {
            try
            {
                var lista = await _httpClient.GetFromJsonAsync<List<CategoriaViewModel>>("api/categorias");
                return lista ?? new List<CategoriaViewModel>();
            }
            catch
            {
                return new List<CategoriaViewModel>();
            }
        }

        public async Task<CategoriaViewModel?> ObtenerCategoriaPorIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CategoriaViewModel>($"api/categorias/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool Ok, string Mensaje)> CrearCategoriaAsync(CrearCategoriaViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/categorias", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Categoría creada correctamente.");

                // Leer mensaje de error del backend
                var body = await response.Content.ReadFromJsonAsync<CatalogoErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al crear la categoría.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Ok, string Mensaje)> ActualizarCategoriaAsync(int id, ActualizarCategoriaViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/categorias/{id}", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Categoría actualizada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<CatalogoErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al actualizar.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Ok, string Mensaje)> EliminarCategoriaAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/categorias/{id}");
                if (response.IsSuccessStatusCode)
                    return (true, "Categoría eliminada correctamente.");

                var body = await response.Content.ReadFromJsonAsync<CatalogoErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al eliminar.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        private sealed class CatalogoErrorResponse
        {
            [System.Text.Json.Serialization.JsonPropertyName("mensaje")]
            public string? Mensaje { get; set; }
        }

        // ── COLORES ─────────────────────────────────────────────────────────────────
        // El backend expone api/colores como bare list (sin wrapper ApiResponse).

        public async Task<List<ColorViewModel>> ListarColoresAsync()
        {
            try
            {
                var lista = await _httpClient.GetFromJsonAsync<List<ColorViewModel>>("api/colores");
                return lista ?? new List<ColorViewModel>();
            }
            catch
            {
                return new List<ColorViewModel>();
            }
        }

        public async Task<ColorViewModel?> ObtenerColorPorIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ColorViewModel>($"api/colores/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool Ok, string Mensaje)> CrearColorAsync(CrearColorViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/colores", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Color creado correctamente.");

                var body = await response.Content.ReadFromJsonAsync<CatalogoErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al crear el color.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Ok, string Mensaje)> ActualizarColorAsync(int id, ActualizarColorViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/colores/{id}", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Color actualizado correctamente.");

                var body = await response.Content.ReadFromJsonAsync<CatalogoErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al actualizar el color.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Ok, string Mensaje)> EliminarColorAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/colores/{id}");
                if (response.IsSuccessStatusCode)
                    return (true, "Color eliminado correctamente.");

                var body = await response.Content.ReadFromJsonAsync<CatalogoErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al eliminar el color.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        // ── MATERIALES ───────────────────────────────────────────────────────────
        // Backend expone api/materiales como bare list (sin ApiResponse wrapper).

        public async Task<List<MaterialViewModel>> ListarMaterialesAsync()
        {
            try
            {
                var lista = await _httpClient.GetFromJsonAsync<List<MaterialViewModel>>("api/materiales");
                return lista ?? new List<MaterialViewModel>();
            }
            catch
            {
                return new List<MaterialViewModel>();
            }
        }

        public async Task<MaterialViewModel?> ObtenerMaterialPorIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<MaterialViewModel>($"api/materiales/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool Ok, string Mensaje)> CrearMaterialAsync(CrearMaterialViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/materiales", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Material creado correctamente.");

                var body = await response.Content.ReadFromJsonAsync<CatalogoErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al crear el material.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Ok, string Mensaje)> ActualizarMaterialAsync(int id, ActualizarMaterialViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/materiales/{id}", model);
                if (response.IsSuccessStatusCode)
                    return (true, "Material actualizado correctamente.");

                var body = await response.Content.ReadFromJsonAsync<CatalogoErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al actualizar el material.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Ok, string Mensaje)> EliminarMaterialAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/materiales/{id}");
                if (response.IsSuccessStatusCode)
                    return (true, "Material eliminado correctamente.");

                var body = await response.Content.ReadFromJsonAsync<CatalogoErrorResponse>();
                return (false, body?.Mensaje ?? $"Error {(int)response.StatusCode} al eliminar el material.");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }
    }
}
