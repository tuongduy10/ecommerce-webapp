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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> login(SignInRequest request)
        {
            var result = await _userService.ValidateUser(request);
            if (!result.isSucceed)
                return BadRequest(result);

            return Ok(new SuccessResponse<string>(generateToken(result.Data)));
        }
        [HttpPost("info")]
        public IActionResult info()
        {
            var token = _contextHelper.getAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var userClaims = this.decodeToken(token).Claims;
            var user = new UserModel()
            {
                id = Int32.Parse(userClaims.FirstOrDefault(claim => claim.Type == "id").Value),
                fullName = userClaims.FirstOrDefault(claim => claim.Type == "fullName").Value,
                phone = userClaims.FirstOrDefault(claim => claim.Type == "phone").Value,
                mail = userClaims.FirstOrDefault(claim => claim.Type == "mail").Value,
                addressInfo = {
                    address = userClaims.FirstOrDefault(claim => claim.Type == "address").Value,
                    wardCode = userClaims.FirstOrDefault(claim => claim.Type == "wardCode").Value,
                    districtCode = userClaims.FirstOrDefault(claim => claim.Type == "districtCode").Value,
                    cityCode = userClaims.FirstOrDefault(claim => claim.Type == "cityCode").Value,
                }
            };
            return Ok(new SuccessResponse<UserModel>("success", user));
        }
        [HttpPost("update-user")]
        public async Task<IActionResult> updateUser(UserSaveRequest request)
        {
            var result = await _userService.updateUser(request);
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
        
        // private functions
        private string generateToken(UserModel user)
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
                    new Claim("mail", user.mail),
                    new Claim("address", user.addressInfo.address),
                    new Claim("wardCode", user.addressInfo.wardCode),
                    new Claim("districtCode", user.addressInfo.districtCode),
                    new Claim("cityCode", user.addressInfo.cityCode),
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var createToken = jwtTokenHandler.CreateToken(description);
            var writeToken = jwtTokenHandler.WriteToken(createToken);

            return writeToken;
        }
        private ClaimsPrincipal decodeToken(string token)
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
