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
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart()
        {
            return Ok();
        }
    }
}
