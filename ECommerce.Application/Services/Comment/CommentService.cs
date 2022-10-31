using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Comment;
using ECommerce.Application.Repositories.Interest;
using ECommerce.Application.Repositories.Notification;
using ECommerce.Application.Repositories.User;
using ECommerce.Application.Services.Comment.Request;
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
        // Repositories
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
                await SaveChangesAsync();

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
                var noti = await _notificationRepo.CreateCommentNotiAsync(comment);
                await SaveChangesAsync();
                return new SuccessResponse<List<string>>("Phản hồi thành công");
            }
            catch (Exception error)
            {
                return new FailResponse<List<string>>("Lỗi \n\n" + error.ToString());
            }
        }
        public async Task<Response<List<string>>> UpdateComment(UpdateCommentRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.content))
                    return new FailResponse<List<string>>("Nội dung không được để trống");

                var comment = await _commentRepo.FindAsyncWhere(item => item.RateId == request.id);
                if (comment == null)
                    return new FailResponse<List<string>>("Không tìm thấy bình luận hoặc bình luận đã bị xóa");

                // Update comment context
                if (comment != null) 
                {
                    comment.Comment = request.content;
                    comment.RateValue = request.rateValue;
                }
                _commentRepo.Update(comment);
                await SaveChangesAsync();

                // Add images
                if (request.fileNames != null)
                {
                    foreach (var filename in request.fileNames)
                    {
                        var image = new RatingImage()
                        {
                            RateId = comment.RateId,
                            RatingImagePath = filename,
                        };
                        await _commentImageRepo.AddAsync(image);
                        await SaveChangesAsync();
                    }
                }

                // Notification
                await _notificationRepo.CreateCommentNotiAsync(comment);

                return new SuccessResponse<List<string>>("Cập nhật thành công");
            }
            catch (Exception e)
            {
                return new FailResponse<List<string>>(e.ToString());
            }
        }
        public async Task<Response<LikeAndDislike>> LikeComment(LikeAndDislikeCount request)
        {
            try
            {
                // Like
                var result = await _interestRepo.LikeComment(request);

                // Notification
                var comment = await _commentRepo.FindAsyncWhere(item => item.RateId == request.rateId);
                await _notificationRepo.CreateLikeDislikeNotiAsync(comment);

                return new SuccessResponse<LikeAndDislike>("Đánh giá thành công", result);
            }
            catch (Exception e)
            {
                return new FailResponse<LikeAndDislike>("Lỗi \n\n" + e.Message);
            }
        }
        public async Task<Response<List<RateGetModel>>> GetAllByProductId(int _productId, int _userId = 0)
        {
            try
            {
                var rate = await _DbContext.Rates
                .Where(rate => rate.ProductId == _productId && rate.ParentId == null)
                .Select(rate => new RateGetModel()
                {
                    Id = rate.RateId,
                    Value = (int)rate.RateValue,
                    UserId = rate.User.UserId,
                    UserName = rate.User.UserFullName,
                    Comment = rate.Comment,
                    CreateDate = (DateTime)rate.CreateDate,
                    Images = _DbContext.RatingImages
                        .Where(img => img.RateId == rate.RateId)
                        .Select(i => new ImageModel()
                        {
                            id = i.RatingImageId,
                            path = i.RatingImagePath
                        })
                        .ToList(),
                    CanAction = rate.UserId == _userId,
                    IsAdmin = _DbContext.UserRoles
                            .Where(isa => isa.UserId == rate.User.UserId)
                            .Select(isa => isa.Role.RoleName)
                            .ToList()
                            .Contains(RoleName.Admin),
                    Liked = _DbContext.Interests
                            .Where(itr => itr.RateId == rate.RateId && itr.Liked == true)
                            .Select(itr => itr.UserId)
                            .ToList()
                            .Contains(_userId),
                    LikeCount = rate.Interests.Where(lc => lc.Liked == true).Count(),
                    Disliked = _DbContext.Interests
                            .Where(itr => itr.RateId == rate.RateId && itr.Liked == false)
                            .Select(itr => itr.UserId)
                            .ToList()
                            .Contains(_userId),
                    DislikeCount = rate.Interests.Where(dc => dc.Liked == false).Count(),
                    UsersLike = rate.Interests
                            .Where(ul => ul.Liked == true)
                            .Select(ul => ul.User.UserFullName)
                            .ToList(),
                    UsersDislike = rate.Interests
                            .Where(ud => ud.Liked == false)
                            .Select(ud => ud.User.UserFullName)
                            .ToList(),
                    Replies = _DbContext.Rates
                    .Where(reply => reply.ParentId == rate.RateId)
                    .Select(reply => new RateGetModel()
                    {
                        Id = reply.RateId,
                        UserId = reply.User.UserId,
                        UserName = reply.User.UserFullName,
                        UserRepliedId = reply.UserReplied.UserId,
                        UserRepliedName = reply.UserReplied.UserFullName,
                        IsAdmin = _DbContext.UserRoles
                            .Where(isa => isa.UserId == reply.User.UserId)
                            .Select(isa => isa.Role.RoleName)
                            .ToList()
                            .Contains(RoleName.Admin),
                        IsSeller = _DbContext.UserRoles
                            .Where(isa => isa.UserId == reply.User.UserId)
                            .Select(isa => isa.Role.RoleName)
                            .ToList()
                            .Contains(RoleName.Seller),
                        IsShopOwner = _DbContext.Products
                            .Where(iso => iso.ProductId == _productId)
                            .Select(iso => iso.Shop.UserId)
                            .FirstOrDefault() == reply.User.UserId,
                        Comment = reply.Comment,
                        Liked = _DbContext.Interests
                            .Where(itr => itr.RateId == reply.RateId && itr.Liked == true)
                            .Select(itr => itr.UserId)
                            .ToList()
                            .Contains(_userId),
                        LikeCount = reply.Interests.Where(lc => lc.Liked == true).Count(),
                        DislikeCount = reply.Interests.Where(dc => dc.Liked == false).Count(),
                        Disliked = _DbContext.Interests
                            .Where(itr => itr.RateId == reply.RateId && itr.Liked == false)
                            .Select(itr => itr.UserId)
                            .ToList()
                            .Contains(_userId),
                        CanAction = reply.UserId == _userId,
                        CreateDate = (DateTime)reply.CreateDate,
                        Images = _DbContext.RatingImages
                            .Where(img => img.RateId == reply.RateId)
                            .Select(i => new ImageModel()
                            {
                                id = i.RatingImageId,
                                path = i.RatingImagePath
                            })
                            .ToList(),
                        UsersLike = reply.Interests
                            .Where(ul => ul.Liked == true)
                            .Select(ul => ul.User.UserFullName)
                            .ToList(),
                        UsersDislike = reply.Interests
                            .Where(ud => ud.Liked == false)
                            .Select(ud => ud.User.UserFullName)
                            .ToList(),
                    })
                    .ToList()
                })
                .ToListAsync();

                return new SuccessResponse<List<RateGetModel>>("success", rate);
            }
            catch (Exception e)
            {
                return new FailResponse<List<RateGetModel>>("Lỗi \n\n" + e.ToString());
            }
        }
        public async Task SaveChangesAsync() => await _DbContext.SaveChangesAsync();
        public async Task<Response<List<string>>> GetUsersFavor(UserFavorRequest request)
        {
            try
            {
                var userNames = await _interestRepo
                    .Query(item => item.RateId == request.id && item.Liked == request.liked)
                    .Select(item => item.User.UserFullName)
                    .ToListAsync();
                return new SuccessResponse<List<string>>("success", userNames);
            }
            catch(Exception e)
            {
                return new FailResponse<List<string>>("Lỗi \n\n" + e.ToString());
            }
        }
    }
}
