using ECommerce.Application.Common;
using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Application.Services.User_v2;
using ECommerce.WebApp.Configs.AppSettings;
using ECommerce.WebApp.Controllers.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserService_v2 _userServiceV2;
        private readonly ILogger<UserController> _logger;
        private readonly AppSetting _appSetting;
        public UserController(
            ILogger<UserController> logger,
            IOptionsMonitor<AppSetting> optionsMonitor,
            IUserService userService,
            IUserService_v2 userServiceV2)
        {
            _logger = logger;
            _userService = userService;
            _userServiceV2 = userServiceV2;
            _appSetting = optionsMonitor.CurrentValue;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(SignInRequest request)
        {
            var result = await _userServiceV2.ValidateUser(request);
            if (!result.isSucceed)
            {
                return BadRequest(result);
            }

            return Ok(new SuccessResponse<string> { 
                Status = "success",
                isSucceed = result.isSucceed,
                Message = result.Message,
                Data = GenerateToken(result.Data)
            });
        }

        private string GenerateToken(UserGetModel user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var description = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.UserId.ToString()),
                    new Claim("tokenId", Guid.NewGuid().ToString()),
                    new Claim("fullName", user.UserFullName),
                    new Claim("phone", user.UserPhone),
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var createToken = jwtTokenHandler.CreateToken(description);
            var writeToken = jwtTokenHandler.WriteToken(createToken);
            
            return writeToken;
        }

    }
}
