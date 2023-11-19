using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Common;
using ECommerce.Application.Enums;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services.Inventory;
using ECommerce.Application.Services.Inventory.Dtos;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.BaseServices.Rate;
using System.Text.RegularExpressions;

namespace ECommerce.Application.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ECommerceContext _DbContext;
        private readonly IRepositoryBase<Data.Models.Product> _productRepo;
        private readonly IRepositoryBase<Option> _optionRepo;
        private readonly IRepositoryBase<OptionValue> _optionValueRepo;
        private readonly IRepositoryBase<Brand> _brandRepo;
        private readonly IRepositoryBase<BrandCategory> _brandCategoryRepo;
        private readonly IRepositoryBase<SubCategory> _subCategoryRepo;
        private readonly IRepositoryBase<SubCategoryOption> _subCategoryOptionRepo;
        private readonly IRepositoryBase<ProductOptionValue> _productOptionValueRepo;
        private readonly IRepositoryBase<Shop> _shopRepo;
        private readonly IRepositoryBase<ProductImage> _productImageRepo;
        private readonly IRepositoryBase<ProductUserImage> _productUserImageRepo;
        private readonly IRepositoryBase<ProductAttribute> _productAttributeRepo;
        private readonly IRepositoryBase<Rate> _rateRepo;
        private readonly IRepositoryBase<Discount> _discountRepo;
        private readonly IInventoryService _inventoryService;
        private readonly IRateService _rateService;
        public ProductService(ECommerceContext DbContext, IInventoryService inventoryService, IRateService rateService)
        {
            _DbContext = DbContext;
            _inventoryService = inventoryService;
            _rateService = rateService;
            if (_productRepo == null)
                _productRepo = new RepositoryBase<Data.Models.Product>(_DbContext);
            if (_brandCategoryRepo == null)
                _brandCategoryRepo = new RepositoryBase<BrandCategory>(_DbContext);
            if (_subCategoryRepo == null)
                _subCategoryRepo = new RepositoryBase<SubCategory>(_DbContext);
            if (_optionRepo == null)
                _optionRepo = new RepositoryBase<Option>(_DbContext);
            if (_optionValueRepo == null)
                _optionValueRepo = new RepositoryBase<OptionValue>(_DbContext);
            if (_subCategoryOptionRepo == null)
                _subCategoryOptionRepo = new RepositoryBase<SubCategoryOption>(_DbContext);
            if (_productOptionValueRepo == null)
                _productOptionValueRepo = new RepositoryBase<ProductOptionValue>(_DbContext);
            if (_productImageRepo == null)
                _productImageRepo = new RepositoryBase<ProductImage>(_DbContext);
            if (_productUserImageRepo == null)
                _productUserImageRepo = new RepositoryBase<ProductUserImage>(_DbContext);
            if (_brandRepo == null)
                _brandRepo = new RepositoryBase<Brand>(_DbContext);
            if (_shopRepo == null)
                _shopRepo = new RepositoryBase<Shop>(_DbContext);
            if (_productAttributeRepo == null)
                _productAttributeRepo = new RepositoryBase<ProductAttribute>(_DbContext);
            if (_rateRepo == null)
                _rateRepo = new RepositoryBase<Rate>(_DbContext);
            if (_discountRepo == null)
                _discountRepo = new RepositoryBase<Discount>(_DbContext);
        }
        public async Task<Response<ProductModel>> getProductDetail(int id)
        {
            try
            {
                var rates = _rateRepo.Entity()
                    .Where(i => i.ProductId == id)
                    .Select(i => new RateModel
                    {
                        id = i.RateId,
                        value = i.RateValue,
                        comment = i.Comment,
                        htmlPosition = i.HtmlPosition,
                        repliedId = i.RepliedId,
                        parentId = i.ParentId,
                        productId = i.ProductId,
                        idsToDelete = i.IdsToDelete,

                        userId = i.UserId,
                        userRepliedId = i.UserRepliedId,
                        createDate = i.CreateDate,
                    })
                    .ToList();
                var ratesHasValue = rates
                    .Where(i => i.value > 0)
                    .ToList();
                var options = _optionRepo.Entity()
                    .Where(opt => _productOptionValueRepo.Entity()
                        .Any(pov => pov.ProductId == id && pov.OptionValue.OptionId == opt.OptionId))
                    .Select(opt => new OptionModel
                    {
                        id = opt.OptionId,
                        name = opt.OptionName,
                        values = _productOptionValueRepo.Entity()
                            .Where(pov =>
                                pov.ProductId == id && pov.OptionValue.OptionId == opt.OptionId)
                            .Select(pov => new OptionValueModel
                            {
                                id = pov.OptionValue.OptionValueId,
                                name = pov.OptionValue.OptionValueName,
                            })
                            .ToList()
                    })
                    .ToList();
                var attributes = _productAttributeRepo.Entity()
                    .Where(i => i.ProductId == id)
                    .Select(i => new AttributeModel
                    {
                        id = i.AttributeId,
                        value = i.Value,
                        name = i.Attribute.AttributeName,
                    })
                    .ToList();
                var imagePaths = _productImageRepo.Entity()
                    .Where(img => img.ProductId == id)
                    .Select(i => i.ProductImagePath)
                    .ToList();
                var userImagePaths = _productUserImageRepo.Entity()
                    .Where(img => img.ProductId == id)
                    .Select(i => i.ProductUserImagePath)
                    .ToList();
                var review = new ReviewModel
                {
                    avgValue = ratesHasValue.Count() > 0 ? (int)Math.Round((double)(ratesHasValue.Sum(i => i.value) / ratesHasValue.Count)) : 0,
                    totalRating = ratesHasValue.Count() > 0 ? ratesHasValue.Count() : 0,
                    rates = rates
                };

                var product = await _productRepo.Entity()
                    .Where(i => i.ProductId == id)
                    .Select(i => new ProductModel
                    {
                        id = i.ProductId,
                        code = i.ProductCode,
                        ppc = i.Ppc,
                        name = i.ProductName,
                        description = i.ProductDescription,
                        sizeGuide = i.SizeGuide,
                        size = i.SizeGuide,

                        delivery = i.Delivery,
                        repay = i.Repay,
                        insurance = i.Insurance,
                        isLegit = i.Legit,

                        brand = new BrandModel
                        {
                            id = i.BrandId,
                            name = i.Brand.BrandName,
                            description = i.Brand.Description,
                            descriptionTitle = i.Brand.DescriptionTitle,
                            imagePath = i.Brand.BrandImagePath,
                        },
                        shop = new ShopModel
                        {
                            id = i.ShopId,
                            name = i.Shop.ShopName,
                        },
                        attributes = attributes,
                        options = options,
                        importDate = i.ProductImportDate,

                        priceAvailable = i.PriceAvailable,
                        pricePreOrder = i.PricePreOrder,
                        discountAvailable = i.DiscountAvailable,
                        discountPreOrder = i.DiscountPreOrder,

                        imagePaths = imagePaths,
                        userImagePaths = userImagePaths,

                        review = review
                    })
                    .FirstOrDefaultAsync();

                return new SuccessResponse<ProductModel>(product);
            }
            catch (Exception error)
            {
                return new FailResponse<ProductModel>(error.Message);
            }
        }
        public async Task<Response<PageResult<ProductModel>>> getProductList(ProductGetRequest request)
        {
            try
            {
                //Request parameters
                int id = request.id ?? -1;
                int brandId = request.brandId;
                int subCategoryId = request.subCategoryId;
                int pageindex = request.PageIndex;
                int pagesize = request.PageSize;
                string orderBy = request.orderBy;
                List<int> listOptionValueId = request.optionValueIds;

                var proIdsByOption = _productOptionValueRepo.Entity()
                    .Where(i => listOptionValueId != null && listOptionValueId.Contains(i.OptionValueId))
                    .Select(i => i.ProductId)
                    .Distinct()
                    .ToList();

                var query = _productRepo.Entity()
                    .Join(_brandRepo.Entity(),
                        product => product.BrandId,
                        brand => brand.BrandId,
                        (product, brand) => new { product, brand })
                    .Join(_shopRepo.Entity(),
                        combined => combined.product.ShopId,
                        shop => shop.ShopId,
                        (combined, shop) => new { combined.product, combined.brand, shop })
                    .Where(combined => 
                        combined.product.BrandId == brandId &&
                        combined.product.Status == (int)ProductStatusEnum.Available)
                    .Select(combined => new { combined.product, combined.brand, combined.shop })
                    .Where(q =>
                        (proIdsByOption != null && proIdsByOption.Contains(q.product.ProductId)) ||
                        (brandId > -1 && q.product.BrandId == brandId) ||
                        (subCategoryId > -1 && q.product.SubCategoryId == subCategoryId) ||
                        (orderBy == "newest" ? q.product.New == true : q.product.New == false) ||
                        (orderBy == "discount" ? q.product.DiscountPercent > 0 : q.product.DiscountPercent > -1))
                    .OrderBy(combined => combined.product.SubCategoryId);

                var list = query
                    .Select(i => new ProductModel()
                    {
                        id = i.product.ProductId,
                        name = i.product.ProductName,
                        discountPercent = i.product.DiscountPercent,
                        status = i.product.Status,
                        isHighlights = i.product.Highlights,
                        isNew = i.product.New,
                        createdDate = i.product.ProductAddedDate,
                        importDate = i.product.ProductImportDate,
                        subCategoryId = i.product.SubCategoryId,

                        priceAvailable = i.product.PriceAvailable,
                        pricePreOrder = i.product.PricePreOrder,
                        discountAvailable = i.product.DiscountAvailable,
                        discountPreOrder = i.product.DiscountPreOrder,

                        imagePaths = _productImageRepo.Entity()
                            .Where(img => img.ProductId == i.product.ProductId)
                            .Select(i => i.ProductImagePath)
                            .ToList(),
                        brand = new BrandModel
                        {
                            id = i.product.BrandId,
                            name = i.product.Brand.BrandName,
                            description = i.product.Brand.Description,
                            descriptionTitle = i.product.Brand.DescriptionTitle,
                            imagePath = i.product.Brand.BrandImagePath,
                        },
                    });

                if (orderBy == "asc")
                {
                    list = list.OrderBy(i => i.discountPreOrder ?? i.discountAvailable ?? i.pricePreOrder ?? i.priceAvailable);
                }
                else if (orderBy == "desc")
                {
                    list = list.OrderByDescending(i => i.discountPreOrder ?? i.discountAvailable ?? i.pricePreOrder ?? i.priceAvailable);
                } 
                else
                {
                    list = list.OrderByDescending(i => i.createdDate);
                }

                var record = await list.CountAsync();
                var data = await PaginatedList<ProductModel>.CreateAsync(list, pageindex, pagesize);
                var result = new PageResult<ProductModel>()
                {
                    Items = data,
                    CurrentRecord = (pageindex * pagesize) <= record ? (pageindex * pagesize) : record,
                    TotalRecord = record,
                    CurrentPage = pageindex,
                    TotalPage = (int)Math.Ceiling(record / (double)pagesize)
                };

                return new SuccessResponse<PageResult<ProductModel>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<PageResult<ProductModel>>(error.Message);
            }
        }
        public async Task<Response<PageResult<ProductModel>>> getProductManagedList(ProductGetRequest request)
        {
            try
            {
                var id = request.id;
                var keyword = request.keyword.ToLower();
                var brandId = request.brandId;
                var shopId = request.shopId;
                var subCategoryId = request.subCategoryId;
                var categoryId = request.categoryId;
                int pageIndex = request.PageIndex;
                int pageSize = request.PageSize;

                var list = _productRepo
                    .Entity()
                    .Where(product =>
                        (id == -1 || product.ProductId == id) &&
                        (shopId == -1 || product.ShopId == shopId) &&
                        (brandId == -1 || product.BrandId == brandId) && 
                        (subCategoryId == -1 || product.SubCategoryId == subCategoryId) &&
                        (categoryId == -1 || 
                            (product.SubCategory != null &&
                             product.SubCategory.Category != null && 
                             product.SubCategory.Category.CategoryId == categoryId)) &&
                        (EF.Functions.Like(product.ProductCode, $"%{keyword}%") ||
                            EF.Functions.Like(product.Ppc, $"%{keyword}%") ||
                            EF.Functions.Like(product.ProductName, $"%{keyword}%")))
                    .Select(i => new ProductModel()
                    {
                        id = i.ProductId,
                        name = i.ProductName,
                        ppc = i.Ppc,
                        code = i.ProductCode,
                        createdDate = i.ProductAddedDate,
                        status = i.Status,
                        stock = (int)i.ProductStock,
                        subCategoryId = i.SubCategoryId,
                        subCategoryName = i.SubCategory.SubCategoryName,
                        categoryName = i.SubCategory.Category.CategoryName,
                        brand = new BrandModel
                        {
                            id = i.Brand.BrandId,
                            name = i.Brand.BrandName,
                        },
                        imagePaths = i.ProductImages
                            .Select(i => i.ProductImagePath)
                            .ToList(),

                        priceAvailable = i.PriceAvailable,
                        pricePreOrder = i.PricePreOrder,
                        discountAvailable = i.DiscountAvailable,
                        discountPreOrder = i.DiscountPreOrder,
                        priceImport = i.PriceImport,
                        priceForSeller = i.PriceForSeller,

                        profitAvailable = i.ProfitAvailable,
                        profitPreOrder = i.ProfitPreOrder,
                        profitForSeller = i.ProfitForSeller,

                        shop = new ShopModel(i.ShopId, i.Shop.ShopName),
                        note = i.Note,
                        link = i.Link
                    })
                    .OrderByDescending(i => i.id);

                var record = await list.CountAsync();
                var data = await PaginatedList<ProductModel>.CreateAsync(list, pageIndex, pageSize); // execute the query here
                var result = new PageResult<ProductModel>()
                {
                    Items = data,
                    CurrentRecord = (pageIndex * pageSize) <= record ? (pageIndex * pageSize) : record,
                    TotalRecord = record,
                    CurrentPage = pageIndex,
                    TotalPage = (int)Math.Ceiling(record / (double)pageSize)
                };

                return new SuccessResponse<PageResult<ProductModel>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<PageResult<ProductModel>>(error.Message);
            }
        }
        public async Task<Response<bool>> save(ProductSaveRequest request) // add or update
        {
            try
            {
                if (string.IsNullOrEmpty(request.name))
                    return new FailResponse<bool>("Vui lòng nhập tên sản phẩm");
                if (request.shop.id == 0)
                    return new FailResponse<bool>("Vui lòng chọn cửa hàng");
                if (request.brand.id == 0)
                    return new FailResponse<bool>("Vui lòng chọn thương hiệu");
                if (request.subCategoryId == 0)
                    return new FailResponse<bool>("Vui lòng chọn loại sản phẩm");
                // price
                if (request.priceAvailable == null && request.discountAvailable == null && request.pricePreOrder == null && request.discountPreOrder == null)
                    return new FailResponse<bool>("Vui lòng nhập giá sản phẩm");
                if (request.priceAvailable == null && request.discountAvailable != null)
                    return new FailResponse<bool>("Vui lòng nhập giá gốc trước khi nhập giảm giá !");
                if (request.priceAvailable < request.discountAvailable)
                    return new FailResponse<bool>("Giá giảm không thể lớn hơn giá gốc !");
                if (request.pricePreOrder == null && request.discountPreOrder != null)
                    return new FailResponse<bool>("Vui lòng nhập giá gốc trước khi nhập giảm giá !");
                if (request.pricePreOrder < request.discountPreOrder)
                    return new FailResponse<bool>("Giá giảm không thể lớn hơn giá gốc !");
                
                var hasCode = await _productRepo.Entity()
                    .Where(i => request.code != null && i.ProductCode == request.code.Trim())
                    .AnyAsync();
                if (hasCode)
                    return new FailResponse<bool>("Mã này đã tồn tại !");

                var isAdmin = true;
                /*
                 * None relationship data
                 */
                var product = new Data.Models.Product
                {
                    ProductCode = request.code.Trim(),
                    ProductName = request.name.Trim(), // required
                    Ppc = await getNewPPC(),
                    ProductDescription = request.description ?? null,
                    SizeGuide = request.size ?? null,
                    Note = string.IsNullOrEmpty(request.note) ? "" : request.note.Trim(),
                    Link = string.IsNullOrEmpty(request.link) ? "" : request.link.Trim(),
                    DiscountPercent = request.discountPercent ?? null,
                    Legit = request.isLegit,
                    Highlights = request.isHighlights,
                    Delivery = !string.IsNullOrEmpty(request.delivery) ? request.delivery.Trim() : null,
                    ProductStock = request.stock,
                    Repay = !string.IsNullOrEmpty(request.repay) ? request.repay.Trim() : null,
                    Insurance = string.IsNullOrEmpty(request.insurance) ? "" : request.insurance.Trim(),
                    New = request.isNew,
                    ShopId = (int)request.shop.id,
                    BrandId = (int)request.brand.id,
                    ProductAddedDate = DateTime.Now, // default
                    SubCategoryId = request.subCategoryId,
                    Status = isAdmin ? (byte?)ProductStatusEnum.Available : (byte?)ProductStatusEnum.Pending,

                    //Price
                    PriceAvailable = request.priceAvailable ?? null,
                    PricePreOrder = request.pricePreOrder ?? null,
                    PriceForSeller = request.priceForSeller ?? null,
                    PriceImport = request.priceImport ?? null,
                    DiscountAvailable = request.discountPercent != null
                                        ? getDiscountPrice(request.priceAvailable, request.discountPercent)
                                        : (request.discountAvailable ?? null),
                    DiscountPreOrder = request.discountPercent != null
                                        ? getDiscountPrice(request.pricePreOrder, request.discountPercent)
                                        : (request.discountPreOrder ?? null),
                    ProfitForSeller = request.priceForSeller - request.priceImport,
                    ProfitAvailable = request.discountPercent != null
                                        ? getDiscountPrice(request.priceAvailable, request.discountPercent) - request.priceImport
                                        : request.discountAvailable != null
                                            ? (request.discountAvailable - request.priceImport)
                                            : (request.priceAvailable - request.priceImport),
                    ProfitPreOrder = request.discountPercent != null
                                        ? getDiscountPrice(request.pricePreOrder, request.discountPercent) - request.priceImport
                                        : request.discountPreOrder != null
                                            ? (request.discountPreOrder - request.priceImport)
                                            : (request.pricePreOrder - request.priceImport),
                };
                await _productRepo.AddAsync(product);
                await _productRepo.SaveChangesAsync();

                /*
                 * Relationship data
                 */
                // Options
                List<int> optionValueIds = request.currentOptions;
                if (optionValueIds.Count > 0)
                {
                    var productOptionValues = optionValueIds.Select(id => new ProductOptionValue
                    {
                        ProductId = product.ProductId,
                        OptionValueId = id
                    }).ToList();
                    await _productOptionValueRepo.AddRangeAsync(productOptionValues);
                    await _productOptionValueRepo.SaveChangesAsync();
                }
                // New option value
                List<OptionModel> newOptions = request.newOptions;
                if (newOptions.Count > 0)
                {
                    foreach (var option in newOptions)
                    {
                        var optionId = option.id;
                        var existingOptionValues = await _optionValueRepo.Entity()
                            .Where(i => i.OptionId == optionId)
                            .Select(i => i.OptionValueName)
                            .ToListAsync();

                        // Get new values of list;
                        // Lấy các giá trị khác với db;
                        var newOptionValues = option.values
                            .Select(value => value.name.Trim())
                            .Except(existingOptionValues)
                            .ToList();

                        var currentOptionValues = option.values
                            .Select(value => value.name.Trim())
                            .Intersect(existingOptionValues)
                            .ToList();

                        if (newOptionValues.Any())
                        {
                            var newOptionValueEntities = newOptionValues
                                .Select(value => new OptionValue
                                {
                                    OptionId = optionId,
                                    OptionValueName = value.Trim(),
                                    IsBaseValue = false
                                })
                                .ToList();

                            await _optionValueRepo.AddRangeAsync(newOptionValueEntities);
                            await _optionValueRepo.SaveChangesAsync();

                            var newOptionValueIds = newOptionValueEntities.Select(e => e.OptionValueId).ToList();
                            await addProductOptionValueByProductId(product.ProductId, newOptionValueIds);
                        }

                        if (currentOptionValues.Any())
                        {
                            var currentOptionValueIds = await _optionValueRepo.Entity()
                                .Where(i => i.OptionId == optionId && currentOptionValues.Contains(i.OptionValueName))
                                .Select(i => i.OptionValueId)
                                .ToListAsync();

                            await addProductOptionValueByProductId(product.ProductId, currentOptionValueIds);
                        }
                    }
                }

                // Attribute
                var attributes = request.attributes.Where(a => !string.IsNullOrEmpty(a.value));
                if (attributes != null && attributes.Any())
                {

                    var attrAddReq = attributes
                        .Select(attr => new ProductAttribute
                        {
                            ProductId = product.ProductId,
                            AttributeId = attr.id,
                            Value = attr.value.Trim()
                        })
                        .ToList();
                    await _productAttributeRepo.AddRangeAsync(attrAddReq);
                    await _productAttributeRepo.SaveChangesAsync();
                }

                // Images
                if (request.systemFileNames != null)
                {
                    var sysImgAddReq = request.systemFileNames
                        .Select(img => new ProductImage
                        {
                            ProductId = product.ProductId,
                            ProductImagePath = img
                        })
                        .ToList();
                    await _productImageRepo.AddRangeAsync(sysImgAddReq);
                    await _productImageRepo.SaveChangesAsync();
                }
                if (request.userFileNames != null)
                {
                    var userImgAddReq = request.systemFileNames
                        .Select(img => new ProductUserImage
                        {
                            ProductId = product.ProductId,
                            ProductUserImagePath = img
                        })
                        .ToList();
                    await _productUserImageRepo.AddRangeAsync(userImgAddReq);
                    await _productUserImageRepo.SaveChangesAsync();
                }

                return new SuccessResponse<bool>();
            }
            catch (Exception error)
            {
                return new FailResponse<bool>(error.Message);
            }
        }
        public async Task<Response<ProductDeleteResponse>> delete(ProductDeleteRequest request)
        {
            try
            {
                var ids = request.ids;
                // Check null product
                var products = await _DbContext.Products.Where(i => ids.Contains(i.ProductId)).ToListAsync();
                if (products == null || products.Count == 0) return new FailResponse<ProductDeleteResponse>("Sản phẩm không tồn tại");

                // System's iamges
                var sysImages = await _DbContext.ProductImages.Where(i => ids.Contains(i.ProductId)).ToListAsync();
                if (sysImages.Count > 0) _DbContext.ProductImages.RemoveRange(sysImages);
                // User's images
                var userImages = await _DbContext.ProductUserImages.Where(i => ids.Contains((int)i.ProductId)).ToListAsync();
                if (userImages.Count > 0) _DbContext.ProductUserImages.RemoveRange(userImages);
                // Prices
                var productPrices = await _DbContext.ProductPrices.Where(i => ids.Contains(i.ProductId)).ToListAsync();
                if (productPrices.Count > 0) _DbContext.ProductPrices.RemoveRange(productPrices);
                // Attributes
                var attrs = await _DbContext.ProductAttributes.Where(i => ids.Contains(i.ProductId)).ToListAsync();
                if (attrs.Count > 0) _DbContext.ProductAttributes.RemoveRange(attrs);
                // Options
                var opts = await _DbContext.ProductOptionValues.Where(i => ids.Contains(i.ProductId)).ToListAsync();
                if (opts.Count > 0) _DbContext.ProductOptionValues.RemoveRange(opts);
                // Product's comments
                var ratingImages = new List<string>();
                var rateRs = await _rateService.DeleteCommentsByProductIds(ids);
                ratingImages = rateRs.isSucceed ? rateRs.Data : null;

                var data = new ProductDeleteResponse
                {
                    systemImages = sysImages.Select(i => i.ProductImagePath).ToList(),
                    userImages = userImages.Select(i => i.ProductUserImagePath).ToList(),
                    ratingImages = ratingImages
                };

                // Remove product and save changes
                _DbContext.Products.RemoveRange(products);
                _DbContext.SaveChangesAsync().Wait();
                return new SuccessResponse<ProductDeleteResponse>();
            }
            catch (Exception error)
            {
                return new FailResponse<ProductDeleteResponse>(error.Message);
            }
        }
        public async Task<Response<DiscountModel>> getDiscount(DiscountGetRequest request)
        {
            try
            {
                var now = DateTime.Now;
                var result = await _discountRepo
                    .Entity()
                    .Where(i =>
                        i.StartDate <= now &&
                        i.EndDate >= now &&
                        i.Status == 1)
                    .Select(i => new DiscountModel
                    {
                        id = i.DiscountId,
                        code = i.DiscountCode,
                        value = i.DiscountValue,
                        type = (bool)i.ForGlobal ? DiscountTypeEnum.global :
                               (bool)i.ForShop ? DiscountTypeEnum.shop :
                               (bool)i.ForBrand ? DiscountTypeEnum.brand : DiscountTypeEnum.global,
                        isPercent = i.IsPercent,
                    })
                    .FirstOrDefaultAsync();

                return new SuccessResponse<DiscountModel>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<DiscountModel>(error.Message);
            }
        }
        public async Task<Response<bool>> updateStatus(UpdateStatusRequest request)
        {
            try
            {
                List<int> ids = request.ids;
                int status = request.status;

                if (ids.Count == 0) 
                    return new FailResponse<bool>("Vui lòng chọn sản phẩm cần cập nhật");
                var productsToUpdate = await _productRepo.ToListAsyncWhere(i => ids.Contains(i.ProductId));
                if (productsToUpdate.Count() > 0)
                {
                    foreach (var product in productsToUpdate)
                    {
                        product.Status = (byte?)status;
                    }
                    await _productRepo.SaveChangesAsync();
                }
                return new SuccessResponse<bool>("Cập nhật thành công");
            }
            catch (Exception error)
            {
                return new FailResponse<bool>("Cập nhật thất bại " + error);
            }
        }
        // private functions
        private async Task addProductOptionValueByProductId(int productId, List<int> optValIds)
        {
            var productOptionValues = optValIds.Select(optValId => new ProductOptionValue
            {
                ProductId = productId,
                OptionValueId = optValId
            });

            await _productOptionValueRepo.AddRangeAsync(productOptionValues);
            await _productOptionValueRepo.SaveChangesAsync();
        }
        private decimal? getDiscountPrice(decimal? price, byte? percent)
        {
            var priceDiscounted = price - (price * percent / 100);
            return (decimal)priceDiscounted;
        }
        private async Task<string> getNewPPC()
        {
            var newestId = await _productRepo
                .Entity()
                .OrderByDescending(s => s.ProductId)
                .Select(i => i.ProductId)
                .FirstOrDefaultAsync();

            byte[] buffer = Guid.NewGuid().ToByteArray();
            string guid = BitConverter.ToUInt32(buffer, 8).ToString();

            int len = (newestId + 1).ToString().Length;
            var ppc = guid.Substring(0, 8 - len) + (newestId + 1).ToString();

            return ppc;
        }
        private string RemoveDiacritics(string text)
        {
            string decomposed = text.Normalize(NormalizationForm.FormD);
            Regex regex = new Regex("\\p{Mn}", RegexOptions.Compiled);

            return regex.Replace(decomposed, string.Empty).Normalize(NormalizationForm.FormC);
        }
    }
}
