using ECommerce.Application.Services.Category.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ECommerceContext _DbContext;
        public CategoryService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<int> Create(CategoryCreateRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int CategoryId)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Update(CategoryModel request)
        {
            throw new NotImplementedException();
        }
        public async Task<List<CategoryModel>> getAll()
        {
            var list = from c in _DbContext.Categories select c;

            return await list.Select(i => new CategoryModel()
            {
                CategoryId = i.CategoryId,
                CategoryName = i.CategoryName
            }).ToListAsync();
        }
    }
}
