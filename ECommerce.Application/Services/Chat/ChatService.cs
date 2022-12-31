using ECommerce.Application.Common;
using ECommerce.Application.Constants;
using ECommerce.Application.Repositories.Message;
using ECommerce.Application.Repositories.Message.Dtos;
using ECommerce.Application.Repositories.Notification;
using ECommerce.Application.Repositories.User;
using ECommerce.Application.Services.Chat.Dtos;
using ECommerce.Application.Services.User_v2;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Chat
{
    public class ChatService : IChatService
    {
        private ECommerceContext _DbContext;
        private INotificationRepository _notiRepo;
        private IMessageRepository _msgRepo;
        private IUserRepository _userRepo;
        public ChatService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
            if(_notiRepo == null)
                _notiRepo = new NotificationRepository(_DbContext);
            if(_msgRepo == null)
                _msgRepo = new MessageRepository(_DbContext);
            if(_userRepo == null)
                _userRepo = new UserRepository(_DbContext);
        }
        public async Task<Response<MessageModel>> SendMessage(MessageModel request)
        {
            try
            {
                if (String.IsNullOrEmpty(request.Message))
                    return new FailResponse<MessageModel>("Nội dung không được để trống");

                var newMessageHistory = new MessageHistory();
                newMessageHistory.Message = request.Message.Trim();
                newMessageHistory.CreateDate = DateTime.Now;
                newMessageHistory.Status = StatusConstant.MSG_UNREAD;
                newMessageHistory.FromUserId = request.FromUserId;
                newMessageHistory.ToUserId = request.ToUserId;
                if(!String.IsNullOrEmpty(request.Attachment))
                    newMessageHistory.Attachment = request.Attachment.Trim();

                await _msgRepo.AddAsync(newMessageHistory);
                await _msgRepo.SaveChangesAsync();

                // response model
                var resModel = new MessageModel();
                resModel.Message = newMessageHistory.Message;
                resModel.CreateDate = newMessageHistory.CreateDate;
                resModel.ToUserId = newMessageHistory.ToUserId;
                resModel.FromUserId = newMessageHistory.FromUserId;
                resModel.Attachment = newMessageHistory.Attachment;
                resModel.Status = newMessageHistory.Status;

                return new SuccessResponse<MessageModel>("", resModel);
            }
            catch (Exception error)
            {
                return new FailResponse<MessageModel>(error.ToString());
            }
        }
        public async Task<Response<List<UserMessage>>> GetUserMessagesAsync(int userId = 0)
        {
            try
            {
                var list = await _userRepo
                    .Query()
                    .Select(i => new UserMessage() { 
                        UserId = i.UserId,
                        UserName = i.UserFullName,
                        PhoneNumber = i.UserPhone,
                        MessageList = _DbContext.MessageHistories
                            .Where(msg => msg.FromUserId == i.UserId)
                            .Select(msg => new MessageModel() { 
                                Id = msg.Id,
                                Message = msg.Message,
                                Attachment = msg.Attachment,
                                CreateDate = msg.CreateDate,
                                ToUserId = msg.ToUserId,
                                Status = msg.Status
                            })
                            .ToList()
                    })
                    .ToListAsync();
                list = list.Where(item => item.MessageList.Count > 0).ToList();
                if(userId != 0)
                    list = list.Where(item => item.UserId == userId).ToList();

                return new SuccessResponse<List<UserMessage>>("", list);
            }
            catch (Exception error)
            {
                return new FailResponse<List<UserMessage>>(error.ToString());
            }
        }
        public async Task<Response<List<MessageModel>>> GetMessages(int fromUserId = 0, int toUserId = 0)
        {
            try
            {
                var list = await _msgRepo
                    .Query()
                    .Select(msg => new MessageModel() {
                        Id = msg.Id,
                        Attachment = msg.Attachment,
                        CreateDate = msg.CreateDate,
                        CreateDateLabel = ((DateTime)msg.CreateDate).ToString(ConfigConstant.DATE_FORMAT),
                        Message = msg.Message,
                        FromUserId = msg.FromUserId,
                        ToUserId = msg.ToUserId,
                        Status = msg.Status,
                    })
                    .ToListAsync();

                if (fromUserId != 0 && toUserId != 0)
                    list = list.Where(item => 
                        (item.FromUserId == toUserId && item.ToUserId == null) || 
                        (item.FromUserId == fromUserId && item.ToUserId == toUserId)).ToList();

                return new SuccessResponse<List<MessageModel>>(list);
            }
            catch (Exception error)
            {
                return new FailResponse<List<MessageModel>>(error.ToString());
            }
        }
        public async Task<Response<MessageModel>> SaveOfflineMessageAsync(OfflineMessage request)
        {
            try
            {
                if (String.IsNullOrEmpty(request.PhoneNumber))
                    return new FailResponse<MessageModel>("Số điện thoại không được để trống");
                if (String.IsNullOrEmpty(request.UserName))
                    return new FailResponse<MessageModel>("Tên không được để trống");
                if (String.IsNullOrEmpty(request.Message))
                    return new FailResponse<MessageModel>("Nội dung không được để trống");
                if (request.PhoneNumber.Count() > 10)
                    return new FailResponse<MessageModel>("Số điện thoại không hợp lệ");
                if (request.UserName.Count() > 100)
                    return new FailResponse<MessageModel>("Tên không hợp lệ");
                if (request.Message.Count() > 500)
                    return new FailResponse<MessageModel>("Nội dung không được vượt quá 500 ký tự");

                


                return new SuccessResponse<MessageModel>("success");
            }
            catch (Exception error)
            {
                return new FailResponse<MessageModel>(error.ToString());
            }
        }
    }
}
