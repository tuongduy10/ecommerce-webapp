using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.BaseServices.User;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApp.Middlewares
{
    public class CustomAuthorizationMiddleware
    {
        public readonly RequestDelegate _next;
        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IUserBaseService userService)
        {
            var _cookies = context.Request.Cookies; // _clientcookie or _admincookie
            var _authType = context.User.Identity.AuthenticationType; // ClientAuth or AdminAuth
            var _path = context.Request.Path.ToString(); // ex: /Home/Index
            var _userId = context.User.Claims.FirstOrDefault(i => i.Type == "UserId") != null ?
                Int32.Parse(context.User.Claims.FirstOrDefault(i => i.Type == "UserId").Value) : 0;

            //if (_userId != 0)
            //{
            //    var _user = await userService.UserProfile(_userId);
            //    if (_user == null || _user.Status == false)
            //    {
            //        context.SignOutAsync(_authType).Wait();
            //        context.Response.Redirect("/Account/SignIn");
            //    }
            //}

            await _next(context);
        }
    }
}
