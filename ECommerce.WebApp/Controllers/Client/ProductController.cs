using ECommerce.Application.Services.Brand;
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
        private readonly IBrandService _brandService;
        public ProductController(IProductService productService, ISubCategoryService subCategoryService, IBrandService brandService)
        {
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
        }
        public async Task<IActionResult> ProductInBrand(int BrandId, int pageIndex = 1)
        {
            int itemsInPage = 5;
            var listProduct = await _productService.getProductPaginated(BrandId, pageIndex, itemsInPage);
            var listSubCategory = await _subCategoryService.getSubCategoryInBrand(BrandId);
            var brand = await _brandService.getBrandById(BrandId);

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
                listSubCategory = listSubCategory,
                brand = brand
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
