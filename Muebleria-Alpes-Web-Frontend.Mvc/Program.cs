using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.RecursosHumanos;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Productos;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Inventario;
using Muebleria_Alpes_Web_Frontend.Mvc.Services.Finanzas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

builder.Services.AddHttpClient("BackendApi", client => client.BaseAddress = new Uri(baseUrl));

builder.Services.AddScoped<TestApiService>();
builder.Services.AddHttpClient<EmpleadoApiService>(client => client.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<DepartamentoApiService>(client => client.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<ProductoApiService>(client => client.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<ProductoImagenApiService>(client => client.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<InventarioApiService>(client => client.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<Muebleria_Alpes_Web_Frontend.Mvc.Services.Catalogos.CatalogoApiService>(client => client.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<FinanzasApiService>(client => client.BaseAddress = new Uri(baseUrl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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
