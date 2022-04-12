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

        public async Task<int> UpdateBlog(BlogUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateSocial(SocialUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
