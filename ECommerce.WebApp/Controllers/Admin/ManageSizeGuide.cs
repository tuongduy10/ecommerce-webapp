using ECommerce.Application.BaseServices.Product;
using ECommerce.Application.BaseServices.Product.Dtos;
using ECommerce.Application.BaseServices.SubCategory;
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
        private IProductBaseService _productService;
        public ManageSizeGuide(ISubCategoryService subCategoryService, IProductBaseService productService)
        {
            _subCategoryService = subCategoryService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var subs = await _subCategoryService.getAll();
            var sizes = await _productService.SizeGuideList();
            var model = new SizeGuideListViewModel
            {
                sizeGuides = sizes,
                subCategories = subs,
            };
            return View(model);
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
            var result = await _productService.DeleteSizeGuide(id);
            if (result.isSucceed) return Ok(result.Message);
            return BadRequest(result.Message);
        }
    }
}
