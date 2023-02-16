
using ECommerce.Application.Common;
using ECommerce.Application.Services.Rate.Dtos;
using ECommerce.Application.Services.Rate.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Rate
{
    public interface IRateService
    {
        Task<List<RateGetModel>> GetAll();
        Task<List<RateGetModel>> GetAllByParentId(int id);
        Task<List<RateGetModel>> GetAllToDay();
        Task<List<RateGetModel>> GetRatesByProductId(int id, int userId = 0);
        Task<List<RateGetModel>> GetAllByUserId(int id);
        Task<ApiResponse> PostComment(PostCommentRequest request);
        Task<ApiResponse> ReplyComment(ReplyCommentRequest request);
        Task<Response<LikeAndDislike>> LikeComment(LikeAndDislikeCount request);
        Task<Response<List<string>>> UpdateComment(UpdateCommentRequest request);
        Task<Response<List<string>>> DeleteComment(int id);
        Task<Response<List<string>>> DeleteComments(List<int> ids);
        Task<Response<List<string>>> DeleteImages(List<int> id);
        Task<ApiResponse> deleteCommentByProductId(int id);
        Task<Response<List<string>>> DeleteCommentsByProductIds(List<int> ids);
    }
}
