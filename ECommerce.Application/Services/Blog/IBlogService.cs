using ECommerce.Application.Services.Blog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Blog
{
    public interface IBlogService
    {
        Task<int> Create(BlogCreateRequest request);
        Task<int> Update(BlogModel request);
        Task<int> Delete(int BlogId);
        Task<List<BlogModel>> getAll();
    }
}
