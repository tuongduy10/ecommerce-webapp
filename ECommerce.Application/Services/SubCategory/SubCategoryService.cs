using ECommerce.Application.Common;
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
        public async Task<ApiResponse> Create(SubCategoryCreateRequest request)
        {
            try
            {
                if (request.CategoryId == 0) return new ApiFailResponse("Không tìm thấy danh mục");
                if (string.IsNullOrEmpty(request.SubCategoryName)) return new ApiFailResponse("Tên không được để trống");


                var subcate = new Data.Models.SubCategory
                {
                    SubCategoryName = request.SubCategoryName.Trim(),
                    CategoryId = request.CategoryId
                };
                await _DbContext.SubCategories.AddAsync(subcate);
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Thêm thành công");
            }
            catch
            {
                return new ApiFailResponse("Thêm thất bại, vui lòng thử lại sau");
            }
        }
        public async Task<ApiResponse> Delete(int CategoryId)
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResponse> Update(SubCategoryUpdateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.name)) return new ApiSuccessResponse("Tên không được bỏ trống");

                var subcategory = await _DbContext.SubCategories
                    .Where(i => i.SubCategoryId == request.id)
                    .FirstOrDefaultAsync();

                subcategory.SubCategoryName = request.name.Trim();

                if (request.opts != null && request.opts.Count > 0)
                {
                    
                }
                if (request.attrs != null && request.attrs.Count > 0)
                {

                }
                await _DbContext.SaveChangesAsync();

                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại, vui lòng thử lại sau");
            }
        }
        public async Task<List<SubCategoryModel>> getAll()
        {
            var list = from c in _DbContext.SubCategories select c;
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
        public async Task<List<SubCategoryModel>> getSubCategoryByCategoryWithOptsAndAttrs(int id)
        {
            var subcategories = await _DbContext.SubCategories
                .Where(i => i.CategoryId == id)
                .Select(i => new SubCategoryModel {
                    SubCategoryId = i.SubCategoryId,
                    SubCategoryName = i.SubCategoryName,
                    options = i.SubCategoryOptions
                    .Where(sbo => sbo.SubCategoryId == i.SubCategoryId)
                    .Select(sbo => new OptionGetModel { 
                        id = sbo.OptionId,
                        name = sbo.Option.OptionName
                    })
                    .ToList(),
                    attributes = i.SubCategoryAttributes
                    .Where(sba => sba.SubCategoryId == i.SubCategoryId)
                    .Select(sba => new AttributeGetModel { 
                        id = sba.AttributeId,
                        name = sba.Attribute.AttributeName
                    })
                    .ToList()
                })
                .ToListAsync();
            return subcategories;
        }
        public async Task<List<OptionGetModel>> getOptionBySubCategoryId(int id)
        {
            var result = await _DbContext.SubCategoryOptions
                .Where(i => i.SubCategoryId == id)
                .Select(i => new OptionGetModel { 
                    id = i.Option.OptionId,
                    name = i.Option.OptionName
                })
                .ToListAsync();

            return result;
        }
        public async Task<List<AttributeGetModel>> getAttributeBySubCategoryId(int id)
        {
            var result = await _DbContext.SubCategoryAttributes
                .Where(i => i.SubCategoryId == id)
                .Select(i => new AttributeGetModel { 
                    id = i.Attribute.AttributeId,
                    name = i.Attribute.AttributeName
                })
                .ToListAsync();

            return result;
        }
        public async Task<List<string>> getOptionValueNameByOptionId(int id)
        {
            var result = await _DbContext.OptionValues
                .Where(i => i.OptionId == id && i.IsBaseValue == true)
                .Select(i => i.OptionValueName)
                .ToListAsync();
            return result;
        }
        public async Task<List<OptionValueGetModel>> getBaseOptionValueByOptionId(int id)
        {
            var result = await _DbContext.OptionValues
                .Where(i => i.OptionId == id && i.IsBaseValue == true)
                .Select(i => new OptionValueGetModel { 
                    id = i.OptionValueId,
                    value = i.OptionValueName
                })
                .ToListAsync();
            return result;
        }
        public async Task<ApiResponse> UpdateOptionForSub(SubListUpdateRequest request)
        {
            try
            {
                if (request.ids == null || request.ids.Count == 0) return new ApiFailResponse("Vui lòng chọn loại sản phẩm");
                if (string.IsNullOrEmpty(request.name)) return new ApiFailResponse("Vui lòng nhập tên");

                var subs_otps = await _DbContext.SubCategoryOptions.Where(i => request.ids.Contains(i.SubCategoryId)).ToListAsync();
                var option = await _DbContext.Options.Where(i => i.OptionName == request.name.Trim()).FirstOrDefaultAsync();
                if (subs_otps.Count == 0)
                {
                    
                }
                if (option == null) // Create new option and update option in subcategory
                {
                    var newOption = new Data.Models.Option
                    {
                        OptionName = request.name.Trim()
                    };
                    await _DbContext.Options.AddAsync(newOption);
                    await _DbContext.SaveChangesAsync();

                    foreach (var id in request.ids)
                    {
                        var newSubOpt = new Data.Models.SubCategoryOption
                        {
                            SubCategoryId = id,
                            OptionId = newOption.OptionId
                        };
                        await _DbContext.SubCategoryOptions.AddAsync(newSubOpt);
                    }
                    await _DbContext.SaveChangesAsync();
                }
                if (option != null) // Update option in subcategory
                {
                    foreach (var id in request.ids)
                    {
                        var subs = await _DbContext.SubCategoryOptions
                            .Where(i => i.SubCategoryId == id)
                            .ToListAsync();
                        if (subs.Count == 0)
                        {
                            
                        }
                        if (subs == null)
                        {
                            var newSubOpt = new Data.Models.SubCategoryOption
                            {
                                SubCategoryId = id,
                                OptionId = option.OptionId
                            };
                            await _DbContext.SubCategoryOptions.AddAsync(newSubOpt);
                            await _DbContext.SaveChangesAsync();
                        }
                    }
                }
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại, thử lại sau");
            }
        }
        public async Task<ApiResponse> UpdateAttributeForSub(SubListUpdateRequest request)
        {
            try
            {
                if (request.ids == null || request.ids.Count == 0) return new ApiFailResponse("Vui lòng chọn loại sản phẩm");
                if (string.IsNullOrEmpty(request.name)) return new ApiFailResponse("Vui lòng nhập tên");

                var subcategories = await _DbContext.SubCategoryAttributes.Where(i => request.ids.Contains(i.SubCategoryId)).ToListAsync();
                if (subcategories.Count == 0) return new ApiFailResponse("Không tìm thấy danh sách loại sản phẩm");

                var attribute = await _DbContext.Attributes.Where(i => i.AttributeName == request.name.Trim()).FirstOrDefaultAsync();
                if (attribute == null) // Create new attribute and update attribute in subcategory
                {
                    var newAttribute = new Data.Models.Attribute
                    {
                        AttributeName = request.name.Trim()
                    };
                    await _DbContext.Attributes.AddAsync(newAttribute);
                    await _DbContext.SaveChangesAsync();

                    foreach (var item in subcategories)
                    {
                        item.AttributeId = newAttribute.AttributeId;
                    }
                    await _DbContext.SaveChangesAsync();
                }
                if (attribute != null) // Update attribute in subcategory
                {
                    foreach (var item in subcategories)
                    {
                        item.AttributeId = attribute.AttributeId;
                    }
                    await _DbContext.SaveChangesAsync();
                }

                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại, thử lại sau");
            }
        }
    }
}
