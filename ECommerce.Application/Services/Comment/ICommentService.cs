using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Comment;
using ECommerce.Application.Repositories.Interest;
using ECommerce.Application.Repositories.Notification;
using ECommerce.Application.Repositories.User;
using ECommerce.Application.Services.Comment.Request;
using ECommerce.Application.Services.Rate.Dtos;
using ECommerce.Application.Services.Rate.Models;
using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Comment
{
    public interface ICommentService
    {
        // Repository get
        ICommentRepository Comment { get; }
        INotificationRepository Notification { get; }
        IRepositoryBase<RatingImage> CommentImage { get; }
        IUserRepository User { get; }
        IInterestRepository Interest { get; }
        // Service methods
        Task<Response<LikeAndDislike>> LikeComment(LikeAndDislikeCount request);
        Task<Response<List<string>>> ReplyCommentAsync(ReplyCommentRequest request);
        Task<Response<List<RateGetModel>>> GetAllByProductId(int _productId, int _userId = 0);
        Task<Response<List<string>>> GetUsersFavor(UserFavorRequest request);
        Task SaveChangesAsync();
    }
}
