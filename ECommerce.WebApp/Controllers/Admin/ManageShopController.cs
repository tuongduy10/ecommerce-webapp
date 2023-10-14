using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Shop;
using ECommerce.Application.Services.Shop.Dtos;
using ECommerce.Application.Services.User;
using ECommerce.WebApp.Models.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    //[Authorize(AuthenticationSchemes = "AdminAuth")]
    //[Authorize(Roles = "Admin")]
    public class ManageShopController : Controller
    {
        private IShopService _shopService;
        private IBrandService _brandService;
        private IUserService _userService;
        public ManageShopController(IShopService shopService, IBrandService brandService, IUserService userService)
        {
            _shopService = shopService;
            _brandService = brandService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _shopService.getUnconfirmedShop();
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> AddShop()
        {
            var brands = await _brandService.getAllManage();
            return View(brands);
        }
        [HttpPost]
        public async Task<IActionResult> AddShop(ShopAddRequest request)
        {
            var result = await _shopService.AddShop(request);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> ShopList()
        {
            var list = await _shopService.getShopList();
            return View(list);
        }
        public async Task<IActionResult> ShopDetail(int ShopId)
        {
            var shop = await _shopService.getShopDetailManage(ShopId);
            var user = await _userService.getUserByShop(ShopId);
            var brands = await _brandService.getAll();
            var model = new ManageShopDetailViewModel
            {
                Shop = shop,
                User = user,
                Brands = brands
            };
            return View(model);
        }
        public async Task<IActionResult> UpdateShopStatus(int id, byte status)
        {
            var result = await _shopService.updateShopStatus(id, status);
            return RedirectToAction("ShopDetail", new { ShopId = id });
        }
        public async Task<IActionResult> UpdateShop(ShopUpdateRequest request)
        {
            var result = await _shopService.updateShop(request);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
