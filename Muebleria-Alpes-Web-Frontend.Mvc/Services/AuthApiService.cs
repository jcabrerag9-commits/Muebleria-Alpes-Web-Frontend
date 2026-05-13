using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services;

public record AuthResult(bool Exitoso, string Mensaje, int Id, string Username, string NombreCompleto, string Rol, int ClienteId = 0);

file class BaseLoginResponse
{
    [JsonPropertyName("resultado")] public string Resultado { get; set; } = "";
    [JsonPropertyName("mensaje")] public string Mensaje { get; set; } = "";
    [JsonPropertyName("data")] public LoginDataDto? Data { get; set; }
}

file class LoginDataDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("username")] public string Username { get; set; } = "";
    [JsonPropertyName("nombreCompleto")] public string NombreCompleto { get; set; } = "";
    [JsonPropertyName("rol")] public string Rol { get; set; } = "";
    [JsonPropertyName("clienteId")] public int ClienteId { get; set; }
}

file class BaseRegistroResponse
{
    [JsonPropertyName("resultado")] public string Resultado { get; set; } = "";
    [JsonPropertyName("mensaje")] public string Mensaje { get; set; } = "";
    [JsonPropertyName("data")] public RegistroDataDto? Data { get; set; }
}

file class RegistroDataDto
{
    [JsonPropertyName("id")]        public int    Id        { get; set; }
    [JsonPropertyName("username")]  public string Username  { get; set; } = "";
    [JsonPropertyName("rol")]       public string Rol       { get; set; } = "";
    [JsonPropertyName("clienteId")] public int    ClienteId { get; set; }
}

public class AuthApiService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public AuthApiService(HttpClient http) => _http = http;

    /// <summary>
    /// Busca o crea un ALP_CLIENTE para el usuario web (POST api/auth/ensure-cliente).
    /// Retorna el CLI_CLIENTE ID, o 0 si falló.
    /// </summary>
    public async Task<int> EnsureClienteAsync(string username)
    {
        try
        {
            var body    = JsonSerializer.Serialize(new { username });
            var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            var resp    = await _http.PostAsync("api/auth/ensure-cliente", content);
            if (!resp.IsSuccessStatusCode) return 0;

            var json = await resp.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("clienteId", out var prop))
                return prop.GetInt32();
        }
        catch { }
        return 0;
    }

    public async Task<AuthResult> LoginAsync(string username, string password)
    {
        try
        {
            var body = JsonSerializer.Serialize(new { username, password });
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("api/auth/login", content);
            var json     = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json) || (!json.TrimStart().StartsWith('{') && !json.TrimStart().StartsWith('[')))
                return new AuthResult(false, $"Error {(int)response.StatusCode} — verifica que el backend esté corriendo", 0, "", "", "");

            var result = JsonSerializer.Deserialize<BaseLoginResponse>(json, _json);

            if (result?.Resultado == "EXITO" && result.Data is not null)
            {
                return new AuthResult(true, result.Mensaje,
                    result.Data.Id, result.Data.Username,
                    result.Data.NombreCompleto, result.Data.Rol,
                    result.Data.ClienteId);
            }

            return new AuthResult(false, result?.Mensaje ?? "Credenciales inválidas", 0, "", "", "");
        }
        catch (Exception ex)
        {
            return new AuthResult(false, $"Error de conexión: {ex.Message}", 0, "", "", "");
        }
    }

    public async Task<AuthResult> RegistrarAsync(string username, string password)
    {
        try
        {
            var body    = JsonSerializer.Serialize(new { username, password });
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("api/auth/registro", content);
            var json     = await response.Content.ReadAsStringAsync();

            // Si la respuesta no es JSON (ej: HTML de error 404/500 por backend no reiniciado)
            var isJson = response.Content.Headers.ContentType?.MediaType
                             ?.Contains("json", StringComparison.OrdinalIgnoreCase) == true
                         || (json.TrimStart().StartsWith('{') || json.TrimStart().StartsWith('['));

            if (!isJson || string.IsNullOrWhiteSpace(json))
            {
                var statusMsg = response.IsSuccessStatusCode
                    ? "Respuesta inesperada del servidor"
                    : $"Error {(int)response.StatusCode} — reinicia el backend y vuelve a intentarlo";
                return new AuthResult(false, statusMsg, 0, "", "", "");
            }

            var result = JsonSerializer.Deserialize<BaseRegistroResponse>(json, _json);

            if (result?.Resultado == "EXITO" && result.Data is not null)
            {
                return new AuthResult(true, result.Mensaje,
                    result.Data.Id, result.Data.Username,
                    result.Data.Username, result.Data.Rol,
                    result.Data.ClienteId);
            }

            return new AuthResult(false, result?.Mensaje ?? "No se pudo completar el registro", 0, "", "", "");
        }
        catch (Exception ex)
        {
            return new AuthResult(false, $"Error: {ex.Message}", 0, "", "", "");
        }
    }
}
