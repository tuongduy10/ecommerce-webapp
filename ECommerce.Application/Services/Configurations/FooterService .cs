using ECommerce.Application.Common;
using ECommerce.Application.Services.Configurations.Dtos;
using ECommerce.Application.Services.Configurations.Dtos.Footer;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public class FooterService : IFooterService
    {
        private readonly ECommerceContext _DbContext;
        public FooterService(ECommerceContext db)
        {
            _DbContext = db;
        }

        public async Task<List<BlogModel>> getAllBlog()
        {
            var query = from blog in _DbContext.Blogs
                        where blog.Status == 1
                        select blog;

            var list = await query.Select(b => new BlogModel()
            {
                BlogId = b.BlogId,
                BlogContent = b.BlogContent,
                BlogTitle = b.BlogTitle,
                BlogPosition = b.BlogPosition,
            }).ToListAsync();

            return list;
        }

        public async Task<List<SocialModel>> getAllSocial()
        {
            var query = from social in _DbContext.Socials
                        where social.Status == 1
                        select social;

            var list = await query.Select(s => new SocialModel()
            {
                SocialId = s.SocialId,
                Position = s.Position,
                SocialName = s.SocialName,
                SocialUrl = s.SocialUrl,
                Icon = s.Icon,
            }).ToListAsync();

            return list;
        }

        public async Task<ApiResponse> UpdateBlog(BlogUpdateRequest request)
        {
            var blog = await _DbContext.Blogs
                                .Where(i => i.BlogId == request.BlogId)
                                .FirstOrDefaultAsync();
            if (blog != null)
            {
                blog.BlogContent = request.BlogContent;
                blog.BlogTitle = request.BlogTitle;
                blog.BlogPosition = request.BlogPosition;
                blog.Status = request.Status;
                _DbContext.SaveChangesAsync().Wait();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            return new ApiFailResponse("Cập nhật không thành công");
        }

        public async Task<int> UpdateSocial(SocialUpdateRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<BlogModel> getBlogDetail(int BlogId)
        {
            var blog = await _DbContext.Blogs
                                .Where(i => i.BlogId == BlogId)
                                .Select(i => new BlogModel()
                                {
                                    BlogId = i.BlogId,
                                    BlogContent = i.BlogContent,
                                    BlogPosition = i.BlogPosition,
                                    BlogTitle = i.BlogTitle,
                                })
                                .FirstOrDefaultAsync();
            return blog;
        }
    }
}
