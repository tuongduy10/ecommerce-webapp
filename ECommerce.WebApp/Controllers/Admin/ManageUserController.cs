using ECommerce.Application.Services.Role;
using ECommerce.Application.Services.Shop;
using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Application.Services.User.Enums;
using ECommerce.WebApp.Models.ManageUser;
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
        private IShopService _shopService;
        public ManageUserController(
            IUserService userService,
            IShopService shopService
        ) {
            _userService = userService;
            _shopService = shopService;
        }
        public async Task<IActionResult> Index(UserGetRequest request)
        {
            var list = await _userService.getUsersByFiltered(request);
            return View(list);
        }
        public async Task<IActionResult> UnConfirmUser()
        {
            var list = await _userService.getAll();
            return View(list);
        }
        [HttpGet]
        public IActionResult AddSeller()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSeller(SignUpRequest request)
        {
            request.isSystemAccount = true;
            var result = await _userService.AddSeller(request);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UserProfile(int id)
        {
            var user = await _userService.UserProfile(id);
            var shop = await _shopService.getShopList();
            var model = new ManageUserProfileViewModel
            {
                User = user,
                Shops = shop
            };
            return View(model);
        }
        public async Task<IActionResult> UpdateUser(UserUpdateRequest request)
        {
            var result = await _userService.UpdateManageUserProfile(request);
            if (result.isSucceed)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
