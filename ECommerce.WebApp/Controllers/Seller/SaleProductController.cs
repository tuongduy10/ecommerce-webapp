using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.SubCategory;
using ECommerce.WebApp.Models.SaleProduct;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Seller
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Seller")]
    public class SaleProductController : Controller
    {
        private IProductService _productService;
        private ISubCategoryService _subCategoryService;
        private IBrandService _brandService;
        private IWebHostEnvironment _webHostEnvironment;
        public SaleProductController(IProductService productService, ISubCategoryService subCategoryService, IBrandService brandService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> ProductList(int subCategoryId = 0)
        {
            var userId = Int32.Parse(User.Claims.Where(i => i.Type == "UserId").Select(i => i.Value).FirstOrDefault());
            var products = await _productService.getProductByUser(userId, subCategoryId);
            var subcategories = await _subCategoryService.getSubCategoryByUser(userId);
            var model = new ProductListViewModel()
            {
                products = products,
                subCategories = subcategories,
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            var brands = await _brandService.getAllBrandInShop(userId);
            return View(brands);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductAddRequest request)
        {
            // Image null check
            if (request.systemImage == null || request.userImage == null)
            {
                return BadRequest("Vui lòng chọn ảnh");
            }

            // Get files name
            var listSysFileName = new List<string>();
            for (int i = 0; i < request.systemImage.Count; i++)
            {
                var file = request.systemImage[i];
                var extension = new FileInfo(file.FileName).Extension;
                var fileName = "product-" + Guid.NewGuid().ToString() + extension;
                listSysFileName.Add(fileName);
            }
            var listUserFileName = new List<string>();
            for (int i = 0; i < request.userImage.Count; i++)
            {
                var file = request.userImage[i];
                var extension = new FileInfo(file.FileName).Extension;
                var fileName = Guid.NewGuid().ToString() + extension;
                listUserFileName.Add(fileName);
            }

            request.systemFileName = listSysFileName;
            request.userFileName = listUserFileName;
            request.userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            // Result 
            var result = await _productService.AddProduct(request);
            if (result.isSucceed)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                for (int i = 0; i < request.systemImage.Count; i++)
                {
                    string filePath = Path.Combine(uploadsFolder, request.systemFileName[i]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        request.systemImage[i].CopyTo(fileStream);
                    }
                }
                for (int i = 0; i < request.userImage.Count; i++)
                {
                    string filePath = Path.Combine(uploadsFolder, request.userFileName[i]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        request.userImage[i].CopyTo(fileStream);
                    }
                }

                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> DisableProducts(List<int> ids)
        {
            var result = await _productService.DisableProducts(ids);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveProducts(List<int> ids)
        {
            var result = await _productService.ApproveProducts(ids);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
