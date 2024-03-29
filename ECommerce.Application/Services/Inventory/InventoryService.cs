﻿using ECommerce.Application.BaseServices.FilterProduct.Dtos;
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
        public InventoryService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
            if (_productRepo == null)
                _productRepo = new RepositoryBase<Data.Entities.Product>(_DbContext);
            if (_brandRepo == null)
                _brandRepo = new RepositoryBase<Brand>(_DbContext);
            if (_categoryRepo == null)
                _categoryRepo = new RepositoryBase<Category>(_DbContext);
            if (_brandCategoryRepo == null)
                _brandCategoryRepo = new RepositoryBase<BrandCategory>(_DbContext);
            if (_shopBrandRepo == null)
                _shopBrandRepo = new RepositoryBase<ShopBrand>(_DbContext);
            if (_subCategoryRepo == null)
                _subCategoryRepo = new RepositoryBase<SubCategory>(_DbContext);
            if (_optionRepo == null)
                _optionRepo = new RepositoryBase<Data.Entities.Option>(_DbContext);
            if (_optionValueRepo == null)
                _optionValueRepo = new RepositoryBase<Data.Entities.OptionValue>(_DbContext);
            if (_subCategoryOptionRepo == null)
                _subCategoryOptionRepo = new RepositoryBase<SubCategoryOption>(_DbContext);
            if (_subCategoryAttributeRepo == null)
                _subCategoryAttributeRepo = new RepositoryBase<SubCategoryAttribute>(_DbContext);
            if (_productOptionValuesRepo == null)
                _productOptionValuesRepo = new RepositoryBase<ProductOptionValue>(_DbContext);
            if (_productAttributeRepo == null)
                _productAttributeRepo = new RepositoryBase<ProductAttribute>(_DbContext);
            if (_sizeGuideRepo == null)
                _sizeGuideRepo = new RepositoryBase<SizeGuide>(_DbContext);
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
                var res = (await _categoryRepo.ToListAsync())
                    .Select(_ => (CategoryModel)_)
                    .ToList();
                return new SuccessResponse<List<CategoryModel>>(res);
            }
            catch (Exception error)
            {
                return new FailResponse<List<CategoryModel>>(error.Message);
            }
        }
        public async Task<Response<List<SubCategoryModel>>> getSubCategories(InventoryRequest request)
        {
            try
            {
                var subCategoryId = request.subCategoryId;
                var brandId = request.brandId;

                var query = _subCategoryRepo.Query();
                if (brandId > -1 || subCategoryId > -1)
                {
                    query = query
                        .Where(subc =>
                            subc.SubCategoryId == subCategoryId ||
                            _brandCategoryRepo
                                .Entity()
                                .Any(brand => subc.CategoryId == brand.CategoryId && brand.BrandId == brandId));
                }

                var list = await query
                    .Select(subc => new SubCategoryModel
                    {
                        id = subc.SubCategoryId,
                        name = subc.SubCategoryName,
                        categoryId = subc.CategoryId,
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
                                .Where(pov => !isBase || (isBase && pov.isBase))
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
