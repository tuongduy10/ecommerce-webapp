using ECommerce.Application.Services.Account;
using ECommerce.Application.Services.Account.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Seller")]
    public class AdminController : Controller
    {
        private IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(string userphone, string password)
        {
            var result = await _userService.SignIn(new SignInRequest(userphone, password));
            if (!result.isSucceed)
            {
                ViewBag.error = result.Message;
                return View("SignIn");
            }

            // User Data
            var user = result.ObjectData.GetType();
            var username = user.GetProperty("UserFullName").GetValue(result.ObjectData, null).ToString();
            var userid = user.GetProperty("UserId").GetValue(result.ObjectData, null).ToString();
            var userroles = user.GetProperty("UserRoles").GetValue(result.ObjectData, null) as List<string>;
            if(userroles.FindAll(role => role.Contains("Admin") || role.Contains("Seller")).Count == 0)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Store data to cookie
            var claims = new List<Claim>
            {
                new Claim("TokenId", Guid.NewGuid().ToString()),
                new Claim("UserId", userid),
                new Claim(ClaimTypes.Name, username),
            };
            foreach (var item in userroles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            claimUserIdentity(claims, "AdminAuth");

            return RedirectToAction("Index", "Admin");
        }
        private void claimUserIdentity(List<Claim> claims, string scheme)
        {
            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(scheme, principal, props).Wait();
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("AdminAuth");
            return RedirectToAction("SignIn", "Admin");
        }
    }
}
