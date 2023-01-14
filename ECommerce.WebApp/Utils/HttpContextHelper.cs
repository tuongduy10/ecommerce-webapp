using ECommerce.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Permissions;

namespace ECommerce.WebApp.Utils
{
    public class HttpContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextHelper()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }
        public int GetCurrentUserId()
        {
            var id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId") != null ?
                Int32.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId").Value) : 0;
            return id;
        }
        public System.Security.Claims.ClaimsPrincipal GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext.User;
        }
    }
}
