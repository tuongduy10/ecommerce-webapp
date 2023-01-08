using ECommerce.Application.Common;
using ECommerce.Application.Repositories.Message.Dtos;
using ECommerce.Application.Services.Chat.Dtos;
using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Chat
{
    public interface IChatService
    {
        Task<Response<MessageModel>> SendMessage(MessageModel request);
        Task<Response<List<UserMessage>>> GetUserMessagesAsync(int userId = 0);
        Task<Response<List<UserMessage>>> GetAllUserMessagesAsync();
        Task<Response<List<MessageModel>>> GetMessages(int fromUserId = 0, int toUserId = 0);
    }
}
