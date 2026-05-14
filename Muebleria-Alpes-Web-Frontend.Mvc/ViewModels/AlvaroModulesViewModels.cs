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

public class UsuarioAlvaroViewModel
{
    [JsonPropertyName("usuarioId")] public int UsuarioId { get; set; }
    [JsonPropertyName("id")] public int Id { get => UsuarioId; set => UsuarioId = value; }
    [JsonPropertyName("codigo")] public string? Codigo { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
    [JsonPropertyName("fechaCreacion")] public DateTime? FechaCreacion { get; set; }
}

public class RolAlvaroViewModel
{
    [JsonPropertyName("rolId")] public int RolId { get; set; }
    [JsonPropertyName("id")] public int Id { get => RolId; set => RolId = value; }
    [JsonPropertyName("codigo")] public string? Codigo { get; set; }
    [JsonPropertyName("nombre")] public string? Nombre { get; set; }
    [JsonPropertyName("descripcion")] public string? Descripcion { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
}

public class PermisoAlvaroViewModel
{
    [JsonPropertyName("permisoId")] public int PermisoId { get; set; }
    [JsonPropertyName("id")] public int Id { get => PermisoId; set => PermisoId = value; }
    [JsonPropertyName("codigo")] public string? Codigo { get; set; }
    [JsonPropertyName("nombre")] public string? Nombre { get; set; }
    [JsonPropertyName("descripcion")] public string? Descripcion { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
}

public class EnvioAlvaroViewModel
{
    [JsonPropertyName("envioId")] public int EnvioId { get; set; }
    [JsonPropertyName("ordenVentaId")] public int OrdenVentaId { get; set; }
    [JsonPropertyName("clienteDireccionId")] public int ClienteDireccionId { get; set; }
    [JsonPropertyName("numeroGuia")] public string? NumeroGuia { get; set; }
    [JsonPropertyName("transportista")] public string? Transportista { get; set; }
    [JsonPropertyName("costoEnvio")] public decimal CostoEnvio { get; set; }
    [JsonPropertyName("fechaEnvio")] public DateTime? FechaEnvio { get; set; }
    [JsonPropertyName("fechaEntregaEstimada")] public DateTime? FechaEntregaEstimada { get; set; }
    [JsonPropertyName("fechaEntregaReal")] public DateTime? FechaEntregaReal { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
    [JsonPropertyName("observaciones")] public string? Observaciones { get; set; }
}

public class FiltroClienteResponse
{
    [JsonPropertyName("clienteId")] public int ClienteId { get; set; }
    [JsonPropertyName("codigo")] public string? Codigo { get; set; }
    [JsonPropertyName("nombre")] public string? Nombre { get; set; }
    [JsonPropertyName("documento")] public string? Documento { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
}

public class FiltroCanalVentaResponse
{
    [JsonPropertyName("canalVentaId")] public int CanalVentaId { get; set; }
    [JsonPropertyName("codigo")] public string? Codigo { get; set; }
    [JsonPropertyName("nombre")] public string? Nombre { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
}

public class FiltroCiudadResponse
{
    [JsonPropertyName("ciudadId")] public int CiudadId { get; set; }
    [JsonPropertyName("codigo")] public string? Codigo { get; set; }
    [JsonPropertyName("nombre")] public string? Nombre { get; set; }
    [JsonPropertyName("departamentoId")] public int? DepartamentoId { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
}

public class FiltroCorteCajaResponse
{
    [JsonPropertyName("corteCajaId")] public int CorteCajaId { get; set; }
    [JsonPropertyName("fechaCorte")] public DateTime? FechaCorte { get; set; }
    [JsonPropertyName("montoInicial")] public decimal MontoInicial { get; set; }
    [JsonPropertyName("montoFinal")] public decimal MontoFinal { get; set; }
    [JsonPropertyName("totalVentas")] public decimal TotalVentas { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
}

public class FiltroOrdenDisponibleResponse
{
    [JsonPropertyName("ordenVentaId")] public int OrdenVentaId { get; set; }
    [JsonPropertyName("numeroOrden")] public string? NumeroOrden { get; set; }
    [JsonPropertyName("clienteId")] public int ClienteId { get; set; }
    [JsonPropertyName("clienteNombre")] public string? ClienteNombre { get; set; }
    [JsonPropertyName("total")] public decimal Total { get; set; }
    [JsonPropertyName("estadoOrden")] public string? EstadoOrden { get; set; }
}

public class FiltroDireccionClienteResponse
{
    [JsonPropertyName("clienteDireccionId")] public int ClienteDireccionId { get; set; }
    [JsonPropertyName("clienteId")] public int ClienteId { get; set; }
    [JsonPropertyName("ciudadId")] public int CiudadId { get; set; }
    [JsonPropertyName("ciudadNombre")] public string? CiudadNombre { get; set; }
    [JsonPropertyName("direccion")] public string? Direccion { get; set; }
    [JsonPropertyName("tipo")] public string? Tipo { get; set; }
    [JsonPropertyName("principal")] public string? Principal { get; set; }
    [JsonPropertyName("estado")] public string? Estado { get; set; }
}

public class ReporteMetricaViewModel
{
    public string Titulo { get; set; } = "";
    public string Valor { get; set; } = "0";
    public string Icono { get; set; } = "bi-graph-up";
}

public class ReporteGenericoFila : Dictionary<string, object?> { }
