using ECommerce.Application.Common;
using ECommerce.Application.BaseServices.User.Dtos;
using ECommerce.Application.BaseServices.User;
using ECommerce.Application.Services.User;
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
using ECommerce.Application.Services.User.Dtos;

namespace ECommerce.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private HttpContextHelper _contextHelper;
        private readonly AppSetting _appSetting;
        public UserController(
            ILogger<UserController> logger,
            IOptionsMonitor<AppSetting> optionsMonitor,
            IUserService userService)
        {
            _logger = logger;
            _contextHelper = new HttpContextHelper();
            _userService = userService;
            _appSetting = optionsMonitor.CurrentValue;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(SignInRequest request)
        {
            var result = await _userService.ValidateUser(request);
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
        [AllowAnonymous]
        [HttpPost("info")]
        public IActionResult UserInfo()
        {
            try
            {
                var token = _contextHelper.getAccessToken();
                if (string.IsNullOrEmpty(token))
                    return Unauthorized();

                var userClaims = this.DecodeToken(token).Claims;
                var user = new UserModel()
                {
                    id = Int32.Parse(userClaims.FirstOrDefault(claim => claim.Type == "id").Value),
                    fullName = userClaims.FirstOrDefault(claim => claim.Type == "fullName").Value,
                    phone = userClaims.FirstOrDefault(claim => claim.Type == "phone").Value
                };
                return Ok(new SuccessResponse<UserModel>("success", user));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        [HttpGet("shops")]
        public async Task<IActionResult> GetShops()
        {
            var res = await _userService.GetShops();
            if (!res.isSucceed)
                return BadRequest(res);
            return Ok(res);
        }

        private string GenerateToken(UserModel user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var description = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.id.ToString()),
                    new Claim("tokenId", Guid.NewGuid().ToString()),
                    new Claim("fullName", user.fullName),
                    new Claim("phone", user.phone),
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
    }
}
