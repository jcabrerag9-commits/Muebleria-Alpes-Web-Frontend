using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using Muebleria_Alpes_Web_Frontend.Mvc.ViewModels;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class TiendaController : Controller
    {
        private readonly TiendaApiService _tiendaService;

        public TiendaController(TiendaApiService tiendaService)
        {
            _tiendaService = tiendaService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _tiendaService.ObtenerProductosAsync();
            var destacados = productos.Take(6).ToList();
            ViewData["Title"] = "Inicio";
            return View(destacados);
        }

        public async Task<IActionResult> Catalogo(string? buscar, int? categoriaId)
        {
            // Carga productos y categorías en paralelo
            var productosTask  = _tiendaService.ObtenerProductosAsync();
            var categoriasTask = _tiendaService.ObtenerCategoriasAsync();
            await Task.WhenAll(productosTask, categoriasTask);

            var todosProductos = productosTask.Result;
            var categorias     = categoriasTask.Result;

            // Filtro por texto (aplica antes de agrupar)
            if (!string.IsNullOrWhiteSpace(buscar))
                todosProductos = todosProductos
                    .Where(p => (p.Nombre ?? "").Contains(buscar, StringComparison.OrdinalIgnoreCase)
                             || (p.Descripcion ?? "").Contains(buscar, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            // Construir grupos: categoría → lista de productos
            // Se llama api/categorias/{id}/productos para cada categoría en paralelo
            var gruposTareas = categorias.Select(async cat =>
            {
                var ids = await _tiendaService.ObtenerProductoIdsDeCategoriaAsync(cat.Id);
                var prods = todosProductos.Where(p => ids.Contains(p.Id)).ToList();
                return (cat, prods);
            });

            var resultados = await Task.WhenAll(gruposTareas);

            // Si hay filtro de categoría, solo mostrar ese grupo
            var grupos = categoriaId.HasValue
                ? resultados
                    .Where(r => r.cat.Id == categoriaId.Value && r.prods.Count > 0)
                    .ToDictionary(r => r.cat, r => r.prods)
                : resultados
                    .Where(r => r.prods.Count > 0)
                    .ToDictionary(r => r.cat, r => r.prods);

            // Productos sin categoría (solo cuando no hay filtro)
            if (!categoriaId.HasValue)
            {
                var productosConCategoria = new HashSet<int>(resultados.SelectMany(r => r.prods.Select(p => p.Id)));
                var sinCategoria = todosProductos.Where(p => !productosConCategoria.Contains(p.Id)).ToList();
                if (sinCategoria.Count > 0)
                {
                    var sinCat = new TiendaCategoriaViewModel { Id = 0, Nombre = "Sin categoría" };
                    grupos[sinCat] = sinCategoria;
                }
            }

            ViewData["Title"] = "Catálogo";
            ViewBag.Buscar        = buscar;
            ViewBag.CategoriaId   = categoriaId;
            ViewBag.Categorias    = categorias;
            ViewBag.Grupos        = grupos;
            return View(todosProductos);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _tiendaService.ObtenerProductoAsync(id);
            if (producto == null)
                return NotFound();

            ViewData["Title"] = producto.Nombre ?? "Detalle";
            return View(producto);
        }

        public async Task<IActionResult> Ofertas()
        {
            var productos = await _tiendaService.ObtenerProductosAsync();
            var ofertas = productos.Where(p => p.TienePromocion).ToList();
            ViewData["Title"] = "Ofertas";
            return View(ofertas);
        }

        /// <summary>
        /// Proxy de imágenes: evita problemas de TLS/CORS al cargar imágenes
        /// directamente desde el backend. El browser siempre habla con el frontend.
        /// GET /Tienda/Imagen/{id} → proxía api/productoImagen/producto/{id}/principal
        /// </summary>
        [HttpGet]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Imagen(int id)
        {
            var result = await _tiendaService.ObtenerImagenBytesAsync(id);
            if (result == null) return NotFound();
            return File(result.Value.Bytes, result.Value.ContentType);
        }
    }
}
