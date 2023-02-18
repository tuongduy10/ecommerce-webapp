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
                var hasSubName = await _DbContext.SubCategories
                    .Where(i => i.CategoryId == request.CategoryId && i.SubCategoryName == request.SubCategoryName.Trim())
                    .FirstOrDefaultAsync() != null;
                if (hasSubName)
                {
                    return new ApiFailResponse("Đã tồn tại");
                }

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
        public async Task<ApiResponse> Delete(int id)
        {
            if (id == 0) return new ApiFailResponse("Vui lòng chọn loại sản phẩm");

            try
            {
                var products = await _DbContext.Products.Where(i => i.SubCategoryId == id).ToListAsync();
                if (products.Count > 0)
                {
                    return new ApiFailResponse($"Hiện đang có {products.Count} sản phẩm đang chọn loại sản phẩm này, không thể xóa");
                }
                var opts = await _DbContext.SubCategoryOptions.Where(i => i.SubCategoryId == id).ToListAsync();
                if (opts.Count > 0)
                {
                    _DbContext.SubCategoryOptions.RemoveRange(opts);
                    await _DbContext.SaveChangesAsync();
                }
                var attrs = await _DbContext.SubCategoryAttributes.Where(i => i.SubCategoryId == id).ToListAsync();
                if (attrs.Count > 0)
                {
                    _DbContext.SubCategoryAttributes.RemoveRange(attrs);
                    await _DbContext.SaveChangesAsync();
                }
                var sub = await _DbContext.SubCategories.Where(i => i.SubCategoryId == id).FirstOrDefaultAsync();
                if (sub != null)
                {
                    _DbContext.SubCategories.Remove(sub);
                    await _DbContext.SaveChangesAsync();
                }

                return new ApiSuccessResponse("Xóa thành công");
            }
            catch
            {
                return new ApiFailResponse("Xóa thật bại, vui lòng thử lại");
            }
        }
        public async Task<ApiResponse> Update(SubCategoryUpdateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.name)) return new ApiSuccessResponse("Vui lòng nhập tên");

                var subcategory = await _DbContext.SubCategories
                    .Where(i => i.SubCategoryId == request.id)
                    .FirstOrDefaultAsync();
                subcategory.SubCategoryName = request.name.Trim();

                // Remove previous data
                 var opts = await _DbContext.SubCategoryOptions
                    .Where(i => i.SubCategoryId == request.id)
                    .ToListAsync();
                if (opts.Count > 0)
                {
                    _DbContext.SubCategoryOptions.RemoveRange(opts);
                    await _DbContext.SaveChangesAsync();
                }
                if (request.optIds != null && request.optIds.Count > 0)
                {
                    // Add new or re-add
                    foreach (var id in request.optIds)
                    {
                        var newOpt = new Data.Models.SubCategoryOption
                        {
                            SubCategoryId = request.id,
                            OptionId = id
                        };
                        await _DbContext.SubCategoryOptions.AddRangeAsync(newOpt);
                        await _DbContext.SaveChangesAsync();
                    }
                }

                // Remove previous data
                var attrs = await _DbContext.SubCategoryAttributes
                    .Where(i => i.SubCategoryId == request.id)
                    .ToListAsync();
                if (attrs.Count > 0)
                {
                    _DbContext.SubCategoryAttributes.RemoveRange(attrs);
                    await _DbContext.SaveChangesAsync();
                }
                if (request.attrIds != null && request.attrIds.Count > 0)
                {
                    // Add new or re-add
                    foreach (var attr in request.attrIds)
                    {
                        var newAttr = new Data.Models.SubCategoryAttribute
                        {
                            SubCategoryId = request.id,
                            AttributeId = attr
                        };
                        await _DbContext.SubCategoryAttributes.AddRangeAsync(newAttr);
                    }
                    await _DbContext.SaveChangesAsync();
                }
                var attrIds = await _DbContext.SubCategoryAttributes
                    .Where(i => i.SubCategoryId == request.id)
                    .Select(i => i.AttributeId)
                    .ToListAsync();
                var proAttrs = await _DbContext.ProductAttributes
                    .Where(i => !attrIds.Contains(i.AttributeId))
                    .ToListAsync();
                _DbContext.ProductAttributes.RemoveRange(proAttrs);
                _DbContext.SaveChangesAsync().Wait();

                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại, vui lòng thử lại sau");
            }
        }
        public async Task<List<SubCategoryModel>> getAll()
        {
            var list = await _DbContext.SubCategories
                .Select(i => new SubCategoryModel()
                {
                    SubCategoryId = i.SubCategoryId,
                    SubCategoryName = i.SubCategoryName,
                    CategoryId = i.CategoryId,
                    SizeGuide = i.SizeGuide
                }).ToListAsync();

            return list;
        }
        public async Task<List<SubCategoryModel>> getSubCategoryInBrand(int BrandId)
        {
            var query = from subc in _DbContext.SubCategories
                        from brand in _DbContext.BrandCategories
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
                var categoryId = await _DbContext.BrandCategories
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
        public async Task<List<OptionValueGetModel>> getOptionValueByOptionId(int id)
        {
            var result = await _DbContext.OptionValues
                .Where(i => i.OptionId == id)
                .Select(i => new OptionValueGetModel
                {
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

                var option = await _DbContext.Options.Where(i => i.OptionName == request.name.Trim()).FirstOrDefaultAsync();
                if (option == null) // Create new option and update option in subcategory
                {
                    // Create new
                    var newOption = new Data.Models.Option
                    {
                        OptionName = request.name.Trim()
                    };
                    await _DbContext.Options.AddAsync(newOption);
                    await _DbContext.SaveChangesAsync();

                    // update option in subcategory
                    foreach (var id in request.ids)
                    {
                        var subs = await _DbContext.SubCategoryOptions
                            .Where(i => i.SubCategoryId == id && i.OptionId == newOption.OptionId)
                            .FirstOrDefaultAsync();
                        if (subs == null)
                        {
                            var newSubOpt = new Data.Models.SubCategoryOption
                            {
                                SubCategoryId = id,
                                OptionId = newOption.OptionId
                            };
                            await _DbContext.SubCategoryOptions.AddAsync(newSubOpt);
                            await _DbContext.SaveChangesAsync();
                        }
                    }
                    
                }
                if (option != null) // Update option in subcategory
                {
                    foreach (var id in request.ids)
                    {
                        var subs = await _DbContext.SubCategoryOptions
                            .Where(i => i.SubCategoryId == id && i.OptionId == option.OptionId)
                            .FirstOrDefaultAsync();
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

                var attribute = await _DbContext.Attributes.Where(i => i.AttributeName == request.name.Trim()).FirstOrDefaultAsync();
                if (attribute == null) // Create new attribute and update attribute in subcategory
                {
                    // Create new
                    var newAttribute = new Data.Models.Attribute
                    {
                        AttributeName = request.name.Trim()
                    };
                    await _DbContext.Attributes.AddAsync(newAttribute);
                    await _DbContext.SaveChangesAsync();

                    // Update with new value
                    foreach (var id in request.ids)
                    {
                        var attrs = await _DbContext.SubCategoryAttributes
                            .Where(i => i.SubCategoryId == id && i.AttributeId == newAttribute.AttributeId)
                            .FirstOrDefaultAsync();
                        if (attrs == null)
                        {
                            var attr = new Data.Models.SubCategoryAttribute
                            {
                                SubCategoryId = id,
                                AttributeId = newAttribute.AttributeId
                            };
                            await _DbContext.SubCategoryAttributes.AddAsync(attr);
                            await _DbContext.SaveChangesAsync();
                        }
                    }
                }
                if (attribute != null) // Update attribute in subcategory
                {
                    foreach (var id in request.ids)
                    {
                        var attrIsNotExist = await _DbContext.SubCategoryAttributes
                            .Where(i => i.SubCategoryId == id && i.AttributeId == attribute.AttributeId)
                            .FirstOrDefaultAsync() == null;
                        if (attrIsNotExist)
                        {
                            var attr = new Data.Models.SubCategoryAttribute
                            {
                                SubCategoryId = id,
                                AttributeId = attribute.AttributeId
                            };
                            await _DbContext.SubCategoryAttributes.AddAsync(attr);
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
        public async Task<List<OptionGetModel>> getAllOptions()
        {
            var result = await _DbContext.Options
                .Select(i => new OptionGetModel {
                    id = i.OptionId,
                    name = i.OptionName,
                    values = _DbContext.OptionValues
                    .Where(v => v.OptionId == i.OptionId && v.IsBaseValue == true)
                    .Select(v => new OptionValueGetModel { 
                        id = v.OptionValueId,
                        value = v.OptionValueName
                    })
                    .ToList(),
                })
                .ToListAsync();
            return result;
        }
        public async Task<ApiResponse> AddOptionBaseValue(OptionBaseValueAddRequest request)
        {
            try
            {
                if (request.ids == null || request.ids.Count == 0) return new ApiFailResponse("Chọn tùy chọn");
                if (string.IsNullOrEmpty(request.value)) return new ApiFailResponse("Nhập giá trị");

                foreach (var id in request.ids)
                {
                    var value = await _DbContext.OptionValues
                        .Where(i => i.OptionId == id && i.OptionValueName == request.value.Trim())
                        .FirstOrDefaultAsync();
                    if (value == null)
                    {
                        // Add new option value
                        var newValue = new Data.Models.OptionValue
                        {
                            OptionValueName = request.value,
                            OptionId = id,
                            IsBaseValue = true
                        };
                        await _DbContext.OptionValues.AddAsync(newValue);
                        await _DbContext.SaveChangesAsync();

                        // Add new product option value from option, find sub, find products, add new.
                        var sub_optIds = await _DbContext.SubCategoryOptions
                            .Where(i => i.OptionId == id)
                            .Select(i => i.SubCategoryId)
                            .ToListAsync();
                        var pros = await _DbContext.Products
                            .Where(i => sub_optIds.Contains(i.SubCategoryId))
                            .ToListAsync();
                        if (pros.Count > 0)
                        {
                            foreach (var pro in pros)
                            {
                                var newProductOptionValue = new Data.Models.ProductOptionValue
                                {
                                    ProductId = pro.ProductId,
                                    OptionValueId = newValue.OptionValueId
                                };
                                await _DbContext.ProductOptionValues.AddAsync(newProductOptionValue);
                                await _DbContext.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        value.IsBaseValue = true;
                        await _DbContext.SaveChangesAsync();
                    }
                }
                return new ApiSuccessResponse("Thêm thành công");
            }
            catch
            {
                return new ApiFailResponse("Thêm bại vui lòng thử lại sau");
            }
        }
        public async Task<ApiResponse> UpdateOptionBaseValue(OptionBaseValueUpdateRequest request)
        {
            try
            {
                if (request.id == 0) return new ApiFailResponse("Chọn tùy chọn");
                if (string.IsNullOrEmpty(request.name)) return new ApiFailResponse("Nhập tên tùy chọn");

                // Update option name
                var option = await _DbContext.Options
                    .Where(i => i.OptionId == request.id)
                    .FirstOrDefaultAsync();
                option.OptionName = request.name.Trim();
                _DbContext.SaveChangesAsync().Wait();

                // Update option value
                var optionValues = await _DbContext.OptionValues
                    .Where(i => i.OptionId == request.id && i.IsBaseValue == true)
                    .ToListAsync();
                if (optionValues.Count > 0)
                {
                    // Remove all
                    if (request.ids == null)
                    {
                        var pro_optVals = await _DbContext.ProductOptionValues
                            .Where(i => i.OptionValue.OptionId == request.id)
                            .ToListAsync();
                        _DbContext.ProductOptionValues.RemoveRange(pro_optVals);
                        _DbContext.OptionValues.RemoveRange(optionValues);
                        _DbContext.SaveChangesAsync().Wait();
                    }
                    // Remove option values not included in request list id
                    else
                    {
                        foreach (var value in optionValues)
                        {
                            if (!request.ids.Contains(value.OptionValueId))
                            {
                                var pro_optVals = await _DbContext.ProductOptionValues
                                    .Where(i => i.OptionValueId == value.OptionValueId)
                                    .ToListAsync();
                                _DbContext.ProductOptionValues.RemoveRange(pro_optVals);
                                _DbContext.OptionValues.Remove(value);
                                _DbContext.SaveChangesAsync().Wait();
                            }
                        }
                    }
                }


                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại vui lòng thử lại sau");
            }
        }
        public async Task<ApiResponse> DeleteOption(int id)
        {
            try
            {
                if (id == 0) return new ApiFailResponse("Chọn tùy chọn");

                // Remove subcategory option
                var sub_options = await _DbContext.SubCategoryOptions
                    .Where(i => i.OptionId == id)
                    .ToListAsync();
                if (sub_options.Count > 0)
                {
                    _DbContext.SubCategoryOptions.RemoveRange(sub_options);
                    _DbContext.SaveChangesAsync().Wait();
                }

                // Remove product option
                var pro_optionValues = await _DbContext.ProductOptionValues
                    .Where(i => i.OptionValue.OptionId == id)
                    .ToListAsync();
                if (pro_optionValues.Count > 0)
                {
                    _DbContext.ProductOptionValues.RemoveRange(pro_optionValues);
                    _DbContext.SaveChangesAsync().Wait();
                }

                // Remove option value
                var optionValues = await _DbContext.OptionValues
                    .Where(i => i.OptionId == id)
                    .ToListAsync();
                if (optionValues.Count > 0)
                {
                    _DbContext.OptionValues.RemoveRange(optionValues);
                    _DbContext.SaveChangesAsync().Wait();
                }

                // Remove option
                var option = await _DbContext.Options
                    .Where(i => i.OptionId == id)
                    .FirstOrDefaultAsync();
                if (option != null)
                {
                    _DbContext.Options.Remove(option);
                    _DbContext.SaveChangesAsync().Wait();
                }

                return new ApiSuccessResponse("Xóa thành công");
            }
            catch
            {
                return new ApiFailResponse("Xóa thất bại vui lòng thử lại sau");
            }
        }
    }
}
