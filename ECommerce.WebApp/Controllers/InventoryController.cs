using ECommerce.Application.Constants;
using ECommerce.Application.BaseServices.Brand;
using ECommerce.Application.BaseServices.Product;
using ECommerce.Application.BaseServices.Product.Dtos;
using ECommerce.Application.BaseServices.Shop;
using ECommerce.Application.Services.Comment;
using ECommerce.Application.Services.Inventory;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Threading.Tasks;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Application.Services.Inventory.Dtos;

namespace ECommerce.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private IProductBaseService _productBaseService;
        private IBrandService _brandService;
        private IShopService _shopService;
        private ICommentService _commentService;
        private IInventoryService _inventoryService;
        private ManageFiles _manageFiles;
        private string FILE_PATH = FilePathConstant.PRODUCT_FILEPATH;
        private string FILE_PREFIX = FilePathConstant.PRODUCT_FILEPREFIX;
        public InventoryController(
            IProductBaseService productBaseService,
            IBrandService brandService,
            IShopService shopService,
            IWebHostEnvironment webHostEnvironment,
            IInventoryService inventoryService,
            ICommentService commentService
        ) {
            _productBaseService = productBaseService;
            _brandService = brandService;
            _shopService = shopService;
            _inventoryService = inventoryService;
            _commentService = commentService;
            _manageFiles = new ManageFiles(webHostEnvironment);
        }
        [AllowAnonymous]
        [HttpPost("sub-categories")]
        public async Task<IActionResult> getSubCategories(InventoryRequest request)
        {
            var res = await _inventoryService.getSubCategories(request);
            if (!res.isSucceed) 
                return BadRequest(res);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpPost("get-brands")]
        public async Task<IActionResult> getBrands(BrandGetRequest request)
        {
            var res = await _inventoryService.getBrands(request);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpGet("get-brand/{id}")]
        public async Task<IActionResult> getBrand(int id)
        {
            var res = await _inventoryService.getBrand(id);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpPost("product-options")]
        public async Task<IActionResult> getProductOptions(InventoryRequest request)
        {
            var result = await _inventoryService.getProductOptions(request);
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("product-attributes")]
        public async Task<IActionResult> getProductAttributes(InventoryRequest request)
        {
            var result = await _inventoryService.getProductAttributes(request);
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("product-option")]
        public async Task<IActionResult> getProductOption(int productId = -1)
        {
            var result = await _inventoryService.getProductOption(productId);
            if (!result.isSucceed)
                return BadRequest(result.Message);
            return Ok(result);
        }
    }
}
