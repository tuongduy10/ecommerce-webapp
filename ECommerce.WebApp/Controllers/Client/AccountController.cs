using ECommerce.Application.Services.Shop;
using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Client
{
    [Authorize(AuthenticationSchemes = "ClientAuth")]
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IShopService _shopService;
        public AccountController(IUserService userService, IShopService shopService)
        {
            _userService = userService;
            _shopService = shopService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignIn(string CurrentUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = CurrentUrl;
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("ClientAuth");
            return RedirectToAction("SignIn", "Account");
        }
        public async Task<IActionResult> UserProfile()
        {
            var _id = User.Claims.FirstOrDefault(i => i.Type == "UserId") != null ? 
                Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value) : 0;

            var user = await _userService.UserProfile(_id);
            if (user == null || user.Status == false) return RedirectToAction("SignOut", "Account");

            return View(user);
        }
        public async Task<IActionResult> UpdateUserPhoneNumber()
        {
            return View();
        }
        public async Task<IActionResult> UpdateUserPassword()
        {
            return View();
        }
        public async Task<IActionResult> SaleRegistration()
        {
            var id = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            var result = await _shopService.isRegisted(id);
            var model = new SaleRegistrationModel();

            if (!result.isSucceed || result.ObjectData == null)
            {
                model.isRegisted = false;
                return View(model);
            }

            var shop = result.ObjectData.GetType();
            var status = Int32.Parse(shop.GetProperty("Status").GetValue(result.ObjectData, null).ToString());
            model.isRegisted = true;
            model.status = status;

            return View(model);
        }
        public async Task<IActionResult> DeleteShop()
        {
            var id = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            var result = await _shopService.deleteShop(id);
            if (result.isSucceed)
            {
                return RedirectToAction("SaleRegistration", "Account");
            }
            ViewBag.Message = result.Message;
            return RedirectToAction("SaleRegistration", "Account");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
    public class SaleRegistrationModel
    {
        public bool isRegisted { get; set; }
        public int status { get; set; }
    }
}
