using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Comment;
using ECommerce.Application.Repositories.Interest;
using ECommerce.Application.Repositories.Notification;
using ECommerce.Application.Repositories.User;
using ECommerce.Application.Services.Rate.Dtos;
using ECommerce.Application.Services.Rate.Models;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Comment
{
    public class CommentService : ICommentService
    {
        private ECommerceContext _DbContext;
        private INotificationRepository _notificationRepo;
        private ICommentRepository _commentRepo;
        private IRepositoryBase<RatingImage> _commentImageRepo;
        private IUserRepository _userRepo;
        private IInterestRepository _interestRepo;
        public CommentService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
            if (_commentRepo == null)
                _commentRepo = new CommentRepository(_DbContext);
            if (_notificationRepo == null)
                _notificationRepo = new NotificationRepository(_DbContext);
            if (_commentImageRepo == null)
                _commentImageRepo = new RepositoryBase<RatingImage>(_DbContext);
            if (_userRepo == null)
                _userRepo = new UserRepository(_DbContext);
            if (_interestRepo == null)
                _interestRepo = new InterestRepository(_DbContext);
        }
        // Repository get
        public ICommentRepository Comment { get => _commentRepo; }
        public INotificationRepository Notification { get => _notificationRepo; }
        public IRepositoryBase<RatingImage> CommentImage { get => _commentImageRepo; }
        public IUserRepository User { get => _userRepo; }
        public IInterestRepository Interest { get => _interestRepo; }
        // Service methods       
        public async Task<Response<List<string>>> ReplyCommentAsync(ReplyCommentRequest request)
        {
            if (string.IsNullOrEmpty(request.comment)) 
                return new FailResponse<List<string>>("Nội dung không được để trống");
            try
            {
                // Add comment
                string idsToDelete = null;
                var all = await _commentRepo.ToListAsync();
                var replied = await _commentRepo.FindAsyncWhere(i => i.RateId == request.repliedId);
                if (replied != null)
                    idsToDelete = string.IsNullOrEmpty(replied.IdsToDelete) ? request.repliedId.ToString() : (replied.IdsToDelete + "," + replied.RateId);
                var comment = new Data.Models.Rate()
                {
                    Comment = request.comment,
                    ProductId = request.productId,
                    UserId = request.userId,
                    UserRepliedId = request.userRepliedId,
                    RepliedId = request.repliedId,
                    ParentId = request.parentId,
                    IdsToDelete = idsToDelete,
                    CreateDate = DateTime.Now,
                    RateValue = 0,
                };
                await _commentRepo.AddAsync(comment);

                // Add Comment Image;
                if (request.fileNames != null)
                {
                    List<RatingImage> images = new List<RatingImage>();
                    foreach (var filename in request.fileNames)
                    {
                        var image = new RatingImage()
                        {
                            RateId = comment.RateId,
                            RatingImagePath = filename,
                        };
                        images.Add(image);
                    }
                    await _commentImageRepo.AddRangeAsync(images);
                }
                await SaveChangesAsync();

                // Add Notification
                await _notificationRepo.CreateCommentNotiAsync(comment);
                await SaveChangesAsync();
                return new SuccessResponse<List<string>>("Phản hồi thành công");
            }
            catch (Exception error)
            {
                return new FailResponse<List<string>>("Lỗi \n\n" + error.ToString());
            }
        }
        public async Task<Response<LikeAndDislike>> LikeComment(LikeRequest request)
        {
            try
            {
                var result = await _interestRepo.LikeComment(request);

                

                await SaveChangesAsync();
                return new SuccessResponse<LikeAndDislike>("Đánh giá thành công", result);
            }
            catch (Exception e)
            {
                return new FailResponse<LikeAndDislike>("Lỗi \n\n" + e.Message);
            }
        }


        public async Task SaveChangesAsync() => await _DbContext.SaveChangesAsync();
    }
}
