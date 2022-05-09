using ECommerce.Application.Services.Account;
using ECommerce.Application.Services.Account.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "ClientAuth")]
    public class AccountAPI : ControllerBase
    {
        private IUserService _userService;
        public AccountAPI(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
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
            //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var principal = new ClaimsPrincipal(identity);
            //var props = new AuthenticationProperties();
            //HttpContext.SignInAsync("ClientAuth", principal, props).Wait();
            //if (userroles.Contains("Admin"))
            //{
            //    HttpContext.SignInAsync("AdminAuth", principal, props).Wait();
            //}
            claimUserIdentity(claims, "ClientAuth");
            if (userroles.Contains("Admin"))
            {
                claimUserIdentity(claims, "AdminAuth");
            }
            return Ok(result);
        }
        private void claimUserIdentity(List<Claim> claims, string scheme)
        {
            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(scheme, principal, props).Wait();
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var result = await _userService.SignUp(request);
            if (!result.isSucceed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CheckUserPhoneNumber")]
        public async Task<IActionResult> CheckUserPhoneNumber([FromBody] string PhoneNumber)
        {
            var result = await _userService.CheckUserPhoneNumber(PhoneNumber);
            if (!result.isSucceed) return BadRequest(result);
            
            return Ok(result);
        }

        [HttpPost("UpdateUserPhoneNumber")]
        public async Task<IActionResult> UpdateUserPhoneNumber([FromBody] string PhoneNumber)
        {
            var id = User.Claims.FirstOrDefault(i => i.Type == "UserId").Value;
            var result = await _userService.UpdateUserPhoneNumber(Int32.Parse(id), PhoneNumber);
            if (!result.isSucceed) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserUpdateRequest request)
        {
            var id = User.Claims.FirstOrDefault(i => i.Type == "UserId").Value;
            request.UserId = Int32.Parse(id);
            var result = await _userService.UpdateUserProfile(request);
            if (!result.isSucceed) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("UpdateUserPassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdatePasswordRequest request)
        {
            var id = User.Claims.FirstOrDefault(i => i.Type == "UserId").Value;
            request.UserId = Int32.Parse(id);
            var result = await _userService.UpdateUserPassword(request);
            if (!result.isSucceed) return BadRequest(result);

            return Ok(result);
        }
    }
}
