using ECommerce.Application.Services.Configurations;
using ECommerce.Application.Services.Notification;
using ECommerce.Application.Services.Rate;
using ECommerce.WebApp.Models.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IHeaderService _headerService;
        private readonly IConfigurationService _configurationService;
        private readonly INotificationService _notificationService;
        public HeaderViewComponent(
            IConfigurationService configurationService, 
            INotificationService notificationService,
            IHeaderService headerService)
        {
            _notificationService = notificationService;
            _configurationService = configurationService;
            _headerService = headerService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var _listHeader = await _headerService.getAll();
            var _config = await _configurationService.getConfiguration();

            var model = new HeaderViewModel()
            {
                listHeader = _listHeader,
                config = _config
            };

            var userId = Request.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId") != null ?
                Int32.Parse(Request.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId").Value) : 0;
            var notifications = await _notificationService.Notification.ToListAsyncWhere(item => item.ReceiverId == userId && item.IsRead == false);
            ViewBag.NotiCount = notifications.Count();

            return View(model);
        }
    }
}
