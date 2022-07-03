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
    public class BrandAPI : ControllerBase
    {
        private IBrandService _brandService;
        public BrandAPI(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var list = await _brandService.getAll();
            return Ok(new { status = "success", data = list });
        }
        [HttpGet("getAllBrandInCategory")]
        public async Task<IActionResult> getAllBrandInCategory([FromQuery]int id)
        {
            var list = await _brandService.getAllBrandInCategory(id);
            return Ok(new { status = "success", data = list });
        }
    }
}
