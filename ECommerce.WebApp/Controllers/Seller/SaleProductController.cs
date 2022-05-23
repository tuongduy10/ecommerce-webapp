using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.SubCategory;
using ECommerce.WebApp.Models.SaleProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Seller
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Seller")]
    public class SaleProductController : Controller
    {
        private IProductService _productService;
        private ISubCategoryService _subCategoryService;
        private IBrandService _brandService;
        public SaleProductController(IProductService productService, ISubCategoryService subCategoryService, IBrandService brandService)
        {
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> ProductList(int subCategoryId = 0)
        {
            var userId = Int32.Parse(User.Claims.Where(i => i.Type == "UserId").Select(i => i.Value).FirstOrDefault());
            var products = await _productService.getProductByUser(userId, subCategoryId);
            var subcategories = await _subCategoryService.getSubCategoryByUser(userId);
            var model = new ProductListViewModel()
            {
                products = products,
                subCategories = subcategories,
            };
            return View(model);
        }
        public async Task<IActionResult> AddProduct()
        {
            var brands = await _brandService.getAll();
            return View(brands);
        }
    }
}
