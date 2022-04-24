using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.FilterProduct;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.SubCategory;
using ECommerce.WebApp.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPI : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IBrandService _brandService;
        private readonly IFilterProductService _filterService;
        public ProductAPI(IProductService productService, ISubCategoryService subCategoryService, IBrandService brandService, IFilterProductService filterService)
        {
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
            _filterService = filterService;
        }

        [HttpGet("getProductInBrand")]
        public async Task<IActionResult> getProductInBrand([FromQuery]int BrandId, int pageIndex = 1)
        {
            int itemsInPage = 30;
            var listProduct = await _productService.getProductPaginated(BrandId, pageIndex, itemsInPage);
            var listSubCategory = await _subCategoryService.getSubCategoryInBrand(BrandId);
            var brand = await _brandService.getBrandById(BrandId);
            var filter = await _filterService.listFilterModel(BrandId);

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
                listSubCategory = listSubCategory,
                brand = brand,
                listFilterModel = filter,
            };

            return Ok(new { status = "success", data = model });
        }
        [HttpGet("getProductBySubCategory")]
        public async Task<IActionResult> getProductBySubCategory([FromQuery] int BrandId, int SubCategoryId, int pageIndex = 1)
        {
            int itemsInPage = 30;
            var listProduct = await _productService.getProductBySubCategoryPaginated(BrandId, SubCategoryId, pageIndex, itemsInPage);
            var listSubCategory = await _subCategoryService.getSubCategoryInBrand(BrandId);
            var brand = await _brandService.getBrandById(BrandId);
            var filter = await _filterService.listFilterModel(BrandId);

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
                listSubCategory = listSubCategory,
                brand = brand,
                listFilterModel = filter
            };

            return Ok(new { status = "success", data = model });
        }
    }
}
