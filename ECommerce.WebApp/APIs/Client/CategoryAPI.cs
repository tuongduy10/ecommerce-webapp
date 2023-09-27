using ECommerce.Application.BaseServices.Category;
using ECommerce.Application.BaseServices.SubCategory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPI : ControllerBase
    {
        private ICategoryService _categoryService;
        private ISubCategoryService _subCategoryService;
        public CategoryAPI(ICategoryService categoryService, ISubCategoryService subCategoryService) 
        {
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var list = await _categoryService.getAll();
            return Ok(new { status = "success", data = list });
        }
        [HttpGet("getSubCategoryCategoryId")]
        public async Task<IActionResult> getSubCategoryCategoryId([FromBody] int id)
        {
            var list = await _subCategoryService.getSubCategoryByCategoryId(id);
            return Ok(new { status = "success", data = list });
        }
        [HttpGet("getSubCategoryByBrandId")]
        public async Task<IActionResult> getSubCategoryByBrandId(int id)
        {
            var result = await _subCategoryService.getSubCategoryInBrand(id);
            return Ok(result);
        }
        [HttpGet("getBaseOptionsBySubCategoryId")] // Get
        public async Task<IActionResult> getOptionsBySubCategoryId(int id)
        {
            var options = await _subCategoryService.getOptionBySubCategoryId(id);
            return Ok(options);
        }
        [HttpGet("getAttributesBySubCategoryId")]
        public async Task<IActionResult> getAttributesBySubCategoryId(int id)
        {
            var attributes = await _subCategoryService.getAttributeBySubCategoryId(id);
            return Ok(attributes);
        }
    }
}
