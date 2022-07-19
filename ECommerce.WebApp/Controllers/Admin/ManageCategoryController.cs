using ECommerce.Application.Services.Category;
using ECommerce.Application.Services.Category.Dtos;
using ECommerce.Application.Services.SubCategory;
using ECommerce.Application.Services.SubCategory.Dtos;
using ECommerce.WebApp.Models.Category;
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
        public async Task<IActionResult> GetBaseOptionValue(int id)
        {
            var result = await _subCategoryService.getBaseOptionValueByOptionId(id);
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
        public async Task<IActionResult> Detail(int id)
        {
            var category = await _categoryService.Detail(id);
            if (category == null) return NotFound();
            var subcategories = await _subCategoryService.getSubCategoryByCategoryWithOptsAndAttrs(id);
            var model = new CategoryDetailViewModel
            {
                category = category,
                subcategories = subcategories
            };
            
            return View(model);
        }
        public async Task<IActionResult> Update(CategoryModel request)
        {
            var result = await _categoryService.Update(request);
            if (result.isSucceed) return Ok(result.Message);
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> AddSubcategory(SubCategoryCreateRequest request)
        {
            var result = await _subCategoryService.Create(request);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UpdateSubcategory(SubCategoryUpdateRequest request)
        {
            var result = await _subCategoryService.Update(request);
            if (result.isSucceed) return Ok(result.Message);
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var result = await _subCategoryService.Delete(id);
            if (result.isSucceed) return Ok(result.Message);
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UpdateAttributeForSub(SubListUpdateRequest request)
        {
            var result = await _subCategoryService.UpdateAttributeForSub(request);
            if (result.isSucceed) return Ok(result.Message);
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UpdateOptionForSub(SubListUpdateRequest request)
        {
            var result = await _subCategoryService.UpdateOptionForSub(request);
            if (result.isSucceed) return Ok(result.Message);
            return BadRequest(result.Message);
        }
    }
}
