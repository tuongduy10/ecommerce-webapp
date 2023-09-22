using ECommerce.Application.Services.Notification;
using ECommerce.Application.BaseServices.Rate;
using ECommerce.Application.BaseServices.Shop;
using ECommerce.Application.BaseServices.User;
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
        private IShopService _shopService;
        private INotificationService _notificationService;
        public UserProfileMenuViewComponent(
            IShopService shopService,
            INotificationService notificationService
        ) {
            _shopService = shopService;
            _notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = User as ClaimsPrincipal;
            var id = Int32.Parse(user.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            var result = await _shopService.isRegisted(id);
            var model = new SaleRegistrationModel();

            var notifications = await _notificationService.Notification.ToListAsyncWhere(item => item.ReceiverId == id && item.IsRead == false);
            ViewBag.NotiCount = notifications.Count();

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
