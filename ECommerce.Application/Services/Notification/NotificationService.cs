using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Comment;
using ECommerce.Application.Repositories.Notification;
using ECommerce.Application.Repositories.Notification.Dtos;
using ECommerce.Application.Repositories.User;
using ECommerce.Application.BaseServices.User.Enums;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private ECommerceContext _DbContext;
        private INotificationRepository _notificationRepo;
        private IUserRepository _userRepo;
        public NotificationService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
            if (_notificationRepo == null)
                _notificationRepo = new NotificationRepository(_DbContext);
            if (_userRepo == null)
                _userRepo = new UserRepository(_DbContext);
        }
        public INotificationRepository Notification
        {
            get
            {
                return _notificationRepo;
            }
        }
        public IUserRepository User
        {
            get
            {
                return _userRepo;
            }
        }
        public async Task<List<NotificationModel>> GetAllByUserIdAsync(int userId = 0)
        {
            var list = await _notificationRepo
                .Query(item => item.ReceiverId == userId)
                .Select(item => new NotificationModel
                {
                    Id = item.Id,
                    TextContent = item.TextContent,
                    ReceiverId = item.ReceiverId == null ? 0 : (int)item.ReceiverId,
                    SenderId = item.SenderId == null ? 0 : (int)item.SenderId,
                    JsLink = item.JsLink,
                    CreateDate = (DateTime)item.CreateDate,
                    IsRead = (bool)item.IsRead,
                    ReceiverIsAmin = item.Receiver == null ? false  : item.Receiver.UserRoles.Select(role => role.Role.RoleName).FirstOrDefault().Contains(RoleName.Admin),
                    SenderIsAdmin = item.Sender == null ? false : item.Sender.UserRoles.Select(role => role.Role.RoleName).FirstOrDefault().Contains(RoleName.Admin),
                    SenderName = item.Sender == null ? "" : item.Sender.UserFullName,
                    TypeCode = item.Type.TypeCode
                })
                .OrderByDescending(item => item.CreateDate)
                .ToListAsync();
            return list;
        }
        public async Task<Response<NotificationModel>> ReadAsync(int id = 0)
        {
            try
            {
                var notification = await _notificationRepo.GetAsyncWhere(item => item.Id == id);
                notification.IsRead = true;
                _notificationRepo.Update(notification);

                var resData = await _notificationRepo.FindByIdAsync(id);
                await SaveAsync();

                return new SuccessResponse<NotificationModel>("", resData);
            }
            catch (Exception error)
            {
                return new FailResponse<NotificationModel>("Lỗi \n\n" + error.ToString());
            }
        }
        public async Task<Response<string>> DeleteAsync(int id = 0)
        {
            try
            {
                var notification = await _notificationRepo.RemoveByIdAsync(id);
                await SaveAsync();

                return new SuccessResponse<string>("Xóa thành công");
            }
            catch (Exception error)
            {
                return new FailResponse<string>("Lỗi \n\n" + error.ToString());
            }
        }
        public async Task SaveAsync() => await _DbContext.SaveChangesAsync();
    }
}
