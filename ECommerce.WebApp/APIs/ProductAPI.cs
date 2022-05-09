using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.FilterProduct;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.SubCategory;
using ECommerce.WebApp.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        [HttpPost("getProductInBrand")]
        public async Task<IActionResult> getProductInBrand([FromBody] ProductGetRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var listProduct = await _productService.getProductPaginated(request);
            var listSubCategory = await _subCategoryService.getSubCategoryInBrand(request.BrandId);
            var brand = await _brandService.getBrandById(request.BrandId);
            var filter = await _filterService.listFilterModel(request.BrandId);

            var model = new ProductInBrandViewModel()
            {
                listProduct = listProduct,
                listSubCategory = listSubCategory,
                brand = brand,
                listFilterModel = filter,
            };

            return Ok(new { status = "success", data = model });
        }
    }
}
