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
        Task<int> UpdateBlog(BlogUpdateRequest request);
        Task<int> UpdateSocial(SocialUpdateRequest request);
        Task<List<SocialModel>> getAllSocial();
        Task<List<BlogModel>> getAllBlog();
    }
}
