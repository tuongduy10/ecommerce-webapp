using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.SubCategory;
using ECommerce.WebApp.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Client
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;
        public ProductController(IProductService productService, ISubCategoryService subCategoryService)
        {
            _productService = productService;
            _subCategoryService = subCategoryService;
        }
        public async Task<IActionResult> ProductInBrand(int BrandId)
        {
            var listProduct = await _productService.getProductsInBrand(BrandId);
            var listSubCategory = await _subCategoryService.getSubCategoryInBrand(BrandId);
            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
                listSubCategory = listSubCategory
            };

            return View(model);
        }

        public async Task<IActionResult> ProductAvaliable()
        {
            return View();
        }
        public async Task<IActionResult> ProductPreOrder()
        {
            return View();
        }
    }
}
