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
        private readonly ECommerceContext DbContext;
        public CategoryService(ECommerceContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public async Task<int> Create(CategoryCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryViewModel>> getAll()
        {
            var list = from c in DbContext.Categories select c;

            return await list.Select(i => new CategoryViewModel()
            {
                CategoryId = i.CategoryId,
                CategoryName = i.CategoryName
            }).ToListAsync();
        }

        public async Task<List<CategoryViewModel>> getAllByBrand(int BrandId, int pageindex, int pagesize)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryViewModel>> getAllByShop(int ShopId, int pageindex, int pagesize)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryViewModel>> getAllBySubCategory(int SubCategoryId, int pageindex, int pagesize)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(CategoryCreateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
