using ECommerce.Application.Services.SubCategory.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.SubCategory
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ECommerceContext _DbContext;
        public SubCategoryService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<int> Create(SubCategoryCreateRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int CategoryId)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Update(SubCategoryCreateRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<List<SubCategoryModel>> getAll()
        {
            var list = from c in _DbContext.SubCategories 
                       select c;

            return await list.Select(i => new SubCategoryModel()
            {
                SubCategoryId = i.SubCategoryId,
                SubCategoryName = i.SubCategoryName,
                CategoryId = i.CategoryId,
            }).ToListAsync();
        }
        public async Task<List<SubCategoryModel>> getSubCategoryInBrand(int BrandId)
        {
            var query = from subc in _DbContext.SubCategories
                        from brand in _DbContext.Brands
                        where subc.CategoryId == brand.CategoryId && brand.BrandId == BrandId
                        select subc;
            var list = await query.Select(i => new SubCategoryModel() {
                SubCategoryId = i.SubCategoryId,
                SubCategoryName = i.SubCategoryName,
                CategoryId = i.CategoryId,
            }).ToListAsync();

            return list;
        }
    }
}
