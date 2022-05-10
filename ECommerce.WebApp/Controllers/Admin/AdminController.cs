using ECommerce.Application.Common;
using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Seller")]
    public class AdminController : Controller
    {
        private const string _cookieAdminScheme = "AdminAuth";
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
            if (!isAdminOrSeller(result))
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            SignInHttpContext(result, _cookieAdminScheme);

            return RedirectToAction("Index", "Admin");
        }
        private bool isAdminOrSeller(ApiResponse result)
        {
            var userroles = result.ObjectData.GetType().GetProperty("UserRoles").GetValue(result.ObjectData, null) as List<string>;
            if (userroles.FindAll(role => role.Contains("Admin") || role.Contains("Seller")).Count == 0)
            {
                return false;
            }

            return true;
        }
        private void SignInHttpContext(ApiResponse result, string scheme)
        {
            // User Data
            var user = result.ObjectData.GetType();
            var username = user.GetProperty("UserFullName").GetValue(result.ObjectData, null).ToString();
            var userid = user.GetProperty("UserId").GetValue(result.ObjectData, null).ToString();
            var userroles = user.GetProperty("UserRoles").GetValue(result.ObjectData, null) as List<string>;

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

            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(scheme, principal, props).Wait();
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("AdminAuth");
            return View("SignIn");
        }
        public async Task<IActionResult> AdminProfile()
        {
            var name = User.Identity.Name;
            var id = User.Claims.FirstOrDefault(i => i.Type == "UserId").Value;
            var user = await _userService.UserProfile(Int32.Parse(id));
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }
    }
}
