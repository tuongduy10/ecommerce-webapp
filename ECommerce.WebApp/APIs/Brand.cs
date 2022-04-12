using ECommerce.Application.Services.Brand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class Brand : ControllerBase
    {
        private IBrandService _brandService;
        public Brand(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var list = await _brandService.getAll();
            return Ok(new { status = "success", data = list });
        }
    }
}
