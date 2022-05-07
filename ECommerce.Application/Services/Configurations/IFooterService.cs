using ECommerce.Application.Common;
using ECommerce.Application.Services.Configurations.Dtos.Footer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public interface IFooterService
    {
        Task<ApiResponse> UpdateBlog(BlogUpdateRequest request);
        Task<ApiResponse> UpdateSocial(SocialUpdateRequest request);
        Task<List<SocialModel>> getAllSocial(); //1 cột 4 social
        Task<List<BlogModel>> getAllBlog(); // 2 cột, 8 blog
        Task<BlogModel> getBlogDetail(int BlogId);
    }
}
