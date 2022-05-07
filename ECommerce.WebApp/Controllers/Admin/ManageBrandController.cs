using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Category;
using ECommerce.WebApp.Models.Brand;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    public class ManageBrandController : Controller
    {
        private IBrandService _brandService;
        private ICategoryService _categoryService;
        public ManageBrandController(IBrandService brandService, ICategoryService categoryService)
        {
            _brandService = brandService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var listBrand = await _brandService.getAllManage();
            var listCategory = await _categoryService.getAll();
            var model = new ManageBrandViewModel()
            {
                listBrand = listBrand,
                listCategory = listCategory
            };

            return View(model);
        }
    }
}
