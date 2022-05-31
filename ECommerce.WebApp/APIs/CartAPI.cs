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
        public CartAPI(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetProductPrice")]
        public async Task<IActionResult> GetProductPrice(int id, int type)
        {
            var result = await _productService.getProductPirce(id, type);
            return Ok(result);
        }
        [HttpPost("getCartTotalPrice")]
        public async Task<IActionResult> getCartTotalPrice(List<GetTotalPriceRequest> requests)
        {
            var sum = 0;
            foreach (var item in requests)
            {
                var price = await _productService.getProductPirce(item.id, item.typeId);
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
    }
    public class GetTotalPriceRequest
    {
        public int id { get; set; }
        public int typeId { get; set; }
        public int qty { get; set; }
    }
}
