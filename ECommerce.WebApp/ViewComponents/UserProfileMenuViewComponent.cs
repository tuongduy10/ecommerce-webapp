using ECommerce.Application.Services.User;
using ECommerce.WebApp.Controllers.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.WebApp.ViewComponents
{
    public class UserProfileMenuViewComponent : ViewComponent
    {
        private IUserService _userService;
        public UserProfileMenuViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = User as ClaimsPrincipal;
            var id = Int32.Parse(user.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            var result = await _userService.isRegisted(id);
            var model = new SaleRegistrationModel();

            // Chưa đăng ký
            if (!result.isSucceed || result.ObjectData == null)
            {
                model.isRegisted = false;
                return View(model);
            }

            var shop = result.ObjectData.GetType();
            var status = Int32.Parse(shop.GetProperty("Status").GetValue(result.ObjectData, null).ToString());
            model.isRegisted = true;
            model.status = status;

            return View(model);
        }
    }
}
