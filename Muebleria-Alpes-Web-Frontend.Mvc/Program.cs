using Microsoft.AspNetCore.Authentication.Cookies;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Finanzas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath  = "/Login/Login";
        options.LogoutPath = "/Login/Logout";
        // Si un cliente intenta acceder al panel admin → vuelve a la tienda
        options.AccessDeniedPath = "/Tienda/Index";
        options.ExpireTimeSpan   = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// Política: solo usuarios autenticados que NO sean clientes pueden entrar al admin
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin", policy =>
        policy.RequireAuthenticatedUser()
              .RequireAssertion(ctx =>
              {
                  var rol = ctx.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                  return rol != null &&
                         !rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase);
              }));
});

var apiUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7015/";

// Named HTTP client for generic use
builder.Services.AddHttpClient("BackendApi", (sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["ApiSettings:BaseUrl"] ?? "https://localhost:7015/";
    client.BaseAddress = new Uri(baseUrl);
});

// Servicios existentes
builder.Services.AddScoped<TestApiService>();

builder.Services.AddHttpClient<ProductoImagenApiService>(client => client.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<InventarioApiService>(client => client.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<Muebleria_Alpes_Web_Frontend.Mvc.Services.Catalogos.CatalogoApiService>(client => client.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<FinanzasApiService>(client => client.BaseAddress = new Uri(apiUrl));

// RRHH
builder.Services.AddHttpClient<EmpleadoApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<DepartamentoApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<NominaApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<AsistenciaApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<VacacionesApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<PuestoApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<TurnoApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<EvaluacionApiService>(c => c.BaseAddress = new Uri(apiUrl));

// Products & Catalog
builder.Services.AddHttpClient<ProductoApiService>(c => c.BaseAddress = new Uri(apiUrl));

// Clients & Sales
builder.Services.AddHttpClient<ClienteApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<AdminApiService>(c => c.BaseAddress = new Uri(apiUrl));

// Inventory & Logistics
builder.Services.AddHttpClient<InventarioApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<LogisticaApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<PromocionApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<DevolucionApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<CajaApiService>(c => c.BaseAddress = new Uri(apiUrl));

// Store (Tienda)
builder.Services.AddHttpClient<TiendaApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<CarritoApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<VentasApiService>(c => c.BaseAddress = new Uri(apiUrl));

// Reports
builder.Services.AddHttpClient<ReportesApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<ReportesModuloApiService>(c => c.BaseAddress = new Uri(apiUrl));

// Seguridad / Envíos / Reportes
builder.Services.AddHttpClient<SeguridadModuloApiService>(c => c.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<EnviosModuloApiService>(c => c.BaseAddress = new Uri(apiUrl));

// Auth
builder.Services.AddHttpClient<AuthApiService>(c => c.BaseAddress = new Uri(apiUrl));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// La tienda es la página de inicio pública (no requiere login)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tienda}/{action=Index}/{id?}");

// Generar placeholder por defecto si no existe
var webRoot = app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
var imgPath = Path.Combine(webRoot, "img", "no-image.png");
if (!File.Exists(imgPath))
{
    var imgDir = Path.GetDirectoryName(imgPath);
    if (imgDir != null && !Directory.Exists(imgDir)) Directory.CreateDirectory(imgDir);
    // Base64 de un pixel gris 1x1 transparente/sutil (válido como PNG)
    var base64Png = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAFBhaW50Lk5FVCB2My41LjbQg61aAAAADUlEQVQYV2P4//8/AwAI/AL+Xo/xkwAAAABJRU5ErkJggg==";
    File.WriteAllBytes(imgPath, Convert.FromBase64String(base64Png));
}

app.Run();
