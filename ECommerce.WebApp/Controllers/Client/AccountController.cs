using ECommerce.Application.Services.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Client
{
    public class AccountController : Controller
    {
        public async Task<IActionResult> SignIn(string CurrentUrl = "/")
        {
            ViewData["ReturnUrl"] = CurrentUrl;
            return View();
        }
        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("SignIn", "Account");
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var id = User.Claims.FirstOrDefault(i => i.Type == "UserId").Value;
            return View(); // chưa có view
        }
    }
}
