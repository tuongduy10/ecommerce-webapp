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
    [AllowAnonymous]
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService) 
        {
            _inventoryService = inventoryService;
        }
        [HttpPost("sub-categories")]
        public async Task<IActionResult> getSubCategories(InventoryRequest request)
        {
            var res = await _inventoryService.getSubCategories(request);
            if (!res.isSucceed) 
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("get-brands")]
        public async Task<IActionResult> getBrands(BrandGetRequest request)
        {
            var res = await _inventoryService.getBrands(request);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("get-categories")]
        public async Task<IActionResult> getCategories()
        {
            var res = await _inventoryService.getCategories();
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("get-category/{id}")]
        public async Task<IActionResult> getCategories(int id)
        {
            var res = await _inventoryService.getCategory(id);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("update-category")]
        public async Task<IActionResult> updateCategory(CategoryModelRequest req)
        {
            var res = await _inventoryService.updateCategory(req);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> addCategory(CategoryModelRequest req)
        {
            var res = await _inventoryService.addCategory(req);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("get-brand/{id}")]
        public async Task<IActionResult> getBrand(int id)
        {
            var res = await _inventoryService.getBrand(id);
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("product-options")]
        public async Task<IActionResult> getProductOptions(InventoryRequest request)
        {
            var result = await _inventoryService.getProductOptions(request);
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("product-attributes")]
        public async Task<IActionResult> getProductAttributes(InventoryRequest request)
        {
            var result = await _inventoryService.getProductAttributes(request);
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
