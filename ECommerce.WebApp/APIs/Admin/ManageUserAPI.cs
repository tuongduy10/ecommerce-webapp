using ECommerce.Application.BaseServices.User;
using ECommerce.Application.BaseServices.User.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageUserAPI : ControllerBase
    {
        private IUserBaseService _userService;
        public ManageUserAPI(IUserBaseService userService)
        {
            _userService = userService;
        }

        [HttpPost("UpdateUserStatus")]
        public async Task<IActionResult> UpdateUserStatus([FromBody] UserUpdateRequest request)
        {
            var UserId = User.Claims.FirstOrDefault(i => i.Type == "UserId").Value;
            if (Int32.Parse(UserId) == request.UserId)
            {
                return BadRequest(new { isSucceed = false, Message = "Không thể cập nhật cho chính tài khoản của bạn !" });
            }
            var result = await _userService.UpdateUserStatus(request);
            if (result.isSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
