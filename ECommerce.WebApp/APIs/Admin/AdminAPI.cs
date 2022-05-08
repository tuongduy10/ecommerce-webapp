using ECommerce.Application.Services.Account;
using ECommerce.Application.Services.Account.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAPI : ControllerBase
    {
        private IUserService _userService;
        public AdminAPI(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            var result = await _userService.SignIn(request);
            if (!result.isSucceed)
            {
                return BadRequest(result);
            }
            // User Data
            var user = result.ObjectData.GetType();
            var username = user.GetProperty("UserFullName").GetValue(result.ObjectData, null).ToString();
            var userid = user.GetProperty("UserId").GetValue(result.ObjectData, null).ToString();
            var userroles = user.GetProperty("UserRoles").GetValue(result.ObjectData, null) as List<string>;
            if (!userroles.Contains("Admin"))
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

            return Ok(result);
        }
        private void claimUserIdentity(List<Claim> claims, string scheme)
        {
            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(scheme, principal, props).Wait();
        }
    }
}
