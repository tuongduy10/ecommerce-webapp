using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
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
        public string GetCurrentUserPhone()
        {
            var phone = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserPhone") != null ?
                _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserPhone").Value : "";
            return phone;
        }
        public string getAccessToken()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return "";
            }
            var token = authorizationHeader.Substring("Bearer ".Length);
            return token;
        }
        public System.Security.Claims.ClaimsPrincipal GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext.User;
        }
    }
}
