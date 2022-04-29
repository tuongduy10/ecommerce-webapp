using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User.Dtos;
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
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            var jwtToken = result.ResultObj;

            // Store data to cookie
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Append("token", jwtToken, option);

            return Ok(result);
        }
    }
}
