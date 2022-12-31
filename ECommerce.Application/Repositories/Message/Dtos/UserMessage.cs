using ECommerce.Application.Services.Chat.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Repositories.Message.Dtos
{
    public class UserMessage
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<MessageModel> MessageList { get; set; }
    }
}
