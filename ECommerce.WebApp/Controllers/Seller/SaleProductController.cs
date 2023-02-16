using ECommerce.Application.Constants;
using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Comment;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.Shop;
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
        private IShopService _shopService;
        private ICommentService _commentService;
        private ManageFiles _manageFiles;
        private string RATING_FILE_PATH = FilePathConstant.RATE_FILEPATH;
        private string FILE_PATH = FilePathConstant.PRODUCT_FILEPATH;
        private string FILE_PREFIX = FilePathConstant.PRODUCT_FILEPREFIX;
        public SaleProductController(
            IProductService productService,
            IShopService shopService,
            ISubCategoryService subCategoryService, 
            IBrandService brandService,
            ICommentService commentService,
            IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
            _commentService = commentService;
            _manageFiles = new ManageFiles(webHostEnvironment);
            _shopService = shopService;
        }
        public IActionResult Index()
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
        public async Task<IActionResult> AddProduct(ProductSaveRequest request)
        {
            // Image null check
            if (request.systemImage == null || request.userImage == null)
            {
                return BadRequest("Vui lòng chọn ảnh");
            }

            // Get files name
            request.systemFileName = _manageFiles.GetFilesName(request.systemImage, FILE_PREFIX);
            request.userFileName = _manageFiles.GetFilesName(request.userImage, FILE_PREFIX);
            request.userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            // Result 
            var result = await _productService.AddProduct(request);
            if (result.isSucceed)
            {
                // Add file with files, files'name, path
                _manageFiles.AddFiles(request.systemImage, request.systemFileName, FILE_PATH);
                _manageFiles.AddFiles(request.userImage, request.userFileName, FILE_PATH);
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> ProductDetail(int id)
        {
            var userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            var brands = await _brandService.getAllBrandInShop(userId);
            return View(brands);
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
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleteCommentRes = await _commentService.DeleteByProductId(id);
            if (deleteCommentRes.isSucceed)
            {
                _manageFiles.DeleteFiles(deleteCommentRes.Data, RATING_FILE_PATH);
            }

            var result = await _productService.DeleteProduct(id);
            if (result.isSucceed)
            {
                _manageFiles.DeleteFiles(result.Data.systemImages, FILE_PATH);
                _manageFiles.DeleteFiles(result.Data.userImages, FILE_PATH);
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProducts(List<int> ids)
        {
            var result = await _productService.DeleteProducts(ids);
            if (result.isSucceed)
            {
                _manageFiles.DeleteFiles(result.Data.systemImages, FILE_PATH);
                _manageFiles.DeleteFiles(result.Data.userImages, FILE_PATH);
                _manageFiles.DeleteFiles(result.Data.ratingImages, RATING_FILE_PATH);
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
