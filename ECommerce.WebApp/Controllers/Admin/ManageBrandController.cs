using ECommerce.Application.Constants;
using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Application.Services.Category;
using ECommerce.WebApp.Models.Brand;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
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
                newFileName = _manageFiles.GetFileName(request.ImagePath, "brand_");
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
                    _manageFiles.AddFile(request.ImagePath, newFileName, FILE_PREFIX);
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
        [HttpGet]
        public async Task<IActionResult> GetBrandsByShop(int id)
        {
            var list = await _brandService.getBrandsByShop(id);
            return Ok(list);
        }
    }
}
