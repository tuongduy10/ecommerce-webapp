using ECommerce.Application.Common;
using ECommerce.Application.Constants;
using ECommerce.Application.BaseServices.Brand;
using ECommerce.Application.BaseServices.Brand.Dtos;
using ECommerce.Application.BaseServices.Category;
using ECommerce.WebApp.Models.Brand;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageBrandController : Controller
    {
        private string FILE_PATH = FilePathConstant.BRAND_FILEPATH;
        private string FILE_PREFIX = FilePathConstant.BRAND_FILEPREFIX;

        private IBrandService _brandService;
        private ICategoryService _categoryService;
        private ManageFiles _manageFiles;
        public ManageBrandController(
            IBrandService brandService, 
            ICategoryService categoryService, 
            IWebHostEnvironment webHostEnvironment)
        {
            _brandService = brandService;
            _categoryService = categoryService;
            _manageFiles = new ManageFiles(webHostEnvironment);
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
        public async Task<IActionResult> AddBrand(BrandCreateRequest request)
        {
            if (request.ImagePath == null)
            {
                return BadRequest("Hình ảnh thương hiệu không được để trống");
            }

            var fileName = _manageFiles.GetFileName(request.ImagePath, FILE_PREFIX);
            request.BrandImagePath = fileName;
            var result = await _brandService.Create(request);
            if (result.isSucceed)
            {
                _manageFiles.AddFile(request.ImagePath, fileName, FILE_PATH);

                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UpdateBrand(BrandUpdateRequest request)
        {
            var newFileName = "";
            if (request.ImagePath != null)
            {
                newFileName = _manageFiles.GetFileName(request.ImagePath, FILE_PREFIX);
                request.BrandImagePath = newFileName;
            }
            var result = await _brandService.Update(request);
            if (result.isSucceed)
            {
                if (result.Data.newFileName != null)
                {
                    // remove previous image
                    var previousFileName = result.Data.previousFileName;
                    _manageFiles.DeleteFile(previousFileName, FILE_PATH);

                    // Save new images to folder
                    _manageFiles.AddFile(request.ImagePath, newFileName, FILE_PATH);
                }
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UpdateStatus(int id, bool status)
        {
            var result = await _brandService.UpdateStatus(id, status);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UpdateBrandsStatus(List<int> ids, bool status)
        {
            var result = await _brandService.UpdateBrandsStatus(ids, status);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UpdateBrandDescription(BrandUpdateRequest request)
        {
            var result = await _brandService.UpdateBrandDescription(request);
            if (!result.isSucceed) 
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
        public async Task<IActionResult> BrandDetail(int BrandId)
        {
            var brand = await _brandService.getBrandById(BrandId);
            if (brand == null) return NotFound();

            ViewBag.Brand = brand;

            return View();
        }
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var result = await _brandService.DeleteBrand(id);
            if (result.isSucceed)
            {
                // Delete file
                var previousFileName = result.Data;
                _manageFiles.DeleteFile(previousFileName, FILE_PATH);

                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> DeleteBrands(List<int> ids)
        {
            var result = await _brandService.DeleteBrands(ids);
            if (result.isSucceed)
            {
                var previousFileNames = result.Data;
                _manageFiles.DeleteFiles(previousFileNames, FILE_PATH);
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetBrandsByShop(int id)
        {
            var list = await _brandService.getBrandsByShop(id);
            return Ok(list);
        }
    }
}
