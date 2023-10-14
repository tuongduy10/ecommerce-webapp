using ECommerce.Application.Common;
using ECommerce.Application.Constants;
using ECommerce.Application.Services.Rate;
using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.WebApp.Models.Admin;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    //[Authorize(AuthenticationSchemes = "AdminAuth")]
    //[Authorize(Policy = "Seller")]
    public class AdminController : Controller
    {
        private const string _cookieAdminScheme = "AdminAuth";
        private string RATE_FILE_PATH = FilePathConstant.RATE_FILEPATH;
        private string RATE_FILE_PREFIX = FilePathConstant.RATE_FILEPREFIX;

        private IUserService _userService;
        private IRateService _rateService;
        private ManageFiles _manageFiles;
        public AdminController(
            IUserService userService, 
            IRateService rateService,
            IWebHostEnvironment webHostEnvironment
        ) {
            _userService = userService;
            _rateService = rateService;
            _manageFiles = new ManageFiles(webHostEnvironment);
        }
        public async Task<IActionResult> Index()
        {
            var comments = await _rateService.GetAll();
            var commentsToDay = await _rateService.GetAllToDay();

            var model = new AdminIndexViewModel
            {
                comments = commentsToDay
            };

            ViewBag.CommentsToDay = commentsToDay.Count();

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet()]
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
            SignInHttpContext(result, _cookieAdminScheme);
            return RedirectToAction("Index", "Admin");
        }
        private void SignInHttpContext(ApiResponse result, string scheme)
        {
            // User Data
            var user = result.ObjectData.GetType();
            var username = user.GetProperty("UserFullName").GetValue(result.ObjectData, null).ToString();
            var userid = user.GetProperty("UserId").GetValue(result.ObjectData, null).ToString();
            var userroles = user.GetProperty("UserRoles").GetValue(result.ObjectData, null) as List<string>;
            var userPhone = user.GetProperty("UserPhone").GetValue(result.ObjectData, null).ToString();

            var claims = new List<Claim>
            {
                new Claim("TokenId", Guid.NewGuid().ToString()),
                new Claim("UserId", userid),
                new Claim("UserPhone", userPhone),
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
        [AllowAnonymous]
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
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _rateService.DeleteComment(id);
            if (result.isSucceed) 
            {
                _manageFiles.DeleteFiles(result.Data, RATE_FILE_PATH);
                return Ok(result.Message);
            }
            
            return BadRequest(result.Message);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }
    }
}
