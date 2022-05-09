using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Category;
using ECommerce.WebApp.Models.Brand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageBrandController : Controller
    {
        private IBrandService _brandService;
        private ICategoryService _categoryService;
        public ManageBrandController(IBrandService brandService, ICategoryService categoryService)
        {
            _brandService = brandService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var listBrand = await _brandService.getAllManage();
            var listCategory = await _categoryService.getAll();
            var model = new ManageBrandViewModel()
            {
                listBrand = listBrand,
                listCategory = listCategory
            };

            return View(model);
        }
    }
}
