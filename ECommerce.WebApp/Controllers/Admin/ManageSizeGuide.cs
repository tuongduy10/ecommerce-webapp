using ECommerce.Application.Services.SubCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageSizeGuide : Controller
    {
        private ISubCategoryService _subCategoryService;
        public ManageSizeGuide(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }
        public async Task<IActionResult> Index()
        {
            var subCategories = await _subCategoryService.getAll();
            return View(subCategories);
        }
    }
}
