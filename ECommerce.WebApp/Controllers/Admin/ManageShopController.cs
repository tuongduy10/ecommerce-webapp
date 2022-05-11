using ECommerce.Application.Services.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Roles = "Admin")]
    public class ManageShopController : Controller
    {
        private IShopService _shopService;
        public ManageShopController(IShopService shopService)
        {
            _shopService = shopService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _shopService.getUnconfirmedShop();
            return View(list);
        }

        public async Task<IActionResult> UpdateShopStatus(int id, byte status)
        {
            var result = await _shopService.updateShopStatus(id, status);
            if (result.isSucceed)
            {
                return RedirectToAction("Index", "ManageShop");
            }
            ViewBag.Message = result.Message;
            return View("Index");
        }
    }
}
