using ECommerce.Application.Services.Category;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPI : ControllerBase
    {
        private ICategoryService _categoryService;
        public CategoryAPI(ICategoryService categoryService) 
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
