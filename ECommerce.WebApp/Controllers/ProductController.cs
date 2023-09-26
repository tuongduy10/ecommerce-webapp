using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/product")]
    public class ProductAPI : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductAPI(IProductService productService)
        {
            _productService = productService;
        }
        [AllowAnonymous]
        [HttpPost("product-list")]
        public async Task<IActionResult> getProductList([FromBody] ProductGetRequest request)
        {
            var res = await _productService.getProductList(request);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("managed-products")]
        public async Task<IActionResult> getManagedProductList([FromBody] ProductGetRequest request)
        {
            var res = await _productService.getManagedProductList(request);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("save")]
        public async Task<IActionResult> save(ProductModel request)
        {
            return Ok();
        }
        [HttpPost("delete")]
        public async Task<IActionResult> delete(List<int> ids)
        {
            return Ok();
        }
    }
}
