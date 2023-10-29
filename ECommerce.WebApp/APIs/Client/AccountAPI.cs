using ECommerce.Application.Common;
using ECommerce.Application.BaseServices.Shop;
using ECommerce.Application.BaseServices.Shop.Dtos;
using ECommerce.Application.BaseServices.User;
using ECommerce.Application.BaseServices.User.Dtos;
using ECommerce.Application.Services.User;
using ECommerce.Data.Models;
using ECommerce.WebApp.Hubs;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private const string COOKIE_CLIENT_SCHEMA = "ClientAuth";
        private IUserBaseService _userService;
        private IUserService _userServiceV2;
        private IShopService _shopService;
        private IHubContext<CommonHub> _commonHub;
        private IHubContext<ClientHub> _clientHub;
        private HttpContextHelper _contextHelper;
        public AccountAPI(
            IUserBaseService userService, 
            IUserService userServiceV2,
            IShopService shopService,
            IHubContext<CommonHub> commonHub,
            IHubContext<ClientHub> clientHub)
        {
            _userService = userService;
            _userServiceV2 = userServiceV2;
            _shopService = shopService;
            _commonHub = commonHub;
            _clientHub = clientHub;
            _contextHelper = new HttpContextHelper();
        }
        private async Task SignInHttpContext(ApiResponse result, string scheme)
        {
            // User Data
            var user = result.ObjectData.GetType();
            var username = user.GetProperty("UserFullName").GetValue(result.ObjectData, null).ToString();
            var userid = user.GetProperty("UserId").GetValue(result.ObjectData, null).ToString();
            var userroles = user.GetProperty("UserRoles").GetValue(result.ObjectData, null) as List<string>;
            var userPhone = user.GetProperty("UserPhone").GetValue(result.ObjectData, null).ToString();

            var claims = new List<Claim>
            {
                new Claim("TokenId", Guid.NewGuid().ToString()),
                new Claim("UserId", userid),
                new Claim("UserPhone", userPhone),
                new Claim(ClaimTypes.Name, username),
            };
            foreach (var item in userroles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(scheme, principal, props).Wait();

            // set online status, send to client
            var userIdFromContext = _contextHelper.GetCurrentUserId();
            if (userIdFromContext != 0)
            {
                var status = await _userServiceV2.UpdateOnlineStatus(userIdFromContext, true);
                await _clientHub.Clients.All.SendAsync("onChangeStatus", status);
            }
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
            
            await SignInHttpContext(result, COOKIE_CLIENT_SCHEMA);
            return Ok(result);
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
        [AllowAnonymous]
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

            await SignInHttpContext(result, COOKIE_CLIENT_SCHEMA);

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
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] UpdatePasswordRequest request)
        {
            var result = await _userService.ResetPassword(request);
            if (!result.isSucceed) return BadRequest(result);

            return Ok(result);
        }
        [HttpPost("SaleRegistration")]
        public async Task<IActionResult> SaleRegistration([FromBody] SaleRegistrationRequest request)
        {
            request.UserId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            var result = await _shopService.SaleRegistration(request);
            if (!result.isSucceed) {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
