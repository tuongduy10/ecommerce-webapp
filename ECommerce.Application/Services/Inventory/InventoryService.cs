using ECommerce.Application.BaseServices.FilterProduct.Dtos;
using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services.Inventory.Dtos;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
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
        private readonly IRepositoryBase<Data.Models.Product> _productRepo;
        private readonly IRepositoryBase<Data.Models.Option> _optionRepo;
        private readonly IRepositoryBase<Data.Models.OptionValue> _optionValueRepo;
        private readonly IRepositoryBase<Brand> _brandRepo;
        private readonly IRepositoryBase<BrandCategory> _brandCategoryRepo;
        private readonly IRepositoryBase<SubCategory> _subCategoryRepo;
        private readonly IRepositoryBase<SubCategoryOption> _subCategoryOptionRepo;
        private readonly IRepositoryBase<ProductOptionValue> _productOptionValuesRepo;
        private readonly IRepositoryBase<ProductAttribute> _productAttributeRepo;
        private readonly IRepositoryBase<SizeGuide> _sizeGuideRepo;
        public InventoryService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
            if (_productRepo == null)
                _productRepo = new RepositoryBase<Data.Models.Product>(_DbContext);
            if (_brandCategoryRepo == null)
                _brandCategoryRepo = new RepositoryBase<BrandCategory>(_DbContext);
            if (_subCategoryRepo == null)
                _subCategoryRepo = new RepositoryBase<SubCategory>(_DbContext);
            if (_optionRepo == null)
                _optionRepo = new RepositoryBase<Data.Models.Option>(_DbContext);
            if (_optionValueRepo == null)
                _optionValueRepo = new RepositoryBase<Data.Models.OptionValue>(_DbContext);
            if (_subCategoryOptionRepo == null)
                _subCategoryOptionRepo = new RepositoryBase<SubCategoryOption>(_DbContext);
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
                    .Query()
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
        public async Task<Response<List<SubCategoryModel>>> getSubCategories(int brandId)
        {
            try
            {
                var list = await _subCategoryRepo.Entity()
                    .Where(subc => _brandCategoryRepo.Entity()
                        .Any(brand => subc.CategoryId == brand.CategoryId && brand.BrandId == brandId))
                    .Select(subc => new SubCategoryModel
                    {
                        id = subc.SubCategoryId,
                        name = subc.SubCategoryName,
                        categoryId = subc.CategoryId,
                        optionList = _subCategoryOptionRepo.Entity()
                            .Where(subcopt => subcopt.SubCategoryId == subc.SubCategoryId)
                            .Select(lo => new OptionModel
                            {
                                id = lo.OptionId,
                                name = lo.Option.OptionName,
                                values = _optionValueRepo.Entity()
                                    .Where(optionvalue => optionvalue.OptionId == lo.OptionId)
                                    .Select(lop => new OptionValueModel
                                    {
                                        id = lop.OptionValueId,
                                        name = lop.OptionValueName,
                                        totalRecord = _productOptionValuesRepo.Entity()
                                            .Where(ov => ov.OptionValueId == lop.OptionValueId &&
                                                            ov.Product.SubCategoryId == subc.SubCategoryId)
                                            .Select(ov => ov.ProductId)
                                            .Count(),
                                    })
                                    .ToList()
                            })
                            .ToList(),
                    })
                    .ToListAsync();

                return new SuccessResponse<List<SubCategoryModel>>(list);
            }
            catch(Exception error)
            {
                return new FailResponse<List<SubCategoryModel>>(error.Message);
            }
        }
        public async Task<Response<List<OptionModel>>> getProductOptions(int productId)
        {
            try
            {
                var result = await _DbContext.Options
                    .Where(opt => _DbContext.ProductOptionValues
                        .Any(pov => pov.ProductId == productId && pov.OptionValue.OptionId == opt.OptionId))
                    .Select(opt => new OptionModel
                    {
                        id = opt.OptionId,
                        name = opt.OptionName,
                        values = _DbContext.ProductOptionValues
                            .Where(pov => 
                                pov.ProductId == productId && pov.OptionValue.OptionId == opt.OptionId)
                            .Select(pov => new OptionValueModel
                            {
                                id = pov.OptionValue.OptionValueId,
                                name = pov.OptionValue.OptionValueName,
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return new SuccessResponse<List<OptionModel>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<List<OptionModel>>(error.Message);
            }
        }
        public async Task<Response<List<AttributeModel>>> getProductAttributes(int productId)
        {
            try
            {
                var attributes = await _productAttributeRepo.Entity()
                    .Where(i => i.ProductId == productId)
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
