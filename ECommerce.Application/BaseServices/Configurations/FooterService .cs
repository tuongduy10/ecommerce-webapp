using ECommerce.Application.Common;
using ECommerce.Application.BaseServices.Configurations.Dtos;
using ECommerce.Application.BaseServices.Configurations.Dtos.Footer;
using ECommerce.Data.Context;
using ECommerce.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Configurations
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
        public async Task<ApiResponse> AddBlog(BlogModel request)
        {
            if (request.BlogContent == null) return new ApiFailResponse("Nội dung không được để trống");
            if (request.BlogTitle == null) return new ApiFailResponse("Tiêu đề không được để trống");
            if (request.BlogPosition == null) return new ApiFailResponse("Vị trí không được để trống");

            Data.Entities.Blog blog = new Data.Entities.Blog();
            blog.BlogTitle = request.BlogTitle;
            blog.BlogContent = request.BlogContent;
            blog.BlogPosition = request.BlogPosition;
            blog.Status = 1;
            await _DbContext.Blogs.AddAsync(blog);
            _DbContext.SaveChangesAsync().Wait();

            return new ApiSuccessResponse("Thêm thành công");
        }
        public async Task<ApiResponse> UpdateBlog(BlogUpdateRequest request)
        {
            if (request.BlogContent == null) return new ApiFailResponse("Nội dung không được để trống");
            if (request.BlogTitle == null) return new ApiFailResponse("Tiêu đề không được để trống");
            if (request.BlogPosition == null) return new ApiFailResponse("Vị trí không được để trống");

            var blog = await _DbContext.Blogs
                                .Where(i => i.BlogId == request.BlogId)
                                .FirstOrDefaultAsync();
            if (blog != null)
            {
                blog.BlogContent = request.BlogContent;
                blog.BlogTitle = request.BlogTitle;
                blog.BlogPosition = request.BlogPosition;
                //blog.Status = request.Status;
                _DbContext.SaveChangesAsync().Wait();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            return new ApiFailResponse("Cập nhật không thành công");
        }

        public async Task<ApiResponse> DeleteBlog(int id)
        {
            var blog = await _DbContext.Blogs
                                .Where(i => i.BlogId == id)
                                .FirstOrDefaultAsync();
            if (blog != null)
            {
                _DbContext.Blogs.Remove(blog);
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Xóa thành công");
            }

            return new ApiFailResponse("Xóa không thành công");
        }
        public async Task<Response<BlogModel>> GetBlog(BlogDto request)
        {
            try
            {
                var blog = await _DbContext.Blogs
                    .Where(i => i.BlogId == request.id)
                    .Select(i => new BlogModel()
                    {
                        BlogId = i.BlogId,
                        BlogContent = i.BlogContent,
                        BlogPosition = i.BlogPosition,
                        BlogTitle = i.BlogTitle,
                        Type = request.type
                    })
                    .FirstOrDefaultAsync();
                if (blog == null) return new FailResponse<BlogModel>("Không tìm thấy");

                return new SuccessResponse<BlogModel>(blog);
            }
            catch (Exception error)
            {
                return new FailResponse<BlogModel>("");
            }
        }
        public async Task<ApiResponse> UpdateSocial(SocialUpdateRequest request)
        {
            var social = await _DbContext.Socials.Where(i => i.SocialId == request.SocialId).FirstOrDefaultAsync();
            if (social != null)
            {
                social.Icon = request.Icon;
                social.SocialName = request.SocialName;
                social.SocialUrl = request.SocialUrl;
                _DbContext.SaveChangesAsync().Wait();
                return new ApiSuccessResponse("Cập nhật thành công");
            }

            return new ApiFailResponse("Cập nhật không thành công");
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
