using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Comment;
using ECommerce.Application.Repositories.Interest;
using ECommerce.Application.Repositories.Notification;
using ECommerce.Application.Repositories.User;
using ECommerce.Application.Services.Comment.Request;
using ECommerce.Application.BaseServices.Rate.Dtos;
using ECommerce.Application.BaseServices.Rate.Models;
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
        Task<Response<bool>> postComment(PostCommentRequest request);
        Task<Response<LikeAndDislike>> LikeComment(LikeAndDislikeCount request);
        Task<Response<List<string>>> ReplyCommentAsync(ReplyCommentRequest request);
        Task<Response<List<string>>> UpdateComment(UpdateCommentRequest request);
        Task<Response<List<RateGetModel>>> getRates(RateGetRequest request);
        Task<Response<List<string>>> GetUsersFavor(UserFavorRequest request);
        Task<Response<List<string>>> DeleteByProductId(int _productId);
        Task<Response<List<string>>> DeleteByUserId(int _userId);
        Task SaveChangesAsync();
    }
}
