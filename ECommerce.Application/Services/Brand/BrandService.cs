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

        public async Task<List<BrandViewModel>> getAll()
        {
            var query = from category in DbContext.Categories
                        join brand in DbContext.Brands on category.CategoryId equals brand.CategoryId
                        where brand.Status == true
                        select new { brand, category};

            var list = await query.Select(i => new BrandViewModel()
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

        public Task<int> Update(BrandCreateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
