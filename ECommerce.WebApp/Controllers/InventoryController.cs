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
        [HttpGet("get-sub-categories")]
        public async Task<IActionResult> getSubCategories(int brandId = 0)
        {
            var res = await _inventoryService.getSubCategories(brandId);
            if (!res.isSucceed) 
                return BadRequest(res);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpGet("get-brand")]
        public async Task<IActionResult> getBrand(int brandId = 0)
        {
            var res = await _inventoryService.getBrand(brandId);

            if (!res.isSucceed)
                return BadRequest(res);
            return Ok();
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
