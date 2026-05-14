using Muebleria_Alpes_Web_Frontend.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("BackendApi", (sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);
});

// Servicios existentes
builder.Services.AddScoped<TestApiService>();

// Servicios de Promociones y Devoluciones
builder.Services.AddScoped<PromocionService>();
builder.Services.AddScoped<DevolucionService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
