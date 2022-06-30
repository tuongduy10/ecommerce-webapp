
using ECommerce.Application.Common;
using ECommerce.Application.Services.Rate.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Rate
{
    public class RateService : IRateService
    {
        private ECommerceContext _DbContext;
        public RateService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<List<RateGetModel>> getRatesByProductId(int id)
        {
            var rate = await _DbContext.Rates
                .Where(rate => rate.ProductId == id)
                .Select(rate => new RateGetModel(){
                    Value = (int)rate.RateValue,
                    UserName = rate.User.UserFullName,
                    Comment = rate.Comment,
                    CreateDate = (DateTime)rate.CreateDate,
                    Images = _DbContext.RatingImages.Where(img => img.RateId == rate.RateId).Select(i=>i.RatingImagePath).ToList()
                })
                .ToListAsync();

            return rate;
        }
        public async Task<ApiResponse> postComment(PostCommentRequest request)
        {
            try
            {
                if (request.files.Count > 3) return new ApiFailResponse("Ảnh không được vượt quá 3");
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
                foreach (var filename in request.fileNames)
                {
                    var image = new Data.Models.RatingImage()
                    {
                        RateId = comment.RateId,
                        RatingImagePath = filename,
                    };
                    await _DbContext.RatingImages.AddAsync(image);
                    await _DbContext.SaveChangesAsync();
                }
                return new ApiSuccessResponse("Đã đánh giá sản phẩm");
            }
            catch
            {
                return new ApiFailResponse("Đánh giá thất bại");
            }
        }
        public async Task<ApiResponse> deleteComment(int id)
        {
            try
            {
                var commentImages = await _DbContext.RatingImages.Where(i => i.RateId == id).ToListAsync();
                if (commentImages != null) _DbContext.RatingImages.RemoveRange(commentImages);

                var comment = await _DbContext.Rates.Where(i => i.RateId == id).FirstOrDefaultAsync();
                if (comment != null) _DbContext.Rates.Remove(comment);

                await _DbContext.SaveChangesAsync();

                return new ApiSuccessResponse("Xóa thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Xóa không thành công: " + error);
            }
        }
        public async Task<ApiResponse> deleteCommentByProductId(int id)
        {
            try
            {
                var comments = await _DbContext.Rates.Where(i => i.ProductId == id).ToListAsync();
                if (comments.Count > 0)
                {
                    var commentIds = comments.Select(i => i.RateId).ToList();
                    if (commentIds.Count > 0)
                    {
                        foreach (var commentId in commentIds)
                        {
                            var images = await _DbContext.RatingImages.Where(i => i.RateId == commentId).ToListAsync();
                            _DbContext.RatingImages.RemoveRange(images);
                        }
                    }
                    _DbContext.Rates.RemoveRange(comments);
                }
                
                await _DbContext.SaveChangesAsync();

                return new ApiSuccessResponse("Xóa thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Xóa không thành công: " + error);
            }
        }
    }
}
