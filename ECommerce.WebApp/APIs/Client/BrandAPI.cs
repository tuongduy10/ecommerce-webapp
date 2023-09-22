using ECommerce.Application.BaseServices.Brand;
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
        [HttpGet("getAllAvailable")]
        public async Task<IActionResult> getAllAvailable()
        {
            var list = await _brandService.GetAllAvailable();
            return Ok(new { status = "success", data = list });
        }
        [HttpGet("getAllAvailableInCategory")]
        public async Task<IActionResult> getAllAvailableInCategory([FromQuery]int id)
        {
            var list = await _brandService.getAllAvailableInCategory(id);
            return Ok(new { status = "success", data = list });
        }
    }
}
