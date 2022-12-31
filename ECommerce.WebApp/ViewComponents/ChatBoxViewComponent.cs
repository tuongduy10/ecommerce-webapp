using ECommerce.Application.Services.Configurations;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.ViewComponents
{
    public class ChatBoxViewComponent : ViewComponent
    {
        private HttpContextHelper _contextHelper;
        public ChatBoxViewComponent()
        {
            if(_contextHelper == null)
                _contextHelper = new HttpContextHelper();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.UserId = _contextHelper.GetCurrentUserId();

            return View();
        }
    }
}
