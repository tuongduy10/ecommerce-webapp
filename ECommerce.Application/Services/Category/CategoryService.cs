using ECommerce.Application.Common;
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
        public async Task<ApiResponse> Create(CategoryCreateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CategoryName)) return new ApiFailResponse("Không được để trống");
                
                var check = await _DbContext.Categories.Where(i => i.CategoryName == request.CategoryName.Trim()).ToListAsync();
                if (check.Count > 0) return new ApiFailResponse("Danh mục này đã tồn tại");

                var category = new Data.Models.Category()
                {
                    CategoryName = request.CategoryName.Trim()
                };
                await _DbContext.Categories.AddAsync(category);
                await _DbContext.SaveChangesAsync();

                return new ApiSuccessResponse("Thêm thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Fail" + error.Message);
            }
        }
        public async Task<ApiResponse> Delete(int categoryId)
        {
            try
            {
                var subcategories = await _DbContext.SubCategories.Where(i => i.CategoryId == categoryId).ToListAsync();
                var hasSubcategory = subcategories.Count > 0;
                if (hasSubcategory) return new ApiFailResponse($"Danh mục này tồn tại {subcategories.Count} danh mục sản phẩm");

                var brands = await _DbContext.Brands.Where(i => i.CategoryId == categoryId).ToListAsync();
                var hasShop = brands.Count > 0;
                if (hasShop) return new ApiFailResponse($"Đang có {brands.Count} thương hiệu hoạt động trên danh mục này, không thể xóa");

                var category = await _DbContext.Categories.Where(i => i.CategoryId == categoryId).FirstOrDefaultAsync();
                _DbContext.Categories.Remove(category);
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Xóa thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Xóa thất bại: " + error.Message);
            }
        }
        public async Task<ApiResponse> Update(CategoryModel request)
        {
            var category = await _DbContext.Categories
                .Where(i => i.CategoryId == request.CategoryId)
                .FirstOrDefaultAsync();

            if (category == null) return new ApiFailResponse("Không tìm thấy danh mục");
            if (string.IsNullOrEmpty(request.CategoryName)) return new ApiFailResponse("Tên không được để trống");

            category.CategoryName = request.CategoryName.Trim();
            await _DbContext.SaveChangesAsync();

            return new ApiSuccessResponse("Cập nhật thành công");
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
        public async Task<CategoryModel> Detail(int id)
        {
            var category = await _DbContext.Categories
                .Where(i => i.CategoryId == id)
                .Select(i => new CategoryModel
                {
                    CategoryId = i.CategoryId,
                    CategoryName = i.CategoryName
                })
                .FirstOrDefaultAsync();

            if (category == null) return null;

            return category;
        }
    }
}
