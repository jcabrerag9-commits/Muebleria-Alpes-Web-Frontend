using Microsoft.AspNetCore.Mvc;
using Muebleria_Alpes_Web_Frontend.Mvc.Models;
using Muebleria_Alpes_Web_Frontend.Mvc.Services;
using System.Diagnostics;

namespace Muebleria_Alpes_Web_Frontend.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestApiService _testApiService;

        public HomeController(TestApiService testApiService)
        {
            _testApiService = testApiService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _testApiService.ProbarConexionAsync();
            return View(model);
        }
    }
}
