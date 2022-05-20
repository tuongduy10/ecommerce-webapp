
using ECommerce.Application.Common;
using ECommerce.Application.Services.Rate.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Rate
{
    public interface IRateService
    {
        Task<List<RateGetModel>> getRatesByProductId(int id);
        Task<ApiResponse> postComment(PostCommentRequest request);
    }
}
