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
        public async Task SendToAdmin(string userId, string phone, string message)
        {
            var msgModel = new MessageModel();
            msgModel.FromUserId = Int32.Parse(userId);
            msgModel.Message = message;
            msgModel.PhoneNumber = phone;
            msgModel.Type = TypeConstant.MSG_FROM_CLIENT;
            
            if (msgModel.FromUserId == 0)
            {
                msgModel.UserName = "temp";
                var sendMsgRes = await _chatService.SendUnAuthMessage(msgModel);
                var _message = new
                {
                    userId = sendMsgRes.Data.FromUserId,
                    userName = sendMsgRes.Data.UserName,
                    message = sendMsgRes.Data.Message,
                };
                await Clients.All.SendAsync("ReceiveMessage", _message);
            } 
            else
            {
                var sendMsgRes = await _chatService.SendMessage(msgModel);
                var _message = new
                {
                    userId = sendMsgRes.Data.FromUserId,
                    userName = sendMsgRes.Data.UserName,
                    message = sendMsgRes.Data.Message,
                };
                await Clients.All.SendAsync("ReceiveMessage", _message);
            }
            
        }
        public async Task SendToAdminNoService(MessageHubModel request)
        {
            var _message = new
            {
                userId = request.FromUserId,
                userName = request.UserName,
                message = request.Message,
            };
            await Clients.All.SendAsync("ReceiveMessage", _message);
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
