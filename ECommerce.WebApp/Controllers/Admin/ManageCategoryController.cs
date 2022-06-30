using ECommerce.Application.Services.Category;
using ECommerce.Application.Services.Category.Dtos;
using ECommerce.Application.Services.SubCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageCategoryController : Controller
    {
        private ICategoryService _categoryService;
        private ISubCategoryService _subCategoryService;
        public ManageCategoryController(ICategoryService categoryService, ISubCategoryService subCategoryService)
        {
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _categoryService.getAll();
            return View(list);
        }
        public async Task<IActionResult> GetOptionValue(int id)
        {
            var result = await _subCategoryService.getOptionValueByOptionId(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryCreateRequest request)
        {
            var result = await _categoryService.Create(request);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.Delete(id);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
