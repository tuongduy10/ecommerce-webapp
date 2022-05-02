using ECommerce.Application.Services.Account;
using ECommerce.Application.Services.Account.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

namespace ECommerce.WebApp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountAPI : ControllerBase
    {
        private IUserService _userService;
        private IConfiguration _config;
        public AccountAPI(IConfiguration config, IUserService userService)
        {
            _config = config;
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

            // Store data to cookie
            var claims = new List<Claim>
            {
                new Claim("TokenId", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Sid, userid),
                new Claim(ClaimTypes.Name, username),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

            return Ok(result);
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var result = await _userService.SignUp(request);
            if (!result.isSucceed)
            {
                return BadRequest(new { status = "error", message = result.Message});
            }
            return Ok(new { status = "success", message = "Tạo tài khoản thành công" });
        }
        [HttpPost("CheckUserPhoneNumber")]
        public async Task<IActionResult> CheckUserPhoneNumber([FromBody] string PhoneNumber)
        {
            var result = await _userService.checkUserPhoneNumber(PhoneNumber);
            if (result == false) return BadRequest(new { status = "error", message = "Số điện thoại đã tồn tại" });
            
            return Ok(new { status = "success", message = "Có thể tại tài khoản với số điện thoại này" });
        }
    }
}
