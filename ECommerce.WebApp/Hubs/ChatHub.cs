using ECommerce.Application.Constants;
using ECommerce.Application.Services.Chat;
using ECommerce.Application.Services.Chat.Dtos;
using ECommerce.Data.Entities;
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
        public async Task SendToAdmin(MessageHubModel request)
        {
            var msgModel = new MessageModel();
            msgModel.Message = request.Message;
            msgModel.FromName = request.FromName;
            msgModel.FromPhoneNumber = request.FromPhoneNumber;
            msgModel.Type = TypeConstant.MSG_FROM_CLIENT;

            if (String.IsNullOrEmpty(msgModel.FromName))
            {
                var sendMsgRes = await _chatService.SendMessage(msgModel);
                var _message = new
                {
                    fromPhoneNumber = sendMsgRes.Data.FromPhoneNumber,
                    fromName = sendMsgRes.Data.FromName,
                    message = sendMsgRes.Data.Message,
                };
                await Clients.All.SendAsync("ReceiveMessage", _message);
            }
            else
            {
                var sendMsgRes = await _chatService.SendUnAuthMessage(msgModel);
                var _message = new
                {
                    fromPhoneNumber = sendMsgRes.Data.FromPhoneNumber,
                    fromName = sendMsgRes.Data.FromName,
                    message = sendMsgRes.Data.Message,
                };
                await Clients.All.SendAsync("ReceiveMessage", _message);
            }
        }
        public async Task SendToAdminNoService(MessageHubModel model)
        {
            var _message = new
            {
                fromPhoneNumber = model.FromPhoneNumber,
                fromName = model.FromName,
                message = model.Message,
            };
            await Clients.All.SendAsync("ReceiveMessage", _message);
        }
        public async Task SendToClient(MessageHubModel model)
        {
            var msgModel = new MessageModel();
            msgModel.FromName = !String.IsNullOrEmpty(model.FromName) ? model.FromName : "";
            msgModel.FromPhoneNumber = !String.IsNullOrEmpty(model.FromPhoneNumber) ? model.FromPhoneNumber : "";
            msgModel.ToPhoneNumber = !String.IsNullOrEmpty(model.ToPhoneNumber) ? model.ToPhoneNumber : "";
            msgModel.Type = TypeConstant.MSG_FROM_ADMIN;
            msgModel.Message = model.Message;

            var sendMsgRes = await _chatService.SendMessage(msgModel);

            await Clients.All.SendAsync("ReceiveFromAdmin", sendMsgRes.Data);
        }
    }
}
