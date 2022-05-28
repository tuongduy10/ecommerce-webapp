using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Category;
using ECommerce.Application.Services.Configurations;
using ECommerce.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IFooterService _footerService;

        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService, IBrandService brandService, IFooterService footerService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _brandService = brandService;
            _footerService = footerService;
        }

        public async Task<IActionResult> Index()
        {
            var _listBrand = await _brandService.getAll();
            var _listCategory = await _categoryService.getAll();

            var model = new HomeViewModel()
            {
                listBrand = _listBrand,
                listCategory = _listCategory
            };

            return View(model);
        }
        public async Task<IActionResult> Payment()
        {
            var result = await _footerService.getBlogDetail(7);
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
