using ECommerce.Application.Services.Discount;
using ECommerce.Application.Services.Product;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.WebApp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPI : ControllerBase
    {
        private IProductService _productService;
        private IDiscountService _discountService;
        public CartAPI(IProductService productService, IDiscountService discountService)
        {
            _productService = productService;
            _discountService = discountService;
        }
        [HttpGet("GetProductPrice")]
        public async Task<IActionResult> GetProductPrice(int id, int type)
        {
            var result = await _productService.getProductPrice(id, type);
            return Ok(result);
        }
        [HttpPost("getCartTotalPrice")]
        public async Task<IActionResult> getCartTotalPrice(List<GetTotalPriceRequest> requests)
        {
            var sum = 0;
            foreach (var item in requests)
            {
                var price = await _productService.getProductPrice(item.id, item.typeId);
                var total = 0;
                if (price.priceOnSell != null)
                {
                    total = (int)(price.priceOnSell * item.qty);
                }
                else
                {
                    total = (int)(price.price * item.qty);
                }
                 
                sum += (int)total;
            }
            return Ok(sum);
        }
        [HttpGet("GetDiscountValue")]
        public async Task<IActionResult> GetDiscountValue(string code)
        {
            var result = await _discountService.getDiscountValue(code);
            if (result == null)
            {
                return BadRequest("Mã giảm giá không hợp lệ hoặc hết hạn");
            }
            return Ok(result);
        }
    }
    public class GetTotalPriceRequest
    {
        public int id { get; set; }
        public int typeId { get; set; }
        public int qty { get; set; }
    }
}
