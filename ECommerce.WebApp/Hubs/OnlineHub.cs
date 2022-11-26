using ECommerce.Application.Services.User.Dtos;
using ECommerce.Application.Services.User_v2;
using ECommerce.Data.Models;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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
        private ILogger<OnlineHub> _logger;
        public OnlineHub(IUserService_v2 userServiceV2, ILogger<OnlineHub> logger)
        {
            _contextHelper = new HttpContextHelper();
            _userSerivceV2 = userServiceV2;
            _logger = logger;
        }
        // Online
        public override async Task OnConnectedAsync()
        {
            var userId = _contextHelper.GetCurrentUserId();
            if (userId != 0)
                await _userSerivceV2.SetOnline(userId);

            await base.OnConnectedAsync();
        }
        // Offline
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = _contextHelper.GetCurrentUserId();
            if (userId != 0)
                await UpdateOnlineStatus(userId, false);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task UpdateOnlineStatus(int userId, bool status)
        {
            var result = await _userSerivceV2.UpdateOnlineStatus(userId, status);
            await Clients.All.SendAsync("ReceiveOnlineUsers", result);
        }
        public async Task SendOnlineUser()
        {
            var userId = _contextHelper.GetCurrentUserId();
            var result = await _userSerivceV2.GetUser(userId);
            await Clients.All.SendAsync("ReceiveOnlineUsers", result);
        }
    }
}
