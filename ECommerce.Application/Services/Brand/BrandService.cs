using ECommerce.Application.Common;
using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
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
        private readonly ECommerceContext _DbContext;
        public BrandService(ECommerceContext DbContext)
        {
            this._DbContext = DbContext;
        }
        public async Task<ApiResponse> Create(BrandCreateRequest request)
        {
            if(string.IsNullOrEmpty(request.BrandImagePath) || string.IsNullOrEmpty(request.BrandName))
            {
                return new ApiFailResponse("Thông tin không được để trống");
            }
            try
            {
                // create new 
                var brand = new Data.Models.Brand
                {
                    BrandName = request.BrandName,
                    BrandImagePath = request.BrandImagePath,
                    CreatedDate = DateTime.Now,
                    Status = true,
                    CategoryId = request.CategoryId
                };

                await _DbContext.Brands.AddAsync(brand);
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Thêm thành công");
            }
            catch
            {
                return new ApiFailResponse("Thêm thất bại");
            }
        }
        public async Task<Response<FileChangedResponse>> Update(BrandUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.BrandName) || request.CategoryId == 0)
            {
                return new FailResponse<FileChangedResponse>("Thông tin không được để trống");
            }
            try
            {
                var brand = await _DbContext.Brands.Where(i => i.BrandId == request.BrandId).FirstOrDefaultAsync();
                var oldFileName = brand.BrandImagePath;
                
                if (request.BrandImagePath != null)
                {
                    brand.BrandImagePath = request.BrandImagePath;
                }
                brand.BrandName = request.BrandName;
                brand.Status = true;
                brand.CategoryId = request.CategoryId;
                await _DbContext.SaveChangesAsync();
                var fileChange = new FileChangedResponse
                {
                    previousFileName = oldFileName,
                    newFileName = brand.BrandImagePath
                };
                return new SuccessResponse<FileChangedResponse>("Cập nhật thành công", fileChange);
            }
            catch
            {
                return new FailResponse<FileChangedResponse>("Cập nhật thất bại");
            }
        }
        public Task<int> Delete(int BrandId)
        {
            throw new NotImplementedException();
        }
        public async Task<List<BrandModel>> getAll()
        {
            var query = from category in _DbContext.Categories
                        join brand in _DbContext.Brands on category.CategoryId equals brand.CategoryId
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
                Category = i.category.CategoryName,
                CategoryId = i.category.CategoryId
            }).ToListAsync();

            return list;
        }
        public async Task<List<BrandModel>> getAllManage()
        {
            var query = from category in _DbContext.Categories
                        join brand in _DbContext.Brands on category.CategoryId equals brand.CategoryId
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
                Category = i.category.CategoryName,
                CategoryId = i.category.CategoryId
            }).ToListAsync();

            return list;
        }
        public async Task<List<BrandModel>> getAllBrandInCategory(int CategoryId)
        {
            var query = from category in _DbContext.Categories
                        join brand in _DbContext.Brands on category.CategoryId equals brand.CategoryId
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
                Category = i.category.CategoryName,
                CategoryId = i.category.CategoryId
            }).ToListAsync();

            return list;
        }
        public async Task<BrandModel> getBrandById(int BrandId)
        {
            var query = from category in _DbContext.Categories
                        join brand in _DbContext.Brands on category.CategoryId equals brand.CategoryId
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
                Category = i.category.CategoryName,
                CategoryId = i.category.CategoryId
            }).SingleOrDefaultAsync();

            return result;
        }
    }
}
