using ECommerce.Application.Services.Role;
using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Application.Services.User.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageUserController : Controller
    {
        private IUserService _userService;
        public ManageUserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _userService.getAll();
            return View(list);
        }
        public async Task<IActionResult> UnConfirmUser()
        {
            var list = await _userService.getAll();
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> AddSeller()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSeller(SignUpRequest request)
        {
            request.RoleId = (int)enumRole.Seller;
            var result = await _userService.SignUp(request);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
