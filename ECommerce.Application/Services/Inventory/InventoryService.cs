using ECommerce.Application.BaseServices.FilterProduct.Dtos;
using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services.Inventory.Dtos;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Data.Context;
using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly ECommerceContext _DbContext;
        private readonly IRepositoryBase<Data.Entities.Product> _productRepo;
        private readonly IRepositoryBase<Data.Entities.Option> _optionRepo;
        private readonly IRepositoryBase<Data.Entities.OptionValue> _optionValueRepo;
        private readonly IRepositoryBase<Brand> _brandRepo;
        private readonly IRepositoryBase<Category> _categoryRepo;
        private readonly IRepositoryBase<BrandCategory> _brandCategoryRepo;
        private readonly IRepositoryBase<ShopBrand> _shopBrandRepo;
        private readonly IRepositoryBase<SubCategory> _subCategoryRepo;
        private readonly IRepositoryBase<SubCategoryOption> _subCategoryOptionRepo;
        private readonly IRepositoryBase<SubCategoryAttribute> _subCategoryAttributeRepo;
        private readonly IRepositoryBase<ProductOptionValue> _productOptionValuesRepo;
        private readonly IRepositoryBase<ProductAttribute> _productAttributeRepo;
        private readonly IRepositoryBase<SizeGuide> _sizeGuideRepo;
        private readonly IRepositoryBase<Data.Entities.Attribute> _attributeRepo;
        public InventoryService(ECommerceContext DbContext,
            IRepositoryBase<Data.Entities.Product> productRepo,
            IRepositoryBase<Data.Entities.Option> optionRepo,
            IRepositoryBase<Data.Entities.OptionValue> optionValueRepo,
            IRepositoryBase<Brand> brandRepo,
            IRepositoryBase<Category> categoryRepo,
            IRepositoryBase<BrandCategory> brandCategoryRepo,
            IRepositoryBase<ShopBrand> shopBrandRepo,
            IRepositoryBase<SubCategory> subCategoryRepo,
            IRepositoryBase<SubCategoryOption> subCategoryOptionRepo,
            IRepositoryBase<SubCategoryAttribute> subCategoryAttributeRepo,
            IRepositoryBase<ProductOptionValue> productOptionValuesRepo,
            IRepositoryBase<ProductAttribute> productAttributeRepo,
            IRepositoryBase<SizeGuide> sizeGuideRepo,
            IRepositoryBase<Data.Entities.Attribute> attributeRepo)
        {
            _DbContext = DbContext;
            _productRepo = productRepo;
            _optionRepo = optionRepo;
            _optionValueRepo = optionValueRepo;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
            _brandCategoryRepo = brandCategoryRepo;
            _shopBrandRepo = shopBrandRepo;
            _subCategoryRepo = subCategoryRepo;
            _subCategoryOptionRepo = subCategoryOptionRepo;
            _subCategoryAttributeRepo = subCategoryAttributeRepo;
            _productOptionValuesRepo = productOptionValuesRepo;
            _productAttributeRepo = productAttributeRepo;
            _sizeGuideRepo = sizeGuideRepo;
            _attributeRepo = attributeRepo;
        }
        public async Task<Response<BrandModel>> getBrand(int brandId = 0) 
        {
            try
            {
                var result = await _brandRepo
                    .Entity()
                    .Where(i => i.BrandId == brandId)
                    .Select(i => new BrandModel()
                    {
                        id = i.BrandId,
                        name = i.BrandName,
                        imagePath = i.BrandImagePath,
                        isActive = i.Status,
                        createdDate = i.CreatedDate,
                        description = i.Description,
                        descriptionTitle = i.DescriptionTitle,
                        isHighlights = i.Highlights,
                        isNew = i.New
                    })
                    .FirstOrDefaultAsync();

                return new SuccessResponse<BrandModel>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<BrandModel>(error.Message);
            }
        }
        public async Task<Response<List<BrandModel>>> getBrands(BrandGetRequest request)
        {
            try
            {
                var shopId = request.shopId;
                var categoryId = request.categoryId;

                var result = await _brandRepo.Queryable(_ =>
                    (shopId == -1 || _.ShopBrands.Any(i => i.ShopId == shopId)) &&
                    (categoryId == -1 || _.BrandCategories.Any(i => i.CategoryId == categoryId)),"")
                    .Select(i => new BrandModel
                    {
                        id = i.BrandId,
                        name = i.BrandName,
                        imagePath = i.BrandImagePath,
                        isActive = i.Status,
                        createdDate = i.CreatedDate,
                        description = i.Description,
                        descriptionTitle = i.DescriptionTitle,
                        isHighlights = i.Highlights,
                        isNew = i.New,
                        categoryNames = i.BrandCategories.Select(_ => _.Category.CategoryName).ToList()
                    })
                    .ToListAsync();

                return new SuccessResponse<List<BrandModel>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<List<BrandModel>>(error.Message);
            }
        }
        public async Task<Response<List<CategoryModel>>> getCategories()
        {
            try
            {
                var res = await _categoryRepo.Queryable(_ => _.CategoryId > -1, "SubCategories")
                    .Select(_ => (CategoryModel)_)
                    .ToListAsync();
                return new SuccessResponse<List<CategoryModel>>(res);
            }
            catch (Exception error)
            {
                return new FailResponse<List<CategoryModel>>(error.Message);
            }
        }
        public async Task<Response<CategoryModel>> getCategory(int id)
        {
            try
            {
                var res = await _categoryRepo.Queryable(_ => _.CategoryId == id, "SubCategories")
                    .Select(_ => (CategoryModel)_)
                    .FirstOrDefaultAsync();
                return new SuccessResponse<CategoryModel>(res);
            }
            catch (Exception error)
            {
                return new FailResponse<CategoryModel>(error.Message);
            }
        }
        public async Task<Response<Category>> updateCategory(CategoryModelRequest req)
        {
            try
            {
                var entity = await _categoryRepo.Queryable(_ => _.CategoryId == req.id, "")
                    .FirstOrDefaultAsync();
                if (entity != null)
                {
                    if (!string.IsNullOrEmpty(req.name))
                        entity.CategoryName = req.name;
                    _categoryRepo.Update(entity);
                    await _categoryRepo.SaveChangesAsync();
                }
                return new SuccessResponse<Category>(entity);
            }
            catch (Exception error)
            {
                return new FailResponse<Category>(error.Message);
            }
        }
        public async Task<Response<Category>> addCategory(CategoryModelRequest req)
        {
            try
            {
                var entity = await _categoryRepo.Queryable(_ => _.CategoryId == req.id, "")
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    var newEntity = new Category();
                    if (!string.IsNullOrEmpty(req.name))
                        newEntity.CategoryName = req.name;
                    await _categoryRepo.AddAsync(newEntity);
                    await _categoryRepo.SaveChangesAsync();
                    return new SuccessResponse<Category>(newEntity);
                }
                return new SuccessResponse<Category>();
            }
            catch (Exception error)
            {
                return new FailResponse<Category>(error.Message);
            }
        }
        public async Task<Response<List<SubCategoryModel>>> getSubCategories(InventoryRequest request)
        {
            try
            {
                var subCategoryId = request.subCategoryId;
                var brandId = request.brandId;

                var list = await _subCategoryRepo.Queryable(_ =>
                    (subCategoryId == -1 || _.SubCategoryId == subCategoryId) &&
                    (brandId == -1 || _brandCategoryRepo
                        .Entity()
                        .Any(brand => _.CategoryId == brand.CategoryId && brand.BrandId == brandId)), "")
                    .Select(subc => new SubCategoryModel
                    {
                        id = subc.SubCategoryId,
                        name = subc.SubCategoryName,
                        categoryId = subc.CategoryId,
                        category = (CategoryModel)subc.Category,
                        optionList = _subCategoryOptionRepo
                            .Entity()
                            .Where(subcopt => subcopt.SubCategoryId == subc.SubCategoryId)
                            .Select(lo => new OptionModel
                            {
                                id = lo.OptionId,
                                name = lo.Option.OptionName,
                                values = _optionValueRepo
                                    .Entity()
                                    .Where(optionvalue => optionvalue.OptionId == lo.OptionId)
                                    .Select(lop => new OptionValueModel
                                    {
                                        id = lop.OptionValueId,
                                        name = lop.OptionValueName,
                                        totalRecord = _productOptionValuesRepo.Entity()
                                            .Where(ov => 
                                                ov.OptionValueId == lop.OptionValueId &&
                                                ov.Product.SubCategoryId == subc.SubCategoryId)
                                            .Select(ov => ov.ProductId)
                                            .Count(),
                                        isBase = (bool)lop.IsBaseValue
                                    })
                                    .ToList()
                            })
                            .ToList(),
                        attributes = subc.SubCategoryAttributes
                            .Select(sa => new AttributeModel
                            {
                                id = sa.AttributeId,
                                name = sa.Attribute.AttributeName
                            })
                            .ToList(),
                        sizeGuide = subc.SizeGuide != null 
                            ? new SizeGuideModel
                            {
                                id = (int)subc.SizeGuideId,
                                content = subc.SizeGuide.SizeContent,
                                isBase = (bool)subc.SizeGuide.IsBaseSizeGuide,
                            } 
                            : null
                    })
                    .ToListAsync();

                return new SuccessResponse<List<SubCategoryModel>>(list);
            }
            catch(Exception error)
            {
                return new FailResponse<List<SubCategoryModel>>(error.Message);
            }
        }
        public async Task<Response<SubCategory>> addSubCategory(SubCategoryModel request)
        {
            if (string.IsNullOrEmpty(request.name))
                return new FailResponse<SubCategory>("Nhập tên loại");
            var check = await _subCategoryRepo.AnyAsyncWhere(_ => _.SubCategoryName == request.name.Trim());
            if (check)
                return new FailResponse<SubCategory>("Loại đã tồn tại");

            var transaction = await _DbContext.Database.BeginTransactionAsync();
            try
            {
                var entity = new SubCategory();
                entity.SubCategoryName = request.name.Trim();
                entity.CategoryId = request.categoryId;
                await _subCategoryRepo.AddAsync(entity);
                await _subCategoryRepo.SaveChangesAsync();

                // attributes
                if (request.attributes.Count > 0)
                {
                    var attrs = request.attributes
                        .Select(_ => new SubCategoryAttribute
                        {
                            SubCategoryId = entity.SubCategoryId,
                            AttributeId = _.id,
                        })
                        .ToList();
                    await _subCategoryAttributeRepo.AddRangeAsync(attrs);
                    await _subCategoryAttributeRepo.SaveChangesAsync();
                }
                // options
                if (request.optionList.Count > 0)
                {
                    var opts = request.optionList
                        .Select(_ => new SubCategoryOption
                        {
                            SubCategoryId = entity.SubCategoryId,
                            OptionId = _.id
                        })
                        .ToList();
                    await _subCategoryOptionRepo.AddRangeAsync(opts);
                    await _subCategoryOptionRepo.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                return new SuccessResponse<SubCategory>();
            }
            catch (Exception error)
            {
                await transaction.RollbackAsync();
                return new FailResponse<SubCategory>(error.Message);
            }
        }
        public async Task<Response<SubCategory>> updateSubCategory(SubCategoryModel request)
        {
            if (request.id == -1)
                return new FailResponse<SubCategory>("Not found");
            if (string.IsNullOrEmpty(request.name))
                return new FailResponse<SubCategory>("Nhập tên loại");
            var entity = await _subCategoryRepo.GetAsyncWhere(_ => _.SubCategoryId == request.id);
            if (entity == null)
                return new FailResponse<SubCategory>("Not exist");

            var transaction = await _DbContext.Database.BeginTransactionAsync();
            try
            {
                entity.SubCategoryName = request.name.Trim();
                entity.CategoryId = request.categoryId;
                _subCategoryRepo.Update(entity);
                await _subCategoryRepo.SaveChangesAsync();

                // attributes
                await _subCategoryAttributeRepo.RemoveRangeAsyncWhere(_ => _.SubCategoryId == entity.SubCategoryId);
                await _subCategoryAttributeRepo.SaveChangesAsync();
                if (request.attributes.Count > 0)
                {
                    var attrs = request.attributes
                        .Select(_ => new SubCategoryAttribute
                        {
                            SubCategoryId = entity.SubCategoryId,
                            AttributeId = _.id,
                        })
                        .ToList();
                    await _subCategoryAttributeRepo.AddRangeAsync(attrs);
                    await _subCategoryAttributeRepo.SaveChangesAsync();
                }

                // options
                await _subCategoryOptionRepo.RemoveRangeAsyncWhere(_ => _.SubCategoryId == entity.SubCategoryId);
                await _subCategoryOptionRepo.SaveChangesAsync();
                if (request.optionList.Count > 0)
                {
                    var opts = request.optionList
                        .Select(_ => new SubCategoryOption
                        {
                            SubCategoryId = entity.SubCategoryId,
                            OptionId = _.id
                        })
                        .ToList();
                    await _subCategoryOptionRepo.AddRangeAsync(opts);
                    await _subCategoryOptionRepo.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                return new SuccessResponse<SubCategory>();
            }
            catch (Exception error)
            {
                await transaction.RollbackAsync();
                return new FailResponse<SubCategory>(error.Message);
            }
        }
        public async Task<Response<bool>> deleteSubCategories(List<int> ids)
        {
            try
            {
                if (ids.Count > 0)
                {
                    
                }
                return new SuccessResponse<bool>();
            }
            catch (Exception error)
            {
                return new FailResponse<bool>();
            }
        }
        public async Task<Response<List<OptionModel>>> getOptions(InventoryRequest request)
        {
            try
            {
                var opts = await _optionRepo
                    .Queryable()
                    .Select(opt => new OptionModel
                    {
                        id = opt.OptionId,
                        name = opt.OptionName,
                        values = opt.OptionValues.Select(_ => (OptionValueModel)_).ToList()
                    })
                    .ToListAsync();
                return new SuccessResponse<List<OptionModel>>(opts);
            }
            catch (Exception error)
            {
                return new FailResponse<List<OptionModel>>(error.Message);
            }
        }
        public async Task<Response<List<OptionModel>>> getProductOptions(InventoryRequest request)
        {
            try
            {
                var productId = request.productId;
                var subCategoryId = request.subCategoryId;

                if (productId > -1)
                {
                    var opts = await _optionRepo
                        .Entity()
                        .Where(opt => _productOptionValuesRepo
                            .Entity()
                            .Any(pov => pov.ProductId == productId && pov.OptionValue.OptionId == opt.OptionId))
                        .Select(opt => new OptionModel
                        {
                            id = opt.OptionId,
                            name = opt.OptionName,
                            values = _productOptionValuesRepo
                                .Entity()
                                .Where(pov => pov.ProductId == productId && pov.OptionValue.OptionId == opt.OptionId)
                                .Select(pov => new OptionValueModel
                                {
                                    id = pov.OptionValue.OptionValueId,
                                    name = pov.OptionValue.OptionValueName,
                                    isBase = (bool)pov.OptionValue.IsBaseValue
                                })
                                .ToList()
                        })
                        .ToListAsync();
                    return new SuccessResponse<List<OptionModel>>(opts);
                }
                if (subCategoryId > -1)
                {
                    var isBase = request.isBase;
                    var subCategoryOpts = await _subCategoryOptionRepo
                        .Entity()
                        .Where(i => i.SubCategoryId == subCategoryId)
                        .Select(i => new OptionModel
                        {
                            id = i.OptionId,
                            name = i.Option.OptionName,
                            values = _optionValueRepo
                                .Entity()
                                .Where(pov => pov.OptionId == i.OptionId)
                                .Select(pov => new OptionValueModel
                                {
                                    id = pov.OptionValueId,
                                    name = pov.OptionValueName,
                                    totalRecord = _productOptionValuesRepo.Entity()
                                            .Where(ov =>
                                                ov.OptionValueId == pov.OptionValueId &&
                                                ov.Product.SubCategoryId == subCategoryId)
                                            .Select(ov => ov.ProductId)
                                            .Count(),
                                    isBase = (bool)pov.IsBaseValue
                                })
                                .Where(pov => !isBase || (isBase == true && pov.isBase))
                                .ToList()
                        })
                        .ToListAsync();
                    return new SuccessResponse<List<OptionModel>>(subCategoryOpts);
                }
                return new SuccessResponse<List<OptionModel>>();
            }
            catch (Exception error)
            {
                return new FailResponse<List<OptionModel>>(error.Message);
            }
        }
        public async Task<Response<List<AttributeModel>>> getAttributes(InventoryRequest request)
        {
            try
            {
                var subCategoryAttrs = await _attributeRepo
                    .Queryable(_ => 
                        request.id == -1 || _.AttributeId == request.id
                    )
                    .Select(_ => (AttributeModel)_)
                    .ToListAsync();
                return new SuccessResponse<List<AttributeModel>>(subCategoryAttrs);
            }
            catch (Exception error)
            {
                return new FailResponse<List<AttributeModel>>(error.Message);
            }
        }
        public async Task<Response<bool>> saveAttributes(InventoryRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.name))
                    return new FailResponse<bool>("Nhập tên");

                string name = request.name.Trim();

                var entity = await _attributeRepo.GetAsyncWhere(_ => _.AttributeId == request.id);
                if (entity != null)
                {
                    entity.AttributeName = name;
                    _attributeRepo.Update(entity);
                }
                else
                {
                    var newEntity = new Data.Entities.Attribute
                    {
                        AttributeName = name
                    };
                    await _attributeRepo.AddAsync(newEntity);
                }
                await _attributeRepo.SaveChangesAsync();
                return new SuccessResponse<bool>();
            }
            catch (Exception error)
            {
                return new FailResponse<bool>(error.Message);
            }
        }
        public async Task<Response<List<AttributeModel>>> getProductAttributes(InventoryRequest request)
        {
            try
            {
                var productId = request.productId;
                var subCategoryId = request.subCategoryId;

                if (productId > -1)
                {
                    var productQuery = _productAttributeRepo
                        .Entity()
                        .Where(i => i.ProductId == productId);
                    var attributes = await productQuery
                        .Select(i => new AttributeModel
                        {
                            id = i.AttributeId,
                            value = i.Value,
                            name = i.Attribute != null
                                ? i.Attribute.AttributeName
                                : "",
                        })
                        .ToListAsync();
                    return new SuccessResponse<List<AttributeModel>>(attributes);
                }
                if (subCategoryId > -1)
                {
                    var subCategoryAttrs = await _subCategoryAttributeRepo
                        .Entity()
                        .Where(i => i.SubCategoryId == subCategoryId)
                        .Select(i => new AttributeModel
                        {
                            id = i.Attribute.AttributeId,
                            name = i.Attribute.AttributeName
                        })
                        .ToListAsync();
                    return new SuccessResponse<List<AttributeModel>>(subCategoryAttrs);
                }
                return new SuccessResponse<List<AttributeModel>>();
            }
            catch (Exception error)
            {
                return new FailResponse<List<AttributeModel>>(error.Message);
            }
        } 
        public async Task<Response<List<SizeGuideModel>>> getProductSizeGuides()
        {
            try
            {
                var list = await _sizeGuideRepo.Entity()
                    .Select(i => new SizeGuideModel
                    {
                        id = i.SizeGuideId,
                        content = i.SizeContent,
                        isBase = (bool)i.IsBaseSizeGuide,
                        subCategories = i.SubCategories
                            .Where(sc => sc.SizeGuideId == i.SizeGuideId)
                            .Select(sc => new SubCategoryModel
                            {
                                id = sc.SubCategoryId,
                                name = sc.SubCategoryName
                            })
                            .ToList()
                    })
                    .ToListAsync();
                return new SuccessResponse<List<SizeGuideModel>>();
            }
            catch (Exception error)
            {
                return new FailResponse<List<SizeGuideModel>>(error.Message);
            }
        }
    }
}
