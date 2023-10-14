using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Application.Services.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs.Admin
{
    //[Authorize(AuthenticationSchemes = "AdminAuth")]
    //[Authorize(Policy="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ManageBrandAPI : ControllerBase
    {
        private IBrandService _brandService;
        private ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ManageBrandAPI(IBrandService brandService, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _brandService = brandService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("getBrandById")]
        public async Task<IActionResult> getBrandById(int id)
        {
            var result = await _brandService.getBrandById(id);
            return Ok(result);
        }
    }
}
