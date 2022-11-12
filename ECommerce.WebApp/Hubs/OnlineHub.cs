using ECommerce.Application.Services.User_v2;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Hubs
{
    public class OnlineHub : Hub
    {
        private IUserService_v2 _userSerivceV2;
        private HttpContextHelper _contextHelper;
        public OnlineHub(IUserService_v2 userServiceV2)
        {
            _contextHelper = new HttpContextHelper();
            _userSerivceV2 = userServiceV2;
        }
        // Online
        public override async Task<Task> OnConnectedAsync()
        {
            var userId = _contextHelper.GetCurrentUserId();
            if (userId != 0)
                await _userSerivceV2.SetOnline(userId);

            return base.OnConnectedAsync();
        }
        // Offline
        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var userId = _contextHelper.GetCurrentUserId();
            if (userId != 0)
                await _userSerivceV2.SetOnline(userId, false);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
