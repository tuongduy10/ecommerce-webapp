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

            //var checkProduct = _DbContext.Products.Where(i => i.SubCategoryId == 1).Count();
            //var HasProducts = checkProduct > 0 ? true : false;

            var list = await query.Select(i => new SubCategoryModel() {
                SubCategoryId = i.SubCategoryId,
                SubCategoryName = i.SubCategoryName,
                CategoryId = i.CategoryId,
            }).ToListAsync();

            return list;
        }
        public async Task<List<SubCategoryModel>> getSubCategoryByUser(int userId)
        {
            // get brandids by shop(userid)
            var brandIds = await _DbContext.ShopBrands
                .Where(i => i.Shop.UserId == userId)
                .Select(i => i.BrandId)
                .Distinct()
                .ToListAsync();

            // get categoryids by brandids
            var categoryIds = new List<int>();
            foreach (var id in brandIds)
            {
                var categoryId = await _DbContext.Brands
                    .Where(i => i.BrandId == id)
                    .Select(i => i.CategoryId)
                    .ToListAsync();
                categoryIds.AddRange(categoryId);
            }
            categoryIds = categoryIds.Distinct().ToList();

            // get subcategoryids by categoryids
            var subcIds = new List<int>();
            foreach (var id in categoryIds)
            {
                var subcId = await _DbContext.SubCategories
                    .Where(i => i.CategoryId == id)
                    .Select(i => i.SubCategoryId)
                    .ToListAsync();
                subcIds.AddRange(subcId);
            }
            subcIds = subcIds.Distinct().ToList();

            // get subcategory by subcategoryids
            var result = new List<SubCategoryModel>();
            foreach (var id in subcIds)
            {
                var sub = await _DbContext.SubCategories
                    .Where(i => i.SubCategoryId == id)
                    .Select(i => new SubCategoryModel()
                    {
                        SubCategoryId = i.SubCategoryId,
                        SubCategoryName = i.SubCategoryName,
                    })
                    .FirstOrDefaultAsync();
                result.Add(sub);
            }

            return result;
        }
        public async Task<List<SubCategoryModel>> getSubCategoryByCategoryId(int id)
        {
            var result = await _DbContext.SubCategories
                .Where(i => i.CategoryId == id)
                .Select(i => new SubCategoryModel() { 
                    SubCategoryId = i.SubCategoryId, 
                    SubCategoryName = i.SubCategoryName 
                })
                .ToListAsync();
            return result;
        }
        public async Task<List<string>> getOptionBySubCategoryId(int id)
        {
            var result = await _DbContext.SubCategoryOptions
                .Where(i => i.SubCategoryId == id)
                .Select(i => i.Option.OptionName)
                .ToListAsync();

            return result;
        }
        public async Task<List<string>> getAttributeBySubCategoryId(int id)
        {
            var result = await _DbContext.SubCategoryAttributes
                .Where(i => i.SubCategoryId == id)
                .Select(i => i.Attribute.AttributeName)
                .ToListAsync();

            return result;
        }
    }
}
