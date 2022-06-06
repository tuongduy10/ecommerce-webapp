using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Application.Services.Category;
using ECommerce.WebApp.Models.Brand;
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
        private IBrandService _brandService;
        private ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ManageBrandController(IBrandService brandService, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _brandService = brandService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
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
            var fileName = Guid.NewGuid().ToString() + new FileInfo(request.ImagePath.FileName).Extension;
            request.BrandImagePath = fileName;
            var result = await _brandService.Create(request);
            if (result.isSucceed)
            {
                // Save images to folder
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/brand");
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    request.ImagePath.CopyTo(fileStream);
                }
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UpdateBrand(BrandUpdateRequest request)
        {
            if (request.ImagePath != null)
            {
                request.BrandImagePath = Guid.NewGuid().ToString() + new FileInfo(request.ImagePath.FileName).Extension;
            }
            var result = await _brandService.Update(request);
            if (result.isSucceed)
            {
                if (result.Data.newFileName != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/brand");
                    // remove previous image
                    var previousFileName = result.Data.previousFileName;
                    DirectoryInfo uploadDirectory = new DirectoryInfo(uploadsFolder);
                    foreach (FileInfo file in uploadDirectory.GetFiles())
                    {
                        if (file.Name == previousFileName)
                        {
                            file.Delete();
                        }
                    }

                    // Save new images to folder
                    string filePath = Path.Combine(uploadsFolder, result.Data.newFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        request.ImagePath.CopyTo(fileStream);
                    }
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
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
