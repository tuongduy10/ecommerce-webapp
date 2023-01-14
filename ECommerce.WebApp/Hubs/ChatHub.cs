using ECommerce.Application.Constants;
using ECommerce.Application.Services.Chat;
using ECommerce.Application.Services.Chat.Dtos;
using ECommerce.Data.Models;
using ECommerce.WebApp.Hubs.Dtos;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Hubs
{
    public class ChatHub : Hub
    {
        private IChatService _chatService;
        private HttpContextHelper _contextHelper;
        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
            _contextHelper = new HttpContextHelper();
        }
        public async Task SendToAdmin(string userId, string message)
        {
            var msgModel = new MessageModel();
            msgModel.FromUserId = Int32.Parse(userId);
            msgModel.Message = message;
            msgModel.Type = TypeConstant.MSG_FROM_CLIENT;

            var sendMsgRes = await _chatService.SendMessage(msgModel);

            await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }
        public async Task SendToAdminNoService(MessageHubModel request)
        {
            var userId = request.FromUserId;
            var message = request.Message;
            await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }
        public async Task SendToClient(string fromUserId, string toUserId, string userName, string message)
        {
            var msgModel = new MessageModel();
            msgModel.FromUserId = !String.IsNullOrEmpty(fromUserId) ? Int32.Parse(fromUserId) : 0;
            msgModel.ToUserId = !String.IsNullOrEmpty(toUserId) ? Int32.Parse(toUserId) : 0;
            msgModel.Type = TypeConstant.MSG_FROM_ADMIN;
            msgModel.Message = message;

            var sendMsgRes = await _chatService.SendMessage(msgModel);

            await Clients.All.SendAsync("ReceiveFromAdmin", userName, message);
        }
    }
}
