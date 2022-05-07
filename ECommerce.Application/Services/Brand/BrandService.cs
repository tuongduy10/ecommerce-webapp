using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Brand
{
    public class BrandService : IBrandService
    {
        private readonly ECommerceContext DbContext;
        public BrandService(ECommerceContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public Task<int> Create(BrandCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(int BrandId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BrandModel>> getAll()
        {
            var query = from category in DbContext.Categories
                        join brand in DbContext.Brands on category.CategoryId equals brand.CategoryId
                        orderby brand.BrandName
                        where brand.Status == true
                        select new { brand, category};

            var list = await query.Select(i => new BrandModel()
            {
                BrandId = i.brand.BrandId,
                BrandName = i.brand.BrandName,
                BrandImagePath = i.brand.BrandImagePath,
                Status = i.brand.Status,
                CreatedDate = i.brand.CreatedDate,
                Highlights = i.brand.Highlights,
                New = i.brand.New,
                Category = i.category.CategoryName
            }).ToListAsync();

            return list;
        }
        public async Task<List<BrandModel>> getAllManage()
        {
            var query = from category in DbContext.Categories
                        join brand in DbContext.Brands on category.CategoryId equals brand.CategoryId
                        orderby brand.BrandName
                        select new { brand, category };

            var list = await query.Select(i => new BrandModel()
            {
                BrandId = i.brand.BrandId,
                BrandName = i.brand.BrandName,
                BrandImagePath = i.brand.BrandImagePath,
                Status = i.brand.Status,
                CreatedDate = i.brand.CreatedDate,
                Highlights = i.brand.Highlights,
                New = i.brand.New,
                Category = i.category.CategoryName
            }).ToListAsync();

            return list;
        }
        public async Task<List<BrandModel>> getAllBrandInCategory(int CategoryId)
        {
            var query = from category in DbContext.Categories
                        join brand in DbContext.Brands on category.CategoryId equals brand.CategoryId
                        orderby brand.BrandName
                        where brand.Status == true && category.CategoryId == CategoryId
                        select new { brand, category };

            var list = await query.Select(i => new BrandModel()
            {
                BrandId = i.brand.BrandId,
                BrandName = i.brand.BrandName,
                BrandImagePath = i.brand.BrandImagePath,
                Status = i.brand.Status,
                CreatedDate = i.brand.CreatedDate,
                Highlights = i.brand.Highlights,
                New = i.brand.New,
                Category = i.category.CategoryName
            }).ToListAsync();

            return list;
        }

        public async Task<BrandModel> getBrandById(int BrandId)
        {
            var query = from category in DbContext.Categories
                        join brand in DbContext.Brands on category.CategoryId equals brand.CategoryId
                        orderby brand.BrandName
                        where brand.Status == true && brand.BrandId == BrandId
                        select new { brand, category };

            var result = await query.Select(i => new BrandModel()
            {
                BrandId = i.brand.BrandId,
                BrandName = i.brand.BrandName,
                BrandImagePath = i.brand.BrandImagePath,
                Status = i.brand.Status,
                CreatedDate = i.brand.CreatedDate,
                Highlights = i.brand.Highlights,
                New = i.brand.New,
                Category = i.category.CategoryName
            }).SingleOrDefaultAsync();

            return result;
        }

        public Task<int> Update(BrandModel request)
        {
            throw new NotImplementedException();
        }
    }
}
