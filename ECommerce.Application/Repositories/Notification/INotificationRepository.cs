using ECommerce.Application.Common;
using ECommerce.Application.Repositories.Notification.Dtos;
using ECommerce.Application.BaseServices.Rate.Dtos;
using ECommerce.Application.BaseServices.Rate.Models;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Notification
{
    public interface INotificationRepository : IRepositoryBase<Data.Models.Notification>
    {
        Task<bool> RemoveByIdAsync(int id = 0);
        Task<NotificationModel> FindByIdAsync(int id = 0);
        Task<Data.Models.Notification> CreateCommentNotiAsync(Rate comment);
        Task<Data.Models.Notification> CreateLikeDislikeNotiAsync(Rate comment);
        Task<Data.Models.Notification> CreateMessageHistoryAsync(MessageModel message);
    }
}
