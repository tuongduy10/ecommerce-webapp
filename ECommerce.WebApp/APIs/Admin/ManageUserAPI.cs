using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageUserAPI : ControllerBase
    {
        private IUserService _userService;
        public ManageUserAPI(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UpdateUserStatus")]
        public async Task<IActionResult> UpdateUserStatus([FromBody] UserUpdateRequest request)
        {
            var result = await _userService.UpdateUserStatus(request);
            if (result.isSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
