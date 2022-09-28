using ECommerce.Application.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECommerce.WebApp.Configs.ActionFilters
{
    public class GlobalActionFilter : ActionFilterAttribute
    {
        private IUserService _userService;
        public GlobalActionFilter(IUserService userService)
        {
            _userService = userService;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await GetUserProfile(context);
            await base.OnActionExecutionAsync(context, next);
        }
        private async Task GetUserProfile(ActionExecutingContext context)
        {
            var _userId = context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId") != null ?
                Int32.Parse(context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId").Value) : 0;
            var _cookies = context.HttpContext.Request.Cookies; // _clientcookie or _admincookie
            var _authType = context.HttpContext.User.Identity.AuthenticationType; // ClientAuth or AdminAuth
            var _path = context.HttpContext.Request.Path.ToString(); // ex: /Home/Index
            var _user = await _userService.UserProfile(_userId);

            if (_user == null || _user.Status == false)
            {
                if (_authType == "ClientAuth" && IsAllowedPath(_path)) 
                    context.Result = new RedirectToActionResult("SignOut", "Account", null);
                if (_authType == "AdminAuth" && IsAllowedPath(_path)) 
                    context.Result = new RedirectToActionResult("SignOut", "Admin", null);
            }
        }
        private bool IsAllowedPath(string path = null)
        {
            return !new List<string>() { 
                //"/", 
                //"/Home", 
                //"/Home/Index", 
                "/Account/SignOut",
                "/Admin/SignOut",
                "/Account/SignIn",
                "/Admin/SignIn"
            }.Contains(path);
        }
    }
}
