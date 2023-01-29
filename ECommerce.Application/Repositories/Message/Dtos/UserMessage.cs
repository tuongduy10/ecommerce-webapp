using ECommerce.Application.Services.Chat.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Repositories.Message.Dtos
{
    public class UserMessage
    {
        public int UserId { get; set; }
        public string FromName { get; set; }
        public string FromPhoneNumber { get; set; }
        public List<MessageModel> MessageList { get; set; }
        public MessageModel LatestMessage { get; set; }
    }
}
