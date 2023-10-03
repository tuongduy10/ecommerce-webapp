using ECommerce.Application.Services.Comment;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    [ApiController]
    [Route("api/discount")]
    public class DiscountController : ControllerBase
    {
        private readonly IProductService _productService;
        public DiscountController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpPost("get-discount")]
        public async Task<IActionResult> getDiscount(DiscountGetRequest request)
        {
            var result = await _productService.getDiscount(request);
            if (!result.isSucceed)
                return BadRequest(result.Message);
            return Ok(result);
        }
    }
}
