using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController() { }
        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
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
