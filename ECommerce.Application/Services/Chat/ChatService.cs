using ECommerce.Application.Common;
using ECommerce.Application.Constants;
using ECommerce.Application.Helpers;
using ECommerce.Application.Repositories.Message;
using ECommerce.Application.Repositories.Message.Dtos;
using ECommerce.Application.Repositories.Notification;
using ECommerce.Application.Repositories.User;
using ECommerce.Application.Services.Chat.Dtos;
using ECommerce.Application.Services.User_v2;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
                var fromUserInfo = await _userRepo.GetUserInfo(request.FromUserId);

                var newMessageHistory = new MessageHistory();
                newMessageHistory.Message = request.Message.Trim();
                newMessageHistory.CreateDate = DateTime.Now;
                newMessageHistory.Status = StatusConstant.MSG_UNREAD;
                newMessageHistory.FromUserId = request.FromUserId;
                newMessageHistory.ToUserId = request.ToUserId;
                newMessageHistory.UserName = fromUserInfo != null ? fromUserInfo.UserFullName : "";
                newMessageHistory.PhoneNumber = fromUserInfo != null ? fromUserInfo.UserPhone : "";
                newMessageHistory.Type = request.Type;
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
                resModel.UserName = newMessageHistory.UserName;
                resModel.Attachment = newMessageHistory.Attachment;
                resModel.Type = newMessageHistory.Type;
                resModel.Status = newMessageHistory.Status;

                return new SuccessResponse<MessageModel>("", resModel);
            }
            catch (Exception error)
            {
                return new FailResponse<MessageModel>(error.ToString());
            }
        }
        public async Task<Response<MessageModel>> SendUnAuthMessage(MessageModel request)
        {
            try
            {
                if (String.IsNullOrEmpty(request.UserName))
                    return new FailResponse<MessageModel>("Tên không được để trống");
                if (String.IsNullOrEmpty(request.PhoneNumber))
                    return new FailResponse<MessageModel>("Số điện thoại không được để trống");
                if (String.IsNullOrEmpty(request.Message))
                    return new FailResponse<MessageModel>("Nội dung không được để trống");

                if (request.PhoneNumber.Contains("+84"))
                {
                    request.PhoneNumber = request.PhoneNumber.Replace("+84", "");
                    if (!request.PhoneNumber.StartsWith("0"))
                    {
                        request.PhoneNumber = "0" + request.PhoneNumber;
                    }
                }
                var userInfo = await _userRepo.FindAsyncWhere(i => i.UserPhone == request.PhoneNumber);
                if (userInfo != null)
                {
                    request.UserName = userInfo.UserFullName;
                    request.FromUserId = userInfo.UserId;
                }

                var newMessageHistory = new MessageHistory();
                newMessageHistory.Message = request.Message.Trim();
                newMessageHistory.CreateDate = DateTime.Now;
                newMessageHistory.FromUserId = request.FromUserId;
                newMessageHistory.ToUserId = request.ToUserId;
                newMessageHistory.UserName = request.UserName.Trim();
                newMessageHistory.PhoneNumber = request.PhoneNumber.Trim();
                newMessageHistory.Type = TypeConstant.MSG_FROM_CLIENT;
                newMessageHistory.Status = StatusConstant.MSG_UNREAD;

                await _msgRepo.AddAsync(newMessageHistory);
                await _msgRepo.SaveChangesAsync();

                // response model
                var resModel = new MessageModel();
                resModel.Message = newMessageHistory.Message;
                resModel.CreateDate = newMessageHistory.CreateDate;
                resModel.ToUserId = newMessageHistory.ToUserId;
                resModel.FromUserId = newMessageHistory.FromUserId;
                resModel.UserName = newMessageHistory.UserName;
                resModel.Attachment = newMessageHistory.Attachment;
                resModel.Type = newMessageHistory.Type;
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
        public async Task<Response<List<UserMessage>>> GetAllUserMessagesAsync(int sellerId = 0)
        {
            try
            {
                var groupMsg = from msg in _msgRepo.Query().ToList()
                               group msg by new
                               {
                                   msg.FromUserId,
                                   msg.UserName,
                                   msg.PhoneNumber
                               };
                    
                var list = new List<UserMessage>();
                foreach (var msg in groupMsg)
                {
                    var messageList = msg
                        .Select(i => new MessageModel() 
                        {
                            FromUserId = i.FromUserId,
                            ToUserId = i.ToUserId,
                            CreateDate = i.CreateDate,
                            CreateDateLabel = ((DateTime)i.CreateDate).ToString(ConfigConstant.DATE_FORMAT),
                            Message = i.Message,
                            Status = i.Status,
                            Type = i.Type,
                            Attachment = i.Attachment
                        })
                        .ToList();
                    list.Add(new UserMessage() { 
                        UserId = msg.Key.FromUserId,
                        UserName = msg.Key.UserName,
                        PhoneNumber = msg.Key.PhoneNumber,
                        MessageList = messageList,
                        LatestMessage = messageList.Count > 0 ? messageList[messageList.Count - 1] : null
                    });
                }
                list = list.OrderByDescending(i => i.LatestMessage.CreateDate).ToList();
                return new SuccessResponse<List<UserMessage>>("", list);
            }
            catch (Exception error)
            {
                return new FailResponse<List<UserMessage>>(error.ToString());
            }
        }
        public async Task<Response<List<UserMessage>>> GetUserList()
        {
            try
            {
                var groupMsg = from msg in _msgRepo.Query().ToList()
                               where msg.Type == TypeConstant.MSG_FROM_CLIENT
                               group msg by new
                               {
                                   msg.FromUserId,
                                   msg.UserName,
                                   msg.PhoneNumber
                               };

                var list = new List<UserMessage>();
                foreach (var msg in groupMsg)
                {
                    var messages = await _msgRepo
                            .Query()
                            .Where(item =>
                                (item.FromUserId == msg.Key.FromUserId && item.ToUserId == null) ||
                                item.FromUserId == msg.Key.FromUserId ||
                                item.ToUserId == msg.Key.FromUserId)
                            .Select(i => new MessageModel()
                            {
                                Message = i.Message,
                                CreateDate = i.CreateDate,
                                CreateDateLabel = ((DateTime)i.CreateDate).ToString(ConfigConstant.DATE_FORMAT),
                                Status = i.Status
                            })
                            .ToListAsync();
                    list.Add(new UserMessage()
                    {
                        UserId = msg.Key.FromUserId,
                        UserName = msg.Key.UserName,
                        PhoneNumber = msg.Key.PhoneNumber,
                        LatestMessage = messages[messages.Count() - 1]
                    });
                }
                list = list.OrderByDescending(i => i.LatestMessage.CreateDate).ToList();
                return new SuccessResponse<List<UserMessage>>("", list);
            }
            catch (Exception error)
            {
                return new FailResponse<List<UserMessage>>(error.ToString());
            }
        }
        public async Task<Response<List<MessageModel>>> GetMessages(int userId = 0)
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
                        UserName = msg.UserName,
                        ToUserId = msg.ToUserId,
                        Type = msg.Type,
                        Status = msg.Status,
                    })
                    .ToListAsync();

                list = list.Where(item =>
                    (item.FromUserId == userId && item.ToUserId == null) ||
                    item.FromUserId == userId ||
                    item.ToUserId == userId).ToList();
                list = list.OrderBy(item => item.CreateDate).ToList();

                // read the messages
                await ReadMessageAsync(list);

                return new SuccessResponse<List<MessageModel>>(list);
            }
            catch (Exception error)
            {
                return new FailResponse<List<MessageModel>>(error.ToString());
            }
        }
        private async Task ReadMessageAsync(List<MessageModel> list)
        {
            // read the messages
            var unReadMsgIds = list
                .Where(i => i.Status == StatusConstant.MSG_UNREAD)
                .Select(i => i.Id)
                .ToList();
            if (unReadMsgIds.Count() > 0)
            {
                var unReadMsg = await _msgRepo.ToListAsyncWhere(i => unReadMsgIds.Contains(i.Id));
                foreach (var msg in unReadMsg)
                {
                    msg.Status = StatusConstant.MSG_READ;
                }
                _msgRepo.UpdateRange(unReadMsg);
                await _msgRepo.SaveChangesAsync();
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


                var messageModel = new MessageHistory();
                messageModel.Message = request.Message.Trim();
                messageModel.UserName = request.UserName.Trim();
                messageModel.PhoneNumber = request.PhoneNumber.Trim();
                await _msgRepo.AddAsync(messageModel);
                await _msgRepo.SaveChangesAsync();


                return new SuccessResponse<MessageModel>("success");
            }
            catch (Exception error)
            {
                return new FailResponse<MessageModel>(error.ToString());
            }
        }
    }
}
