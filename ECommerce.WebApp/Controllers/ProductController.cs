using ECommerce.Application.Services.Comment;
using ECommerce.Application.Services.Comment.Request;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;
        public ProductController(
            IProductService productService,
            ICommentService commentService)
        {
            _productService = productService;
            _commentService = commentService;
        }
        [AllowAnonymous]
        [HttpPost("product-list")]
        public async Task<IActionResult> getProductList(ProductGetRequest request)
        {
            var res = await _productService.getProductList(request);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("managed-products")]
        public async Task<IActionResult> getManagedProductList(ProductGetRequest request)
        {
            var res = await _productService.getManagedProductList(request);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpGet("product-detail")]
        public async Task<IActionResult> getProductDetail(int id = -1)
        {
            var result = await _productService.getProductDetail(id);
            if (!result.isSucceed)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("product-review")]
        public async Task<IActionResult> getProductReview(RateGetRequest request)
        {
            var result = await _commentService.getRates(request);
            if (!result.isSucceed)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("rate-product")]
        public async Task<IActionResult> rateProduct()
        {
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("save")]
        public async Task<IActionResult> save(ProductSaveRequest request)
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
