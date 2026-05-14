using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Services;

public abstract class AlpesApiServiceBase
{
    protected readonly HttpClient Http;
    protected readonly JsonSerializerOptions Json = new() { PropertyNameCaseInsensitive = true };

    protected AlpesApiServiceBase(HttpClient http) => Http = http;

    protected async Task<List<T>> GetListAsync<T>(string url)
    {
        try
        {
            var response = await Http.GetAsync(url);
            var text = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(text)) return new List<T>();

            var wrapped = JsonSerializer.Deserialize<AlpesApiResponse<List<T>>>(text, Json);
            if (wrapped?.Data is not null) return wrapped.Data;

            var direct = JsonSerializer.Deserialize<List<T>>(text, Json);
            return direct ?? new List<T>();
        }
        catch { return new List<T>(); }
    }

    protected async Task<T?> GetDataAsync<T>(string url)
    {
        try
        {
            var response = await Http.GetAsync(url);
            var text = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(text)) return default;

            var wrapped = JsonSerializer.Deserialize<AlpesApiResponse<T>>(text, Json);
            if (wrapped is not null && wrapped.Data is not null) return wrapped.Data;

            return JsonSerializer.Deserialize<T>(text, Json);
        }
        catch { return default; }
    }

    protected async Task<(bool ok, string message)> SendAsync(HttpMethod method, string url, object? payload)
    {
        try
        {
            using var request = new HttpRequestMessage(method, url);
            if (payload is not null)
                request.Content = JsonContent.Create(payload);

            var response = await Http.SendAsync(request);
            var text = await response.Content.ReadAsStringAsync();
            var msg = response.IsSuccessStatusCode ? "Operación realizada correctamente." : $"Error HTTP {(int)response.StatusCode}";

            if (!string.IsNullOrWhiteSpace(text) && text.TrimStart().StartsWith("{"))
            {
                try
                {
                    var op = JsonSerializer.Deserialize<OperacionSimpleResponse>(text, Json);
                    msg = op?.Message ?? op?.Mensaje ?? msg;
                    return (response.IsSuccessStatusCode && (op?.Success ?? true), msg);
                }
                catch
                {
                    return (response.IsSuccessStatusCode, msg);
                }
            }

            return (response.IsSuccessStatusCode, msg);
        }
        catch (Exception ex) { return (false, $"Error de conexión: {ex.Message}"); }
    }

    protected async Task<(bool ok, string message)> SendWithFallbackAsync(HttpMethod method, (string url, object? payload) primary, params (string url, object? payload)[] fallbacks)
    {
        var result = await SendAsync(method, primary.url, primary.payload);
        if (result.ok) return result;

        var last = result;
        foreach (var fallback in fallbacks)
        {
            last = await SendAsync(method, fallback.url, fallback.payload);
            if (last.ok) return last;
        }
        return last;
    }

    protected Task<(bool ok, string message)> PostAsync(string url, object? payload) => SendAsync(HttpMethod.Post, url, payload);
    protected Task<(bool ok, string message)> PutAsync(string url, object? payload) => SendAsync(HttpMethod.Put, url, payload);
    protected Task<(bool ok, string message)> PatchAsync(string url, object? payload) => SendAsync(HttpMethod.Patch, url, payload);
    protected Task<(bool ok, string message)> DeleteAsync(string url, object? payload) => SendAsync(HttpMethod.Delete, url, payload);
}

public class SeguridadModuloApiService : AlpesApiServiceBase
{
    public SeguridadModuloApiService(HttpClient http) : base(http) { }

    public Task<List<UsuarioViewModel>> ListarUsuariosAsync(string estado = "ACTIVO") => GetListAsync<UsuarioViewModel>($"api/Seguridad/usuarios?estado={estado}");
    public Task<List<RolViewModel>> ListarRolesAsync(string estado = "ACTIVO") => GetListAsync<RolViewModel>($"api/Seguridad/roles?estado={estado}");
    public Task<List<PermisoViewModel>> ListarPermisosAsync(string estado = "ACTIVO") => GetListAsync<PermisoViewModel>($"api/Seguridad/permisos?estado={estado}");

    public Task<(bool ok, string message)> CrearUsuarioAsync(string username, string passwordPlano, string estado)
        => PostAsync("api/Seguridad/usuarios", new { username, passwordPlano, estado });

    public Task<(bool ok, string message)> ActualizarUsuarioAsync(int usuarioId, string username, string estado)
        => SendWithFallbackAsync(HttpMethod.Put,
            ("api/Seguridad/usuarios", new { usuarioId, username, estado }),
            ($"api/Seguridad/usuarios/{usuarioId}", new { usuarioId, username, estado }));

    public Task<(bool ok, string message)> CambiarPasswordAsync(int usuarioId, string passwordPlano)
        => SendWithFallbackAsync(HttpMethod.Patch,
            ("api/Seguridad/usuarios/password", new { usuarioId, passwordPlano }),
            ($"api/Seguridad/usuarios/{usuarioId}/password", new { usuarioId, passwordPlano }));

    public Task<(bool ok, string message)> BloquearUsuarioAsync(int usuarioId)
        => SendWithFallbackAsync(HttpMethod.Patch,
            ("api/Seguridad/usuarios/bloquear", new { usuarioId }),
            ($"api/Seguridad/usuarios/{usuarioId}/bloquear", new { usuarioId }));

    public Task<(bool ok, string message)> DesbloquearUsuarioAsync(int usuarioId)
        => SendWithFallbackAsync(HttpMethod.Patch,
            ("api/Seguridad/usuarios/desbloquear", new { usuarioId }),
            ($"api/Seguridad/usuarios/{usuarioId}/desbloquear", new { usuarioId }));

    public Task<(bool ok, string message)> InactivarUsuarioAsync(int usuarioId)
        => SendWithFallbackAsync(HttpMethod.Delete,
            ("api/Seguridad/usuarios/logico", new { usuarioId }),
            ($"api/Seguridad/usuarios/{usuarioId}/logico", new { usuarioId }));

    public Task<(bool ok, string message)> CrearRolAsync(string nombre, string descripcion, string estado)
        => PostAsync("api/Seguridad/roles", new { nombre, descripcion, estado });

    public Task<(bool ok, string message)> ActualizarRolAsync(int rolId, string nombre, string descripcion, string estado)
        => SendWithFallbackAsync(HttpMethod.Put,
            ("api/Seguridad/roles", new { rolId, nombre, descripcion, estado }),
            ($"api/Seguridad/roles/{rolId}", new { rolId, nombre, descripcion, estado }));

    public Task<(bool ok, string message)> InactivarRolAsync(int rolId)
        => SendWithFallbackAsync(HttpMethod.Delete,
            ("api/Seguridad/roles/logico", new { rolId }),
            ($"api/Seguridad/roles/{rolId}/logico", new { rolId }));

    public Task<(bool ok, string message)> CrearPermisoAsync(string nombre, string descripcion, string estado)
        => PostAsync("api/Seguridad/permisos", new { nombre, descripcion, estado });

    public Task<(bool ok, string message)> ActualizarPermisoAsync(int permisoId, string nombre, string descripcion, string estado)
        => SendWithFallbackAsync(HttpMethod.Put,
            ("api/Seguridad/permisos", new { permisoId, nombre, descripcion, estado }),
            ($"api/Seguridad/permisos/{permisoId}", new { permisoId, nombre, descripcion, estado }));

    public Task<(bool ok, string message)> InactivarPermisoAsync(int permisoId)
        => SendWithFallbackAsync(HttpMethod.Delete,
            ("api/Seguridad/permisos/logico", new { permisoId }),
            ($"api/Seguridad/permisos/{permisoId}/logico", new { permisoId }));

    public Task<(bool ok, string message)> AsignarRolUsuarioAsync(int usuarioId, int rolId)
        => SendWithFallbackAsync(HttpMethod.Post,
            ("api/Seguridad/usuarios/roles", new { usuarioId, rolId, fechaInicio = DateTime.Now, fechaFin = (DateTime?)null, estado = "ACTIVO" }),
            ($"api/Seguridad/usuarios/{usuarioId}/roles", new { usuarioId, rolId, fechaInicio = DateTime.Now, fechaFin = (DateTime?)null, estado = "ACTIVO" }));

    public Task<(bool ok, string message)> QuitarRolUsuarioAsync(int usuarioRolId)
        => SendWithFallbackAsync(HttpMethod.Patch,
            ("api/Seguridad/usuarios/roles/quitar", new { usuarioRolId }),
            ($"api/Seguridad/usuarios/roles/{usuarioRolId}/quitar", new { usuarioRolId }));

    public Task<(bool ok, string message)> AsignarPermisoRolAsync(int rolId, int permisoId)
        => SendWithFallbackAsync(HttpMethod.Post,
            ("api/Seguridad/roles/permisos", new { rolId, permisoId, estado = "ACTIVO" }),
            ($"api/Seguridad/roles/{rolId}/permisos", new { rolId, permisoId, estado = "ACTIVO" }));

    public Task<(bool ok, string message)> QuitarPermisoRolAsync(int rolPermisoId)
        => SendWithFallbackAsync(HttpMethod.Patch,
            ("api/Seguridad/roles/permisos/quitar", new { rolPermisoId }),
            ($"api/Seguridad/roles/permisos/{rolPermisoId}/quitar", new { rolPermisoId }));

    public Task<(bool ok, string message)> RegistrarBitacoraAsync(int usuarioId, string username, string resultado, string detalle)
        => PostAsync("api/Seguridad/bitacora-acceso", new { usuarioId, username, ip = "Frontend MVC", userAgent = "Panel ERP", resultado, detalle });
}

public class EnviosModuloApiService : AlpesApiServiceBase
{
    public EnviosModuloApiService(HttpClient http) : base(http) { }

    public Task<List<EnvioModuloViewModel>> ListarPorEstadoAsync(string estado = "PREPARANDO") => GetListAsync<EnvioModuloViewModel>($"api/Envios/estado/{estado}");
    public Task<EnvioModuloViewModel?> ObtenerAsync(int envioId) => GetDataAsync<EnvioModuloViewModel>($"api/Envios/{envioId}");
    public Task<List<FiltroOrdenDisponibleResponse>> OrdenesDisponiblesAsync() => GetListAsync<FiltroOrdenDisponibleResponse>("api/Envios/filtros/ordenes-disponibles");
    public Task<List<FiltroDireccionClienteResponse>> DireccionesClienteAsync(int clienteId) => GetListAsync<FiltroDireccionClienteResponse>($"api/Envios/filtros/direcciones-cliente/{clienteId}");

    public Task<(bool ok, string message)> CrearAsync(int ordenVentaId, int clienteDireccionId, string numeroGuia, string transportista, decimal costoEnvio, string estado, int usuarioId)
        => PostAsync("api/Envios", new { ordenVentaId, clienteDireccionId, numeroGuia, transportista, costoEnvio, fechaEnvio = DateTime.Now, fechaEntregaEstimada = DateTime.Now.AddDays(3), estado, observaciones = "Creado desde frontend", usuarioId = usuarioId == 0 ? 1 : usuarioId });

    public Task<(bool ok, string message)> CambiarEstadoAsync(int envioId, string estado, int usuarioId)
        => SendWithFallbackAsync(HttpMethod.Patch,
            ("api/Envios/estado", new { envioId, estado, usuarioId = usuarioId == 0 ? 1 : usuarioId }),
            ($"api/Envios/{envioId}/estado", new { envioId, estado, usuarioId = usuarioId == 0 ? 1 : usuarioId }));

    public Task<(bool ok, string message)> ConfirmarEntregaAsync(int envioId, int usuarioId)
        => SendWithFallbackAsync(HttpMethod.Patch,
            ("api/Envios/confirmar-entrega", new { envioId, fechaEntregaReal = DateTime.Now, observaciones = "Entrega confirmada desde frontend", usuarioId = usuarioId == 0 ? 1 : usuarioId }),
            ($"api/Envios/{envioId}/confirmar-entrega", new { envioId, fechaEntregaReal = DateTime.Now, observaciones = "Entrega confirmada desde frontend", usuarioId = usuarioId == 0 ? 1 : usuarioId }));
}

public class ReportesModuloApiService : AlpesApiServiceBase
{
    public ReportesModuloApiService(HttpClient http) : base(http) { }

    public Task<List<FiltroClienteResponse>> ClientesAsync() => GetListAsync<FiltroClienteResponse>("api/ReportesCliente/filtros/clientes");
    public Task<List<FiltroCanalVentaResponse>> CanalesAsync() => GetListAsync<FiltroCanalVentaResponse>("api/ReportesVentas/filtros/canales-venta");
    public Task<List<FiltroCiudadResponse>> CiudadesAsync() => GetListAsync<FiltroCiudadResponse>("api/ReportesVentas/filtros/ciudades");
    public Task<List<FiltroCorteCajaResponse>> CortesAsync(string? estado = null) => GetListAsync<FiltroCorteCajaResponse>("api/ReportesCaja/filtros/cortes-caja" + (string.IsNullOrWhiteSpace(estado) ? "" : $"?estado={estado}"));

    public async Task<string> ObtenerTextoAsync(string url)
    {
        try
        {
            var text = await Http.GetStringAsync(url);
            return text;
        }
        catch (Exception ex) { return $"{{\"success\":false,\"message\":\"{ex.Message}\"}}"; }
    }

    public async Task<string> PostTextoAsync(string url, object? payload)
    {
        try
        {
            var resp = await Http.PostAsJsonAsync(url, payload);
            return await resp.Content.ReadAsStringAsync();
        }
        catch (Exception ex) { return $"{{\"success\":false,\"message\":\"{ex.Message}\"}}"; }
    }
}
