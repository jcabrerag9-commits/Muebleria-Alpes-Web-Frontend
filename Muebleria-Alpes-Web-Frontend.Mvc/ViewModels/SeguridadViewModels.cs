using System.Text.Json.Serialization;

namespace Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

public class AlpesApiResponse<T>
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("message")] public string? Message { get; set; }
    [JsonPropertyName("data")] public T? Data { get; set; }
}

public class OperacionSimpleResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("message")] public string? Message { get; set; }
    [JsonPropertyName("resultado")] public string? Resultado { get; set; }
    [JsonPropertyName("mensaje")] public string? Mensaje { get; set; }
}

public class UsuarioViewModel
{
    [JsonPropertyName("usuarioId")] public int UsuarioId { get; set; }
    [JsonPropertyName("id")] public int Id { get => UsuarioId; set => UsuarioId = value; }
    [JsonPropertyName("codigo")] public string? Codigo { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
    [JsonPropertyName("fechaCreacion")] public DateTime? FechaCreacion { get; set; }
}

public class RolViewModel
{
    [JsonPropertyName("rolId")] public int RolId { get; set; }
    [JsonPropertyName("id")] public int Id { get => RolId; set => RolId = value; }
    [JsonPropertyName("codigo")] public string? Codigo { get; set; }
    [JsonPropertyName("nombre")] public string? Nombre { get; set; }
    [JsonPropertyName("descripcion")] public string? Descripcion { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
}

public class PermisoViewModel
{
    [JsonPropertyName("permisoId")] public int PermisoId { get; set; }
    [JsonPropertyName("id")] public int Id { get => PermisoId; set => PermisoId = value; }
    [JsonPropertyName("codigo")] public string? Codigo { get; set; }
    [JsonPropertyName("nombre")] public string? Nombre { get; set; }
    [JsonPropertyName("descripcion")] public string? Descripcion { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
}
