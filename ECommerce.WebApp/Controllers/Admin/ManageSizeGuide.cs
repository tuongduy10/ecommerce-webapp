using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.SubCategory;
using ECommerce.WebApp.Models.SizeGuide;
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
        private IProductService _productService;
        public ManageSizeGuide(ISubCategoryService subCategoryService, IProductService productService)
        {
            _subCategoryService = subCategoryService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var subCategories = await _subCategoryService.getAll();
            return View(subCategories);
        }
        public async Task<IActionResult> AddSizeGuide(SizeGuideAddRequest request)
        {
            var result = await _productService.AddSizeGuide(request);
            if (result.isSucceed) return Ok(result.Message);
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _productService.SizeGuideDetail(id);
            var subs = await _subCategoryService.getAll();
            if (result.Data == null) return NotFound(result.Message);
            var model = new SizeGuideDetailViewModel
            {
                SizeGuide = result.Data,
                SubCategories = subs
            };
            return View(model);
        }
        public async Task<IActionResult> UpdateSizeGuide(SizeGuideAddRequest request)
        {
            var result = await _productService.UpdateSizeGuide(request);
            if (result.isSucceed) return Ok(result.Message); 
            return BadRequest();
        }
        public async Task<IActionResult> DeleteSizeGuide(int id)
        {
            return Ok();
        }
    }
}
