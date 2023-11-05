using ECommerce.Application.Constants;
using ECommerce.Application.Services.Comment;
using ECommerce.Application.Services.Comment.Request;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;
        private ManageFiles _manageFiles;
        private string RATING_FILE_PATH = FilePathConstant.RATE_FILEPATH;
        private string PRODUCT_FILE_PATH = FilePathConstant.PRODUCT_FILEPATH;
        private string PRODUCT_FILEPREFIX = FilePathConstant.PRODUCT_FILEPREFIX;
        public ProductController(
            IProductService productService,
            ICommentService commentService,
            IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _commentService = commentService;
            _manageFiles = new ManageFiles(webHostEnvironment);
        }
        [AllowAnonymous]
        [HttpPost("product-managed-list")]
        public async Task<IActionResult> getProductManagedList(ProductGetRequest request)
        {
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("product-list")]
        public async Task<IActionResult> getProductList(ProductGetRequest request)
        {
            var res = await _productService.getProductList(request);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("managed-products")]
        public async Task<IActionResult> getManagedProductList(ProductGetRequest request)
        {
            var res = await _productService.getManagedProductList(request);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpGet("product-detail/{id}")]
        public async Task<IActionResult> getProductDetail(int id)
        {
            var result = await _productService.getProductDetail(id);
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("product-review")]
        public async Task<IActionResult> getProductReview(RateGetRequest request)
        {
            var result = await _commentService.getRates(request);
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("rate-product")]
        public async Task<IActionResult> rateProduct()
        {
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("save")]
        public async Task<IActionResult> save(ProductSaveRequest request)
        {
            // Result 
            var result = await _productService.save(request);
            if (result.isSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> delete(ProductDeleteRequest request)
        {
            var result = await _productService.delete(request);
            if (result.isSucceed)
            {
                _manageFiles.DeleteFiles(result.Data.systemImages, PRODUCT_FILE_PATH);
                _manageFiles.DeleteFiles(result.Data.userImages, PRODUCT_FILE_PATH);
                _manageFiles.DeleteFiles(result.Data.ratingImages, RATING_FILE_PATH);
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
