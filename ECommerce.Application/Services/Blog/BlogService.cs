using ECommerce.Application.Services.Blog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Blog
{
    public class BlogService : IBlogService
    {
        public async Task<int> Create(BlogCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(int BlogId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BlogModel>> getAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(BlogModel request)
        {
            throw new NotImplementedException();
        }
    }
}
