using ECommerce.Application.Common;
using ECommerce.Application.Repositories.Notification.Dtos;
using ECommerce.Application.Services.Rate.Dtos;
using ECommerce.Application.Services.Rate.Models;
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
                InfoId = comment.RateId
            };
            await AddAsync(notification);
            return notification;
        }
        public async Task<Data.Models.Notification> CreateLikeDislikeNotiAsync(Rate comment)
        {
            var notification = await FindAsyncWhere(item => item.InfoId == comment.RateId && item.TypeId == (int)Enums.NotificationType.Like);
            var userNames = await _DbContext.Interests
                        .Where(item => item.RateId == comment.RateId && item.Liked == true)
                        .Select(item => item.User.UserFullName)
                        .ToListAsync();

            var textConent = "";
            if (userNames.Count == 1)
                textConent = $"{userNames[0]} đã thích bình luận của bạn";
            if (userNames.Count > 1 && userNames.Count <= 4)
                textConent = $"{String.Join(", ", userNames.SkipLast(1))} và {userNames.TakeLast(1)} đã thích bình luận của bạn";
            if (userNames.Count > 4)
                textConent = $"{String.Join(", ", userNames.Take(4))} và {userNames.Count - 4} người khác đã thích bình luận của bạn";

            if (notification == null && textConent != "")
            {
                // add new
                var newNotification = new Data.Models.Notification()
                {
                    TextContent = textConent,
                    ReceiverId = comment.UserId,
                    SenderId = null,
                    JsLink = $"/Product/ProductDetail?ProductId={comment.ProductId}&isScrolledTo=true&commentId={comment.RateId}",
                    TypeId = (int?)Enums.NotificationType.Like,
                    CreateDate = DateTime.Now,
                    InfoId = comment.RateId,
                    IsRead = false,
                };
                await AddAsync(newNotification);
                await SaveChangesAsync();
                return newNotification; // test git branch
            }
            else
            {
                var likeCount = comment.Interests.Where(item => item.Liked == true).Count();
                if (likeCount == 0 || textConent == "")
                {
                    await RemoveAsyncWhere(item => item.InfoId == comment.RateId);
                    await SaveChangesAsync();
                    return null;
                }
                else
                {
                    // update
                    notification.TextContent = textConent;
                    notification.IsRead = false;
                    Update(notification);
                    await SaveChangesAsync();
                    return notification;
                }
            }
        }
        public async Task<NotificationModel> FindByIdAsync(int id = 0)
        {
            var model = await Query(item => item.Id == id)
                .Select(item => new NotificationModel
                {
                    Id = item.Id,
                    TextContent = item.TextContent,
                    ReceiverId = item.ReceiverId == null ? 0 : (int)item.ReceiverId,
                    SenderId = item.SenderId == null ? 0 : (int)item.SenderId,
                    JsLink = item.JsLink,
                    CreateDate = (DateTime)item.CreateDate,
                    IsRead = (bool)item.IsRead,
                    ReceiverIsAmin = item.Receiver == null ? false : item.Receiver.UserRoles.Select(role => role.Role.RoleName).FirstOrDefault().Contains(RoleName.Admin),
                    SenderIsAdmin = item.Sender == null ? false : item.Sender.UserRoles.Select(role => role.Role.RoleName).FirstOrDefault().Contains(RoleName.Admin),
                    SenderName = item.Sender == null ? "" : item.Sender.UserFullName,
                    TypeCode = item.Type.TypeCode
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
