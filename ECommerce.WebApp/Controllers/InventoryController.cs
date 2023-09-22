using ECommerce.Application.Constants;
using ECommerce.Application.BaseServices.Brand;
using ECommerce.Application.Services.Comment;
using ECommerce.Application.BaseServices.Product;
using ECommerce.Application.BaseServices.Product.Dtos;
using ECommerce.Application.BaseServices.Shop;
using ECommerce.Application.BaseServices.SubCategory;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private IProductBaseService _productService;
        private IBrandService _brandService;
        private IShopService _shopService;
        private ISubCategoryService _subCategoryService;
        private ICommentService _commentService;
        private ManageFiles _manageFiles;
        private string FILE_PATH = FilePathConstant.PRODUCT_FILEPATH;
        private string FILE_PREFIX = FilePathConstant.PRODUCT_FILEPREFIX;
        public InventoryController(
            IProductBaseService productService,
            IBrandService brandService,
            IShopService shopService,
            IWebHostEnvironment webHostEnvironment,
            ISubCategoryService subCategoryService,
            ICommentService commentService
        ) {
            _productService = productService;
            _brandService = brandService;
            _shopService = shopService;
            _subCategoryService = subCategoryService;
            _commentService = commentService;
            _manageFiles = new ManageFiles(webHostEnvironment);
        }
        [HttpPost("product-list")]
        public async Task<IActionResult> ProductList()
        {
            return Ok();
        }
        [HttpPost("product-detail")]
        public async Task<IActionResult> ProductDetail(int id = 0)
        {
            return Ok();
        }
        [HttpPost("save")]
        public async Task<IActionResult> Save(ProductSaveRequest request)
        {
            // Image null check
            if (request.systemImage == null)
            {
                return BadRequest("Vui lòng chọn ảnh");
            }

            request.userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            // Get files name
            if (request.systemImage != null)
                request.systemFileName = _manageFiles.GetFilesName(request.systemImage, FILE_PREFIX);
            if (request.userImage != null)
                request.userFileName = _manageFiles.GetFilesName(request.userImage, FILE_PREFIX);
            // Result 
            var result = await _productService.AddProduct(request);
            if (result.isSucceed)
            {
                // Add file with files, files'name, path
                if (request.systemImage != null && request.systemFileName != null)
                    _manageFiles.AddFiles(request.systemImage, request.systemFileName, FILE_PATH);
                if (request.userImage != null && request.userFileName != null)
                    _manageFiles.AddFiles(request.userImage, request.userFileName, FILE_PATH);

                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
