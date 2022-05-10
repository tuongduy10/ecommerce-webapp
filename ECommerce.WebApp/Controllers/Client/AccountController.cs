using ECommerce.Application.Services.User;
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
        public AccountController(IUserService userService)
        {
            _userService = userService;
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
            var id = User.Claims.FirstOrDefault(i => i.Type == "UserId").Value;
            var user = await _userService.UserProfile(Int32.Parse(id));
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
}
