
using ECommerce.Application.Common;
using ECommerce.Application.Constants;
using ECommerce.Application.Repositories.Notification.Dtos;
using ECommerce.Application.Repositories.Notification.Enums;
using ECommerce.Application.BaseServices.Rate.Dtos;
using ECommerce.Application.BaseServices.Rate.Models;
using ECommerce.Application.BaseServices.User;
using ECommerce.Application.BaseServices.User.Enums;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Rate
{
    public class RateService : IRateService
    {
        private ECommerceContext _DbContext;
        public RateService(
            ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<List<RateGetModel>> GetAll()
        {
            var rate = await _DbContext.Rates
                .Select(rate => new RateGetModel()
                {
                    Id = rate.RateId,
                    ProductName = rate.Product.ProductName,
                    ProductId = (int)rate.ProductId,
                    ShopName = rate.Product.Shop.ShopName,
                    Value = (int)rate.RateValue,
                    UserName = rate.User.UserFullName,
                    Comment = rate.Comment,
                    CreateDate = (DateTime)rate.CreateDate,
                    Images = _DbContext.RatingImages
                        .Where(img => img.RateId == rate.RateId)
                        .Select(i => new ImageModel() { 
                            id = i.RatingImageId,
                            path = i.RatingImagePath,
                        })
                        .ToList(),
                    ParentId = rate.ParentId == null ? 0 : (int)rate.ParentId,
                    ParentCreateDate = ((DateTime)_DbContext.Rates
                        .Where(reply => reply.RateId == rate.ParentId)
                        .Select(reply => reply.CreateDate)
                        .FirstOrDefault())
                        .ToString(ConfigConstant.DATE_FORMAT),
                    Replies = _DbContext.Rates
                        .Where(reply => reply.ParentId == rate.RateId)
                        .Select(reply => new RateGetModel())
                        .ToList()
                })
                .OrderByDescending(i => i.CreateDate)
                .ToListAsync();

            return rate;
        }
        public async Task<List<RateGetModel>> GetAllByParentId(int id)
        {
            var rate = await _DbContext.Rates
                .Where(rate => rate.ParentId == id)
                .Select(rate => new RateGetModel()
                {
                    Id = rate.RateId,
                    ProductName = rate.Product.ProductName,
                    ProductId = (int)rate.ProductId,
                    ShopName = rate.Product.Shop.ShopName,
                    Value = (int)rate.RateValue,
                    UserName = rate.User.UserFullName,
                    Comment = rate.Comment,
                    CreateDate = (DateTime)rate.CreateDate,
                    Images = _DbContext.RatingImages
                        .Where(img => img.RateId == rate.RateId)
                        .Select(i => new ImageModel() { 
                            id = i.RatingImageId,
                            path = i.RatingImagePath
                        })
                        .ToList(),
                    LikeCount = rate.Interests.Where(lc => lc.Liked == true).Count(),
                    DislikeCount = rate.Interests.Where(lc => lc.Liked == false).Count()
                })
                .OrderByDescending(i => i.CreateDate)
                .ToListAsync();

            return rate;
        }
        public async Task<List<RateGetModel>> GetAllToDay() 
        {
            var rate = await _DbContext.Rates
                .Where(rate => rate.ParentId == null)
                .Select(rate => new RateGetModel()
                {
                    Id = rate.RateId,
                    ProductName = rate.Product.ProductName,
                    ProductId = (int)rate.ProductId,
                    ShopName = rate.Product.Shop.ShopName,
                    Value = (int)rate.RateValue,
                    UserName = rate.User.UserFullName,
                    Comment = rate.Comment,
                    CreateDate = (DateTime)rate.CreateDate,
                    Images = _DbContext.RatingImages
                        .Where(img => img.RateId == rate.RateId)
                        .Select(i => new ImageModel() { 
                            id = i.RatingImageId,
                            path = i.RatingImagePath
                        })
                        .ToList()
                })
                .OrderByDescending(i => i.CreateDate)
                .ToListAsync();

            var today = DateTime.Today.Date.ToString("dd/MM/yyyy");
            rate = rate.Where(i => i.CreateDate.Date.ToString("dd/MM/yyyy") == today).ToList();

            return rate;
        }
        public async Task<List<RateGetModel>> GetRatesByProductId(int id, int _userId = 0)
        {
            var rate = await _DbContext.Rates
                .Where(rate => rate.ProductId == id && rate.ParentId == null)
                .Select(rate => new RateGetModel(){
                    Id = rate.RateId,
                    Value = (int)rate.RateValue,
                    UserId = rate.User.UserId,
                    UserName = rate.User.UserFullName,
                    Comment = rate.Comment,
                    CreateDate = (DateTime)rate.CreateDate,
                    Images = _DbContext.RatingImages
                        .Where(img => img.RateId == rate.RateId)
                        .Select(i => new ImageModel() { 
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
                    Replies = _DbContext.Rates
                    .Where(reply => reply.ParentId == rate.RateId)
                    .Select(reply => new RateGetModel() { 
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
                        IsSeller =  _DbContext.UserRoles
                            .Where(isa => isa.UserId == reply.User.UserId)
                            .Select(isa => isa.Role.RoleName)
                            .ToList()
                            .Contains(RoleName.Seller),
                        IsShopOwner = _DbContext.Products
                            .Where(iso => iso.ProductId == id)
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
                            .Select(i => new ImageModel() { 
                                id = i.RatingImageId,
                                path = i.RatingImagePath
                            })
                            .ToList(),
                    })
                    .ToList()
                })
                .ToListAsync();

            return rate;
        }
        public async Task<List<RateGetModel>> GetAllByUserId(int _userId)
        {
            var rate = await _DbContext.Rates
                .Where(rate => rate.UserRepliedId == _userId)
                .Select(rate => new RateGetModel()
                {
                    Id = rate.RateId,
                    Value = (int)rate.RateValue,
                    UserId = rate.User.UserId,
                    UserName = rate.User.UserFullName,
                    Comment = rate.Comment,
                    CreateDate = (DateTime)rate.CreateDate,
                    ProductId = (int)rate.ProductId,
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
                    })
                    .ToList()
                })
                .OrderByDescending(i => i.CreateDate)
                .ToListAsync();

            return rate;
        }
        public async Task<ApiResponse> ReplyComment(ReplyCommentRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.comment)) return new ApiFailResponse("Nội dung không được để trống");

                string idsToDelete = null;
                var replied = await _DbContext.Rates.Where(i => i.RateId == request.repliedId).FirstOrDefaultAsync();
                if (replied != null)
                {
                    idsToDelete = string.IsNullOrEmpty(replied.IdsToDelete) ? request.repliedId.ToString() : (replied.IdsToDelete + "," + replied.RateId);
                }

                // Add comment content
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
                await _DbContext.Rates.AddAsync(comment);
                await _DbContext.SaveChangesAsync();

                // Add image
                if (request.fileNames != null)
                {
                    foreach (var filename in request.fileNames)
                    {
                        var image = new Data.Models.RatingImage()
                        {
                            RateId = comment.RateId,
                            RatingImagePath = filename,
                        };
                        await _DbContext.RatingImages.AddAsync(image);
                    }
                    await _DbContext.SaveChangesAsync();
                }

                // Notification
                var notification = new Data.Models.Notification()
                {
                    TextContent = comment.Comment,
                    IsRead = false,
                    ReceiverId = comment.UserRepliedId,
                    SenderId = comment.UserId,
                    JsLink = $"/Product/ProductDetail?ProductId={comment.ProductId}&isScrolledTo=true&commentId={comment.RateId}",
                    TypeId = (int?)NotificationType.Comment,
                };
                await _DbContext.Notifications.AddAsync(notification);
                await _DbContext.SaveChangesAsync();

                return new ApiSuccessResponse("Phản hồi thành công");
            }
            catch(Exception error)
            {
                return new ApiFailResponse("Phản hồi thất bại \n\n" + error.ToString());
            }
            
        }
        public async Task<Response<LikeAndDislike>> LikeComment(LikeAndDislikeCount request)
        {
            try
            {
                var currObj = await _DbContext.Interests
                .Where(i => i.UserId == request.userId && i.RateId == request.rateId)
                .FirstOrDefaultAsync();

                int rateId = 0;
                if (currObj == null)
                {
                    var _obj = new Data.Models.Interest
                    {
                        RateId = request.rateId,
                        UserId = request.userId,
                        Liked = request.liked
                    };
                    await _DbContext.Interests.AddAsync(_obj);
                    rateId = _obj.RateId;
                }
                else
                {
                    if (currObj.Liked == request.liked) currObj.Liked = null;
                    else currObj.Liked = request.liked;
                    
                    rateId = currObj.RateId;
                }
                await _DbContext.SaveChangesAsync();
                var interest = await this.LikeAndDislikeCount(rateId);

                return new SuccessResponse<LikeAndDislike>("Đánh giá thành công", interest);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        public async Task<LikeAndDislike> LikeAndDislikeCount(int rateId)
        {
            try
            {
                var interestObj = await _DbContext.Interests
                    .Where(i => i.RateId == rateId)
                    .ToListAsync();

                var result = new LikeAndDislike
                {
                    RateId = rateId,
                    LikeCount = interestObj == null ? 0 : interestObj.Where(i => i.Liked == true).Count(),
                    DislikeCount = interestObj == null ? 0 : interestObj.Where(i => i.Liked == false).Count()
                };
                 
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<ApiResponse> PostComment(PostCommentBaseRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.comment)) return new ApiFailResponse("Nội dung không được để trống");

                // Check if user haven't any order with this product
                // get all order with productId and userId
                //var productInOrders = await _DbContext.OrderDetails
                //    .Where(i => i.ProductId == request.productId && i.Order.UserId == request.userId)
                //    .Select(i => i.OrderId)
                //    .ToListAsync();
                //var hasProduct = productInOrders.Count > 0;
                //if (!hasProduct) return new ApiFailResponse("Bạn cần mua sản phẩm này để đánh giá");

                // Check if user's shop is selling this product
                // get user's shop by product id
                var shops = await _DbContext.Products
                    .Where(i => i.ProductId == request.productId && i.Shop.UserId == request.userId)
                    .FirstOrDefaultAsync();
                var isOwner = shops != null;
                if (isOwner) return new ApiFailResponse("Bạn không thể đánh giá sản phẩm của mình");

                // Add comment content
                var comment = new Data.Models.Rate()
                {
                    Comment = request.comment,
                    RateValue = request.value,
                    ProductId = request.productId,
                    UserId = request.userId,
                    CreateDate = DateTime.Now,
                };
                await _DbContext.Rates.AddAsync(comment);
                await _DbContext.SaveChangesAsync();

                // Add image
                if (request.fileNames != null)
                {
                    foreach (var filename in request.fileNames)
                    {
                        var image = new Data.Models.RatingImage()
                        {
                            RateId = comment.RateId,
                            RatingImagePath = filename,
                        };
                        await _DbContext.RatingImages.AddAsync(image);
                    }
                    await _DbContext.SaveChangesAsync();
                }
                return new ApiSuccessResponse("Đã đánh giá sản phẩm");
            }
            catch
            {
                return new ApiFailResponse("Đánh giá thất bại");
            }
        }
        public async Task<Response<List<string>>> UpdateComment(UpdateCommentRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.content))
                    return new FailResponse<List<string>>("Nội dung không được để trống"); 

                var comment = await _DbContext.Rates
                    .Where(rate => rate.RateId == request.id)
                    .FirstOrDefaultAsync();

                if (comment == null)
                    return new FailResponse<List<string>>("Không tìm thấy bình luận hoặc bình luận đã bị xóa");

                comment.Comment = request.content;
                comment.RateValue = request.rateValue;

                // Add images
                if (request.fileNames != null)
                {
                    foreach (var filename in request.fileNames)
                    {
                        var image = new Data.Models.RatingImage()
                        {
                            RateId = comment.RateId,
                            RatingImagePath = filename,
                        };
                        await _DbContext.RatingImages.AddAsync(image);
                    }
                }

                await _DbContext.SaveChangesAsync();
                return new SuccessResponse<List<string>>("Cập nhật thành công");
            }
            catch (Exception e) 
            {
                return new FailResponse<List<string>>(e.ToString());
            }
        }
        public async Task<Response<List<string>>> DeleteComment(int id)
        {
            try
            {
                List<string> deleteImages = new List<string>();
                List<int> deleteNotiIds = new List<int>();

                var imagesStr = await _DbContext.RatingImages
                    .Where(i => i.RateId == id)
                    .Select(i => i.RatingImagePath)
                    .ToListAsync();
                deleteImages.AddRange(imagesStr);

                var comment = await _DbContext.Rates.Where(i => i.RateId == id).FirstOrDefaultAsync();
                if (comment != null)
                    _DbContext.Rates.Remove(comment);
                var commentImages = await _DbContext.RatingImages.Where(i => i.RateId == id).ToListAsync();
                if (commentImages != null)
                    _DbContext.RatingImages.RemoveRange(commentImages);
                var interests = await _DbContext.Interests.Where(i => i.RateId == id).ToListAsync();
                if (interests != null)
                    _DbContext.Interests.RemoveRange(interests);

                deleteNotiIds.Add(comment.RateId);

                // Delete by parentId
                var replies = await _DbContext.Rates.Where(i => i.ParentId == comment.RateId).ToListAsync();
                if (replies != null)
                {
                    foreach (var reply in replies)
                    {
                        var imgStr = await _DbContext.RatingImages
                            .Where(i => i.RateId == reply.RateId)
                            .Select(i => i.RatingImagePath)
                            .ToListAsync();
                        deleteImages.AddRange(imgStr);

                        var replyImages = await _DbContext.RatingImages.Where(i => i.RateId == reply.RateId).ToListAsync();
                        if (replyImages != null)
                            _DbContext.RatingImages.RemoveRange(replyImages);
                        var replyInterests = await _DbContext.Interests.Where(i => i.RateId == reply.RateId).ToListAsync();
                        if (replyInterests != null)
                            _DbContext.Interests.RemoveRange(replyInterests);

                        deleteNotiIds.Add(reply.RateId);
                    }
                    _DbContext.Rates.RemoveRange(replies);
                }

                // Delete replied
                var repliesByRepliedId = await _DbContext.Rates
                    .Where(i => i.IdsToDelete != null && i.IdsToDelete.Contains(comment.RateId.ToString()))
                    .ToListAsync();
                if (repliesByRepliedId != null)
                {
                    foreach (var reply in repliesByRepliedId)
                    {
                        var imgStr = await _DbContext.RatingImages
                            .Where(i => i.RateId == reply.RateId)
                            .Select(i => i.RatingImagePath)
                            .ToListAsync();
                        deleteImages.AddRange(imgStr);

                        var replyImages = await _DbContext.RatingImages.Where(i => i.RateId == reply.RateId).ToListAsync();
                        if (replyImages != null)
                            _DbContext.RatingImages.RemoveRange(replyImages);
                        var replyInterests = await _DbContext.Interests.Where(i => i.RateId == reply.RateId).ToListAsync();
                        if (replyInterests != null)
                            _DbContext.Interests.RemoveRange(replyInterests);

                        deleteNotiIds.Add(reply.RateId);
                    }
                    _DbContext.Rates.RemoveRange(repliesByRepliedId);
                }

                // Notifications
                var notifications = await _DbContext.Notifications
                    .Where(item => deleteNotiIds.Contains((int)item.InfoId))
                    .ToListAsync();
                if (notifications != null && notifications.Count > 0)
                    _DbContext.Notifications.RemoveRange(notifications);
                
                await _DbContext.SaveChangesAsync();


                return new SuccessResponse<List<string>>("Xóa thành công", deleteImages);
            }
            catch (Exception error)
            {
                return new FailResponse<List<string>>("Xóa không thành công: \n\n" + error.ToString());
            }
        }
        public async Task<Response<List<string>>> DeleteComments(List<int> ids)
        {
            try
            {
                List<string> deleteImages = new List<string>();
                List<int> deleteNotiIds = new List<int>();

                var imagesStr = await _DbContext.RatingImages
                    .Where(i => ids.Contains((int)i.RateId))
                    .Select(i => i.RatingImagePath)
                    .ToListAsync();
                deleteImages.AddRange(imagesStr);

                var comment = await _DbContext.Rates.Where(i => ids.Contains((int)i.RateId)).FirstOrDefaultAsync();
                if (comment != null)
                    _DbContext.Rates.Remove(comment);
                var commentImages = await _DbContext.RatingImages.Where(i => ids.Contains((int)i.RateId)).ToListAsync();
                if (commentImages != null)
                    _DbContext.RatingImages.RemoveRange(commentImages);
                var interests = await _DbContext.Interests.Where(i => ids.Contains((int)i.RateId)).ToListAsync();
                if (interests != null)
                    _DbContext.Interests.RemoveRange(interests);

                deleteNotiIds.Add(comment.RateId);

                // Delete by parentId
                var replies = await _DbContext.Rates.Where(i => i.ParentId == comment.RateId).ToListAsync();
                if (replies != null)
                {
                    foreach (var reply in replies)
                    {
                        var imgStr = await _DbContext.RatingImages
                            .Where(i => i.RateId == reply.RateId)
                            .Select(i => i.RatingImagePath)
                            .ToListAsync();
                        deleteImages.AddRange(imgStr);

                        var replyImages = await _DbContext.RatingImages.Where(i => i.RateId == reply.RateId).ToListAsync();
                        if (replyImages != null)
                            _DbContext.RatingImages.RemoveRange(replyImages);
                        var replyInterests = await _DbContext.Interests.Where(i => i.RateId == reply.RateId).ToListAsync();
                        if (replyInterests != null)
                            _DbContext.Interests.RemoveRange(replyInterests);

                        deleteNotiIds.Add(reply.RateId);
                    }
                    _DbContext.Rates.RemoveRange(replies);
                }

                // Delete replied
                var repliesByRepliedId = await _DbContext.Rates
                    .Where(i => i.IdsToDelete != null && i.IdsToDelete.Contains(comment.RateId.ToString()))
                    .ToListAsync();
                if (repliesByRepliedId != null)
                {
                    foreach (var reply in repliesByRepliedId)
                    {
                        var imgStr = await _DbContext.RatingImages
                            .Where(i => i.RateId == reply.RateId)
                            .Select(i => i.RatingImagePath)
                            .ToListAsync();
                        deleteImages.AddRange(imgStr);

                        var replyImages = await _DbContext.RatingImages.Where(i => i.RateId == reply.RateId).ToListAsync();
                        if (replyImages != null)
                            _DbContext.RatingImages.RemoveRange(replyImages);
                        var replyInterests = await _DbContext.Interests.Where(i => i.RateId == reply.RateId).ToListAsync();
                        if (replyInterests != null)
                            _DbContext.Interests.RemoveRange(replyInterests);

                        deleteNotiIds.Add(reply.RateId);
                    }
                    _DbContext.Rates.RemoveRange(repliesByRepliedId);
                }

                // Notifications
                var notifications = await _DbContext.Notifications
                    .Where(item => deleteNotiIds.Contains((int)item.InfoId))
                    .ToListAsync();
                if (notifications != null && notifications.Count > 0)
                    _DbContext.Notifications.RemoveRange(notifications);

                await _DbContext.SaveChangesAsync();


                return new SuccessResponse<List<string>>("Xóa thành công", deleteImages);
            }
            catch (Exception error)
            {
                return new FailResponse<List<string>>("Xóa không thành công: \n\n" + error);
            }
        }
        public async Task<Response<List<string>>> DeleteImages(List<int> id)
        {
            try
            {
                var imagesStr = await _DbContext.RatingImages
                    .Where(i => id.Contains((int)i.RatingImageId))
                    .Select(i => i.RatingImagePath)
                    .ToListAsync();

                var images = await _DbContext.RatingImages
                    .Where(i => id.Contains((int)i.RatingImageId))
                    .ToListAsync();

                if (images != null)
                    _DbContext.RatingImages.RemoveRange(images);
                await _DbContext.SaveChangesAsync();

                return new SuccessResponse<List<string>>("Xóa thành công", imagesStr);
            }
            catch (Exception error)
            {
                return new FailResponse<List<string>>(error.ToString());
            }
        }
        public async Task<ApiResponse> deleteCommentByProductId(int id)
        {
            try
            {
                var comments = await _DbContext.Rates.Where(i => i.ProductId == id).ToListAsync();
                if (comments.Count == 0)
                    return new ApiFailResponse("Không có bình luận để xóa");
                
                var commentIds = comments.Select(i => i.RateId).ToList();
                
                // Images
                var images = await _DbContext.RatingImages.Where(i => commentIds.Contains((int)i.RateId)).ToListAsync();
                if (images.Count > 0)
                    _DbContext.RatingImages.RemoveRange(images);
                // Like/dislike
                var interests = await _DbContext.Interests.Where(i => commentIds.Contains(i.RateId)).ToListAsync();
                if (interests.Count > 0)
                    _DbContext.Interests.RemoveRange(interests);

                _DbContext.Rates.RemoveRange(comments);
                _DbContext.SaveChangesAsync().Wait();

                return new ApiSuccessResponse("Xóa thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Xóa không thành công: " + error);
            }
        }
        public async Task<Response<List<string>>> DeleteCommentsByProductIds(List<int> ids)
        {
            try
            {
                var comments = await _DbContext.Rates.Where(i => ids.Contains((int)i.ProductId)).ToListAsync();
                if (comments.Count == 0)
                    return new FailResponse<List<string>>("Xóa không có bình luận để xóa");

                var commentIds = comments.Select(i => i.RateId).ToList();
                var result = await DeleteComments(commentIds);
                if(result.isSucceed)
                    return new SuccessResponse<List<string>>("Xóa thành công", result.Data);

                return new SuccessResponse<List<string>>("Xóa không thành công: " + result.Message);
            }
            catch (Exception error)
            {
                return new FailResponse<List<string>>("Xóa không thành công: " + error.ToString());
            }
        }
    }
}
