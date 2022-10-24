using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Comment;
using ECommerce.Application.Repositories.Notification;
using ECommerce.Application.Repositories.Notification.Dtos;
using ECommerce.Application.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Notification
{
    public interface INotificationService
    {
        INotificationRepository Notification { get; }
        IUserRepository User { get; }
        Task<Response<NotificationModel>> ReadAsync(int id = 0);
        Task<Response<string>> DeleteAsync(int id = 0);
        Task<List<NotificationModel>> GetAllByUserIdAsync(int userId = 0);
        Task SaveAsync();
    }
}
