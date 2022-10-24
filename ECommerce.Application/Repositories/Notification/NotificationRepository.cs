using ECommerce.Application.Common;
using ECommerce.Application.Repositories.Notification.Dtos;
using ECommerce.Application.Services.User.Enums;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Notification
{
    public class NotificationRepository : RepositoryBase<Data.Models.Notification>, INotificationRepository
    {
        public NotificationRepository(ECommerceContext DbContext):base(DbContext)
        {

        }

        public async Task<Data.Models.Notification> CreateCommentNotiAsync(Rate comment)
        {
            // Notification
            var notification = new Data.Models.Notification()
            {
                TextContent = comment.Comment,
                CreateDate = DateTime.Now,
                IsRead = false,
                ReceiverId = comment.UserRepliedId,
                SenderId = comment.UserId,
                JsLink = $"/Product/ProductDetail?ProductId={comment.ProductId}&isScrolledTo=true&commentId={comment.RateId}",
                TypeId = (int?)Enums.NotificationType.Comment,
            };
            await AddAsync(notification);
            return notification;
        }
        public async Task<Data.Models.Notification> CreateLikeDislikeNotiAsync()
        {
            // Notification
            var notification = new Data.Models.Notification()
            {
                TextContent = "người đã thích bình luận của bạn",
                ReceiverId = 0,
                SenderId = 0,
                JsLink = $"/Product/ProductDetail?ProductId={0}&isScrolledTo=true&commentId={0}",
                TypeId = (int?)Enums.NotificationType.Comment,
                CreateDate = DateTime.Now,
                IsRead = false,
            };
            return notification;
        }
        public async Task<NotificationModel> FindByIdAsync(int id = 0)
        {
            var model = await Query(item => item.Id == id)
                .Select(item => new NotificationModel
                {
                    Id = item.Id,
                    TextContent = item.TextContent,
                    ReceiverId = (int)item.ReceiverId,
                    SenderId = (int)item.SenderId,
                    JsLink = item.JsLink,
                    IsRead = (bool)item.IsRead,
                    ReceiverIsAmin = item.Receiver.UserRoles.Select(role => role.Role.RoleName).FirstOrDefault().Contains(RoleName.Admin),
                    SenderIsAdmin = item.Sender.UserRoles.Select(role => role.Role.RoleName).FirstOrDefault().Contains(RoleName.Admin),
                    SenderName = item.Sender.UserFullName
                })
                .FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> RemoveByIdAsync(int id = 0)
        {
            var entity = await FindAsyncWhere(item => item.Id == id);
            Remove(entity);
            return true;
        }
    }
}
