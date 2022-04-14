using ECommerce.Application.Services.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPI : ControllerBase
    {
        private ISubCategoryService _categoryService;
        public CategoryAPI(ISubCategoryService categoryService) 
        {
            _categoryService = categoryService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var list = await _categoryService.getAll();
            return Ok(new { status = "success", data = list });
        }
    }
}
