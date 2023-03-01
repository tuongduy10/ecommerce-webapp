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
            try
            {
                if (string.IsNullOrEmpty(request.BrandImagePath)) return new ApiFailResponse("Vui lòng chọn ảnh");
                if (string.IsNullOrEmpty(request.BrandName)) return new ApiFailResponse("Tên không được để trống");
                if (request.CategoryIds == null || request.CategoryIds.Count == 0) return new ApiFailResponse("Vui lòng chọn danh mục"); 

                var name = await _DbContext.Brands.Where(i => i.BrandName == request.BrandName.Trim()).ToListAsync();
                if (name.Count() > 0) return new ApiFailResponse("Tên đã tồn tại");

                // create new brand
                var brand = new Data.Models.Brand
                {
                    BrandName = request.BrandName.Trim(),
                    BrandImagePath = request.BrandImagePath.Trim(),
                    CreatedDate = DateTime.Now,
                    Status = true,
                    Highlights = request.Highlights,
                    Description = !String.IsNullOrEmpty(request.Description) ? request.Description : null,
                };
                await _DbContext.Brands.AddAsync(brand);

                // Shop brans
                foreach (var categoryId in request.CategoryIds)
                {
                    var brandCategory = new BrandCategory
                    {
                        BrandId = brand.BrandId,
                        CategoryId = categoryId,
                    };
                    await _DbContext.BrandCategories.AddAsync(brandCategory);
                }
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
            if (string.IsNullOrEmpty(request.BrandName)) return new FailResponse<FileChangedResponse>("Thông tin không được để trống");
            if (request.CategoryIds == null || request.CategoryIds.Count == 0) return new FailResponse<FileChangedResponse>("Vui lòng chọn danh mục");

            try
            {
                var brand = await _DbContext.Brands.Where(i => i.BrandId == request.BrandId).FirstOrDefaultAsync();
                var oldFileName = brand.BrandImagePath;

                var fileChange = new FileChangedResponse();
                fileChange.previousFileName = oldFileName;
                if (request.BrandImagePath != null)
                {
                    brand.BrandImagePath = request.BrandImagePath;
                    fileChange.newFileName = brand.BrandImagePath;
                }
                brand.BrandName = request.BrandName.Trim();
                brand.Highlights = request.Highlights;
                brand.Status = request.Status;
                await _DbContext.SaveChangesAsync();

                var brandCategory = await _DbContext.BrandCategories
                    .Where(i => i.BrandId == request.BrandId)
                    .ToListAsync();
                if (brandCategory.Count > 0)
                {
                    _DbContext.BrandCategories.RemoveRange(brandCategory);
                    _DbContext.SaveChangesAsync().Wait();
                }
                foreach (var id in request.CategoryIds)
                {
                    var newBrandCategory = new BrandCategory
                    {
                        BrandId = request.BrandId,
                        CategoryId = id,
                    };
                    await _DbContext.BrandCategories.AddAsync(newBrandCategory);
                    await _DbContext.SaveChangesAsync();
                }

                return new SuccessResponse<FileChangedResponse>("Cập nhật thành công", fileChange);
            }
            catch(Exception error)
            {
                return new FailResponse<FileChangedResponse>("Cập nhật thất bại" + error.Message);
            }
        }
        public async Task<ApiResponse> UpdateStatus(int id, bool status)
        {
            try
            {
                var brand = await _DbContext.Brands.Where(i => i.BrandId == id).FirstOrDefaultAsync();
                brand.Status = status;
                _DbContext.SaveChangesAsync().Wait();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại");
            }
        }
        public async Task<ApiResponse> UpdateBrandsStatus(List<int> ids, bool status)
        {
            try
            {
                var brands = await _DbContext.Brands.Where(i => ids.Contains(i.BrandId)).ToListAsync();
                foreach (var brand in brands)
                {
                    brand.Status = status;
                }
                _DbContext.SaveChangesAsync().Wait();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại");
            }
        }
        public async Task<List<BrandModel>> getAll()
        {
            var list = await _DbContext.Brands
                .Select(i => new BrandModel() {
                    BrandId = i.BrandId,
                    BrandName = i.BrandName,
                    BrandImagePath = i.BrandImagePath,
                    Status = i.Status,
                    CreatedDate = i.CreatedDate,
                    Highlights = i.Highlights,
                    New = i.New,
                    Category = String.Join(", ", i.BrandCategories.Where(br => br.BrandId == i.BrandId).Select(br => br.Category.CategoryName).ToList()),
                    //CategoryId = i.Category.CategoryId
                })
                .OrderBy(i => i.BrandName)
                .ToListAsync();

            return list;
        }
        public async Task<List<BrandModel>> GetAllAvailable()
        {
            var list = await _DbContext.Brands
                .Where(i => i.Status == true)
                .Select(i => new BrandModel()
                {
                    BrandId = i.BrandId,
                    BrandName = i.BrandName,
                    BrandImagePath = i.BrandImagePath,
                    Status = i.Status,
                    CreatedDate = i.CreatedDate,
                    Highlights = i.Highlights,
                    New = i.New,
                    Category = String.Join(", ", i.BrandCategories.Where(br => br.BrandId == i.BrandId).Select(br => br.Category.CategoryName).ToList()),
                    CategoryIds = _DbContext.BrandCategories
                        .Where(bc => bc.BrandId == i.BrandId)
                        .Select(bc => bc.BrandId)
                        .ToList()
                })
                .OrderBy(i => i.BrandName)
                .ToListAsync();

            return list;
        }
        public async Task<List<BrandModel>> getAllManage()
        {
            var result = await _DbContext.Brands
                .Select(i => new BrandModel()
                {
                    BrandId = i.BrandId,
                    BrandName = i.BrandName,
                    BrandImagePath = i.BrandImagePath,
                    Status = i.Status,
                    CreatedDate = i.CreatedDate,
                    Highlights = i.Highlights,
                    New = i.New,
                    //Category = i.Category.CategoryName,
                    CategoryIds = i.BrandCategories.Where(bc => bc.BrandId == i.BrandId).Select(bc => bc.CategoryId).ToList(),
                    Shops = i.ShopBrands
                    .Select(s => new ShopManage() { 
                        id = s.Shop.ShopId,
                        name = s.Shop.ShopName,
                        status = (int)s.Shop.Status
                    })
                    .ToList()
                })
                .ToListAsync();
            return result;
        }
        public async Task<List<BrandModel>> getAllAvailableManage()
        {
            var result = await _DbContext.Brands
                .Where(i => i.Status == true)
                .Select(i => new BrandModel()
                {
                    BrandId = i.BrandId,
                    BrandName = i.BrandName,
                    BrandImagePath = i.BrandImagePath,
                    Status = i.Status,
                    CreatedDate = i.CreatedDate,
                    Highlights = i.Highlights,
                    New = i.New,
                    //Category = i.Category.CategoryName,
                    CategoryIds = i.BrandCategories
                        .Where(bc => bc.BrandId == i.BrandId)
                        .Select(bc => bc.CategoryId)
                        .ToList(),
                    Shops = i.ShopBrands
                    .Select(s => new ShopManage()
                    {
                        id = s.Shop.ShopId,
                        name = s.Shop.ShopName,
                        status = (int)s.Shop.Status
                    })
                    .ToList()
                })
                .ToListAsync();
            return result;
        }
        public async Task<List<BrandModel>> getAllAvailableInCategory(int CategoryId)
        {
            var list = await _DbContext.BrandCategories
                .Where(i => i.CategoryId == CategoryId && i.Brand.Status == true)
                .Select(i => new BrandModel() {
                    BrandId = i.Brand.BrandId,
                    BrandName = i.Brand.BrandName,
                    BrandImagePath = i.Brand.BrandImagePath,
                    Status = i.Brand.Status,
                    CreatedDate = i.Brand.CreatedDate,
                    Highlights = i.Brand.Highlights,
                    New = i.Brand.New,
                    Category = i.Category.CategoryName,
                    //CategoryId = i.Category.CategoryId
                })
                .OrderBy(i => i.BrandName)
                .ToListAsync();

            return list;
        }
        public async Task<List<BrandModel>> getAllBrandInShop(int userId)
        {
            var roleName = await _DbContext.UserRoles.Where(i => i.UserId == userId).Select(i => i.Role.RoleName).FirstOrDefaultAsync();
            var list = new List<BrandModel>();
            if (roleName == "Admin")
            {
                list = await _DbContext.Brands
                    .Select(i => new BrandModel()
                    {
                        BrandId = i.BrandId,
                        BrandName = i.BrandName,
                        BrandImagePath = i.BrandImagePath,
                        Status = i.Status,
                        CreatedDate = i.CreatedDate,
                        Highlights = i.Highlights,
                        New = i.New,
                        //Category = i.Category.CategoryName,
                        //CategoryId = i.Category.CategoryId
                    })
                    .ToListAsync();
            }
            else
            {
                var shopId = await _DbContext.Shops.Where(i => i.UserId == userId).Select(i => i.ShopId).FirstOrDefaultAsync();
                list = await _DbContext.ShopBrands
                    .Where(i => i.ShopId == shopId)
                    .Select(i => new BrandModel()
                    {
                        BrandId = i.Brand.BrandId,
                        BrandName = i.Brand.BrandName,
                        BrandImagePath = i.Brand.BrandImagePath,
                        Status = i.Brand.Status,
                        CreatedDate = i.Brand.CreatedDate,
                        Highlights = i.Brand.Highlights,
                        New = i.Brand.New,
                        //Category = i.Brand.Category.CategoryName,
                        //CategoryId = i.Brand.Category.CategoryId
                    })
                    .ToListAsync();
            }

            return list;
        }
        public async Task<List<BrandModel>> getBrandsByShop(int id)
        {
            var list = await _DbContext.ShopBrands
                    .Where(i => i.ShopId == id)
                    .Select(i => new BrandModel()
                    {
                        BrandId = i.Brand.BrandId,
                        BrandName = i.Brand.BrandName,
                        BrandImagePath = i.Brand.BrandImagePath,
                        Status = i.Brand.Status,
                        CreatedDate = i.Brand.CreatedDate,
                        Highlights = i.Brand.Highlights,
                        New = i.Brand.New,
                        //Category = i.Brand.Category.CategoryName,
                        //CategoryId = i.Brand.Category.CategoryId
                    })
                    .ToListAsync();
            return list;
        }
        public async Task<BrandModel> getBrandById(int BrandId)
        {
            var result = await _DbContext.Brands
                .Where(i => i.BrandId == BrandId)
                .Select(i => new BrandModel() {
                    BrandId = i.BrandId,
                    BrandName = i.BrandName,
                    BrandImagePath = i.BrandImagePath,
                    Status = i.Status,
                    CreatedDate = i.CreatedDate,
                    Highlights = i.Highlights,
                    New = i.New
                })
                .SingleOrDefaultAsync();

            return result;
        }
        public async Task<Response<string>> DeleteBrand(int id)
        {
            try
            {
                if (id == 0) return new FailResponse<string>("Vui lòng chọn thương hiệu");

                var products = await _DbContext.Products
                    .Where(i => id == i.BrandId)
                    .ToListAsync();
                var hasProduct = products.Count() > 0;
                if (hasProduct)
                {
                    var idList = String.Join(", ", products.Select(i => i.Ppc).ToList());
                    return new FailResponse<string>($"Thương hiệu đang tồn tại sản phẩm {idList}, không thể xóa");
                }

                var shopBrands = await _DbContext.ShopBrands.Where(i => i.BrandId == id).ToListAsync();
                if (shopBrands != null)
                    _DbContext.ShopBrands.RemoveRange(shopBrands);
                var brandCategories = await _DbContext.BrandCategories.Where(i => i.BrandId == id).ToListAsync();
                if (brandCategories != null)
                    _DbContext.BrandCategories.RemoveRange(brandCategories);
                var brand = await _DbContext.Brands.Where(i => i.BrandId == id).FirstOrDefaultAsync();
                if(brand != null)
                    _DbContext.Brands.Remove(brand);
                _DbContext.SaveChangesAsync().Wait();
                 
                return new SuccessResponse<string>("Xóa thành công", brand.BrandImagePath);
            }
            catch
            {
                return new FailResponse<string>("Xóa thất bại");
            }
        }
        public async Task<Response<List<string>>> DeleteBrands(List<int> ids)
        {
            try
            {
                if (ids == null) return new FailResponse<List<string>>("Vui lòng chọn thương hiệu");

                var products = await _DbContext.Products
                    .Where(i => ids.Contains(i.BrandId))
                    .ToListAsync();
                var hasProduct = products.Count() > 0;
                if (hasProduct)
                {
                    var idList = String.Join(", ", products.Select(i => i.Ppc).ToList());
                    return new FailResponse<List<string>>($"Có thương hiệu đang tồn tại sản phẩm {idList}, không thể xóa");
                }
                
                var shopBrands = await _DbContext.ShopBrands
                    .Where(i => ids.Contains(i.BrandId))
                    .ToListAsync();
                if (shopBrands != null)
                    _DbContext.ShopBrands.RemoveRange(shopBrands);

                var brandCategories = await _DbContext.BrandCategories
                    .Where(i => ids.Contains(i.BrandId))
                    .ToListAsync();
                if (brandCategories != null)
                    _DbContext.BrandCategories.RemoveRange(brandCategories);
                
                var brands = await _DbContext.Brands
                    .Where(i => ids.Contains(i.BrandId))
                    .ToListAsync();
                if (brands != null)
                    _DbContext.Brands.RemoveRange(brands);
                _DbContext.SaveChangesAsync().Wait();

                List<string> paths = brands.Select(item => item.BrandImagePath).ToList();
                return new SuccessResponse<List<string>>("Xóa thành công", paths);
            }
            catch
            {
                return new FailResponse<List<string>>("Xóa thất bại");
            }
        }
    }
}
