using ECommerce.Application.Common;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User_v2;
using ECommerce.WebApp.Configs.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Linq;
using ECommerce.WebApp.Utils;

namespace ECommerce.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserService_v2 _userServiceV2;
        private readonly ILogger<UserController> _logger;
        private HttpContextHelper _contextHelper;
        private readonly AppSetting _appSetting;
        public UserController(
            ILogger<UserController> logger,
            IOptionsMonitor<AppSetting> optionsMonitor,
            IUserService userService,
            IUserService_v2 userServiceV2)
        {
            _logger = logger;
            _contextHelper = new HttpContextHelper();
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

            return Ok(new SuccessResponse<string>
            {
                Status = "success",
                isSucceed = result.isSucceed,
                Message = result.Message,
                Data = GenerateToken(result.Data)
            });
        }
        [HttpPost("info")]
        public IActionResult UserInfo()
        {
            var token = _contextHelper.getAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var userClaims = this.DecodeToken(token).Claims;
            var user = new UserGetModel()
            {
                UserFullName = userClaims.FirstOrDefault(claim => claim.Type == "fullName").Value,
                UserPhone = userClaims.FirstOrDefault(claim => claim.Type == "phone").Value
            };
            return Ok(new SuccessResponse<UserGetModel>("success", user));
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
        private ClaimsPrincipal DecodeToken(string token)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var jwtTokenHandler = new JwtSecurityTokenHandler()
                .ValidateToken(token, new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken secToken);
            return jwtTokenHandler;
        }
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp()
        {
            return Ok();
        }
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword()
        {
            return Ok();
        }
    }
}
