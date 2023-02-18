using ECommerce.Application.Common;
using ECommerce.Application.Helpers;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.Product.Enum;
using ECommerce.Application.Services.Product.Models;
using ECommerce.Application.Services.Product.Response;
using ECommerce.Application.Services.Rate;
using ECommerce.Application.Services.SubCategory.Dtos;
using ECommerce.Application.Services.User;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product
{
    public class ProductService : IProductService
    {
        private ECommerceContext _DbContext;
        private IUserService _userService;
        private IRateService _rateService;
        public ProductService(
            ECommerceContext DbContext, 
            IUserService userService, 
            IRateService rateService
        ) {
            _DbContext = DbContext;
            _userService = userService;
            _rateService = rateService;
        }
        public async Task<List<ProductShopListModel>> getAllManaged(int subcategoryId)
        {
            var query = from products in _DbContext.Products
                        select products;
            if (subcategoryId != 0)
            {
                query = query.Where(i => i.SubCategoryId == subcategoryId);
            }

            var result = await query
                .Where(i => i.Status == 1)
                .Select(i => new ProductShopListModel()
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    PPC = i.Ppc,
                    ProductCode = i.ProductCode,
                    CreatedDate = i.ProductAddedDate,
                    Status = i.Status,
                    Stock = (int)i.ProductStock,
                    SubCategoryName = i.SubCategory.SubCategoryName,
                    CategoryName = i.SubCategory.Category.CategoryName,
                    BrandName = i.Brand.BrandName,
                    ProductImages = i.ProductImages.Select(i => i.ProductImagePath).FirstOrDefault(),
                    Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.ProductId).ToList(),
                    ShopName = i.Shop.ShopName,
                    ShopId = i.ShopId,
                    BrandId = i.BrandId,
                    Note = i.Note,
                    Link = i.Link
                })
                .ToListAsync();
            var list = result.OrderByDescending(i => i.ProductId).ToList();
            return list;
        }
        public async Task<List<Dtos.Option>> getProductOption(int productId)
        {
            // get Options by product id
            var opts = from opt in _DbContext.Options
                       from opt_v in _DbContext.OptionValues
                       from pro_optv in _DbContext.ProductOptionValues
                       where pro_optv.ProductId == productId &&
                             pro_optv.OptionValueId == opt_v.OptionValueId &&
                             opt_v.OptionId == opt.OptionId
                       select opt;

            // get option ids distincted
            var opt_ids = await opts.Select(i => i.OptionId).Distinct().ToListAsync();

            // list option with option_values 
            var result = new List<Dtos.Option>();
            foreach (var id in opt_ids)
            {
                var opt_v = await _DbContext.Options
                                    .Where(i => i.OptionId == id)
                                    .Select(i => new Dtos.Option() {
                                        id = i.OptionId,
                                        name = i.OptionName,
                                        values = _DbContext.ProductOptionValues
                                                .Where(pov => pov.ProductId == productId && pov.OptionValue.OptionId == i.OptionId)
                                                .Select(pov => pov.OptionValue.OptionValueName)
                                                .ToList()
                                    })
                                    .FirstOrDefaultAsync();
                result.Add(opt_v);
            }

            return result;
        }
        public async Task<ProductDetailModel> getProductDetail(int id)
        {
            var attr = await _DbContext.ProductAttributes
                .Where(i => i.ProductId == id)
                .Select(i => new Dtos.Attribute() { 
                    Value = i.Value,
                    AttrName = i.Attribute.AttributeName
                })
                .ToListAsync();

            var rate = await _DbContext.Rates
                .Where(i => i.ProductId == id && i.RateValue != 0)
                .ToListAsync();

            int value = 0;
            int total = 0;
            if (rate.Count != 0)
            {
                value = (int)Math.Round((double)(rate.Sum(i => i.RateValue) / rate.Count));
                total = rate.Count;
            }
            Dtos.Rate productRate = new Dtos.Rate()
            {
                Value = value,
                Total = total
            };

            var product = await _DbContext.Products
                .Where(i => i.ProductId == id && i.Status == 1)
                .Select(i => new ProductDetailModel()
                {
                    ProductId = i.ProductId,
                    ProductCode = i.ProductCode,
                    PPC = i.Ppc,
                    ProductName = i.ProductName,
                    ProductDescription = i.ProductDescription,
                    ProductDescriptionMobile = i.ProductDescriptionMobile,
                    SizeGuide = i.SizeGuide,

                    FreeDelivery = i.FreeDelivery,
                    FreeReturn = i.FreeReturn,
                    Legit = i.Legit,
                    Insurance = i.Insurance,

                    SubCategoryId = i.SubCategoryId,
                    BrandId = i.Brand.BrandId,
                    BrandName = i.Brand.BrandName,
                    ShopName = i.Shop.ShopName,
                    ShopId = i.Shop.ShopId,
                    ProductRate = productRate,

                    Attributes = attr,
                    ProductImportDate = i.ProductImportDate,
                    DiscountPercent = i.DiscountPercent,
                    Prices = _DbContext.ProductPrices
                        .Where(price => price.ProductId == id)
                        .ToList(),
                    Types = _DbContext.ProductTypes
                        .Select(type => new Dtos.Type() { 
                            ProductTypeName = type.ProductTypeName,
                            ProductTypeId = type.ProductTypeId
                        }).ToList(),

                    ProductImages = _DbContext.ProductImages
                        .Where(img => img.ProductId == id)
                        .Select(img => img.ProductImagePath)
                        .ToList(),
                    ProductUserImages = _DbContext.ProductUserImages
                        .Where(img => img.ProductId == id)
                        .Select(img => img.ProductUserImagePath)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return product;
        }
        public async Task<ProductDetailModel> GetProductDetailManaged(int id)
        {
            var rate = await _DbContext.Rates.Where(i => i.ProductId == id).ToListAsync();
            Dtos.Rate productRate = new Dtos.Rate()
            {
                Value = rate.Count == 0 ? 0 : (int)Math.Round((double)(rate.Sum(i => i.RateValue) / rate.Count)),
                Total = rate.Count == 0 ? 0 : rate.Count
            };

            var sysImages = await _DbContext.ProductImages
                .Where(i => i.ProductId == id)
                .ToListAsync();
            var userImages = await _DbContext.ProductUserImages
                .Where(i => i.ProductId == id)
                .ToListAsync();

            var product = await _DbContext.Products
                .Where(i => i.ProductId == id)
                .Select(i => new ProductDetailModel()
                {
                    ProductId = i.ProductId,
                    ProductCode = i.ProductCode,
                    PPC = i.Ppc,
                    ProductName = i.ProductName,
                    ProductDescription = i.ProductDescription,
                    ProductDescriptionMobile = i.ProductDescriptionMobile,
                    SizeGuide = i.SizeGuide,
                    Note = i.Note,
                    Link = i.Link,
                    Stock = (int)i.ProductStock,

                    New = i.New,
                    Highlight = i.Highlights,
                    FreeDelivery = i.FreeDelivery,
                    FreeReturn = i.FreeReturn,
                    Legit = i.Legit,
                    Insurance = i.Insurance,

                    SubCategoryId = i.SubCategoryId,
                    BrandId = i.BrandId,                                                
                    ShopId = i.ShopId,
                    ProductRate = productRate,

                    //Attributes = attr,
                    Attributes = _DbContext.SubCategoryAttributes
                        .Where(sub_attr => sub_attr.SubCategoryId == i.SubCategoryId)
                        .Select(sub_attr => new Dtos.Attribute() {
                            Id = sub_attr.AttributeId,
                            AttrName = sub_attr.Attribute.AttributeName,
                            Value = sub_attr.Attribute.ProductAttributes
                                .Where(attr => attr.AttributeId == sub_attr.AttributeId && attr.ProductId == id)
                                .Select(attr => attr.Value)
                                .FirstOrDefault()
                        })
                        .ToList(),
                    //Options = opts,
                    Options = _DbContext.Options
                        .Where(sub_opt => sub_opt.SubCategoryOptions.Any(so => so.SubCategoryId == i.SubCategoryId))
                        .Select(sub_opt => new OptionGetModel()
                        {
                            id = sub_opt.OptionId,
                            name = sub_opt.OptionName,
                            values = sub_opt.OptionValues
                                .Where( ov => 
                                    ov.OptionId == sub_opt.OptionId &&
                                    ov.ProductOptionValues.Any(pov => pov.ProductId == id && pov.OptionValueId == ov.OptionValueId)
                                )
                                .Select(ov => new OptionValueGetModel() { 
                                    id = ov.OptionValueId,
                                    value = ov.OptionValueName
                                })
                                .ToList()
                        })
                        .ToList(),
                    ProductImportDate = i.ProductImportDate,
                    DiscountPercent = i.DiscountPercent,
                    Prices = _DbContext.ProductPrices
                        .Where(price => price.ProductId == id)
                        .ToList(),
                    Types = _DbContext.ProductTypes
                        .Select(type => new Dtos.Type()
                        {
                            ProductTypeName = type.ProductTypeName,
                            ProductTypeId = type.ProductTypeId
                        }).ToList(),

                    ProductImages = _DbContext.ProductImages.Where(img => img.ProductId == id).Select(img => img.ProductImagePath).ToList(), // for view only
                    ProductUserImages = _DbContext.ProductUserImages.Where(img => img.ProductId == id).Select(img => img.ProductUserImagePath).ToList(), // for view only

                    SystemImages = sysImages,
                    UserImages = userImages
                })
                .FirstOrDefaultAsync();
            return product;
        }
        // Product in brand
        public async Task<PageResult<ProductInBrandModel>> getProductPaginated(ProductGetRequest request)
        {
            try
            {
                //Request parameters
                int BrandId = request.BrandId;
                int SubCategoryId = request.SubCategoryId;
                int pageindex = request.PageIndex;
                int pagesize = request.PageSize;
                string orderBy = request.OrderBy;
                List<int> listOptionValueId = request.listOptionValueId;

                // Query
                var query = (from product in _DbContext.Products
                             join brand in _DbContext.Brands on product.BrandId equals brand.BrandId
                             join shop in _DbContext.Shops on product.ShopId equals shop.ShopId
                             where product.BrandId == BrandId && product.Status == (int)enumProductStatus.Available
                             orderby product.SubCategoryId
                             select new { product, brand, shop }).AsQueryable();

                // Query get products by brandid and subcategoryid
                if (BrandId > 0 && SubCategoryId > 0)
                {
                    query = query.Where(q => q.product.SubCategoryId == SubCategoryId);
                }
                // Query get products by optionvalue
                if (BrandId > 0 && listOptionValueId != null)
                {

                    // product ids from ProductOptionValues
                    var optProIds = await _DbContext.ProductOptionValues
                        .Where(i => listOptionValueId.Contains(i.OptionValueId))
                        .Select(i => i.ProductId)
                        .ToListAsync();
                    optProIds = optProIds.Distinct().ToList(); // 

                    var proIds = await _DbContext.Products
                        .Where(i => optProIds.Contains(i.ProductId) && i.BrandId == BrandId && i.SubCategoryId == SubCategoryId)
                        .Select(i => i.ProductId)
                        .ToListAsync();

                    query = query.Where(q => proIds.Contains(q.product.ProductId));
                }

                // Order by... request
                if (orderBy == "Newest") query = query.Where(q => q.product.New == true);
                if (orderBy == "Discount") query = query.Where(q => q.product.DiscountPercent > 0);

                // Select from query
                var list = query
                    .Where(i => i.product.Status == 1)
                    .OrderByDescending(i => i.product.ProductAddedDate)
                    .Select(i => new ProductInBrandModel()
                    {
                        ProductId = i.product.ProductId,
                        ProductName = i.product.ProductName,
                        DiscountPercent = i.product.DiscountPercent,
                        Status = i.product.Status,
                        Highlights = i.product.Highlights,
                        New = i.product.New,
                        ProductImportDate = i.product.ProductImportDate,
                        SubCategoryId = i.product.SubCategoryId,
                        Type = (
                            from product in _DbContext.Products
                            from price in _DbContext.ProductPrices
                            from type in _DbContext.ProductTypes
                            where product.ProductId == i.product.ProductId &&
                                  price.ProductTypeId == type.ProductTypeId && price.ProductId == product.ProductId
                            select new { id = type.ProductTypeId, name = type.ProductTypeName }
                        ).Select(t => new Dtos.Type()
                        {
                            ProductTypeId = t.id,
                            ProductTypeName = t.name
                        }).ToList(),

                        Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.product.ProductId).ToList(),
                        ProductImages = _DbContext.ProductImages.Where(img => img.ProductId == i.product.ProductId).Select(i => i.ProductImagePath).FirstOrDefault(),
                        BrandName = i.brand.BrandName,
                        ShopName = i.shop.ShopName,
                    });

                var record = await list.CountAsync();
                var data = await PaginatedList<ProductInBrandModel>.CreateAsync(list, pageindex, pagesize);
                var result = new PageResult<ProductInBrandModel>()
                {
                    Items = data,
                    CurrentRecord = (pageindex * pagesize) <= record ? (pageindex * pagesize) : record,
                    TotalRecord = record,
                    CurrentPage = pageindex,
                    TotalPage = (int)Math.Ceiling(record / (double)pagesize)
                };

                return result;
            }
            catch(Exception error)
            {
                throw error;
            }
        }
        // Hotsale, new product only
        public async Task<PageResult<ProductInBrandModel>> getProductInPagePaginated(ProductGetRequest request)
        {
            //Request parameters
            int pageindex = request.PageIndex;
            int pagesize = request.PageSize;

            // Query
            var query = (from product in _DbContext.Products
                         join brand in _DbContext.Brands on product.BrandId equals brand.BrandId
                         join shop in _DbContext.Shops on product.ShopId equals shop.ShopId
                         orderby product.SubCategoryId
                         select new { product, brand, shop }).AsQueryable();

            // Get by... request for 1 page only
            if (request.GetBy == "Newest") query = query.Where(q => q.product.New == true);
            if (request.GetBy == "Discount") query = query.Where(q => q.product.DiscountPercent > 0);

            // order by... request
            if (request.OrderBy == "Newest") query = query.Where(q => q.product.New == true);
            if (request.OrderBy == "Discount") query = query.Where(q => q.product.DiscountPercent > 0);

            // Select from query
            var list = query
                .Where(i => i.product.Status == 1)
                .OrderByDescending(i => i.product.ProductAddedDate)
                .Select(i => new ProductInBrandModel()
                {
                    ProductId = i.product.ProductId,
                    ProductName = i.product.ProductName,
                    DiscountPercent = i.product.DiscountPercent,
                    Status = i.product.Status,
                    Highlights = i.product.Highlights,
                    New = i.product.New,
                    ProductImportDate = i.product.ProductImportDate,
                    SubCategoryId = i.product.SubCategoryId,
                    Type = (
                        from product in _DbContext.Products
                        from price in _DbContext.ProductPrices
                        from type in _DbContext.ProductTypes
                        where product.ProductId == i.product.ProductId &&
                              price.ProductTypeId == type.ProductTypeId && price.ProductId == product.ProductId
                        select new { id = type.ProductTypeId, name = type.ProductTypeName }
                    ).Select(t => new Dtos.Type()
                    {
                        ProductTypeId = t.id,
                        ProductTypeName = t.name
                    }).ToList(),

                    Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.product.ProductId).ToList(),
                    ProductImages = _DbContext.ProductImages.Where(img => img.ProductId == i.product.ProductId).Select(i => i.ProductImagePath).FirstOrDefault(),
                    BrandName = i.brand.BrandName,
                    ShopName = i.shop.ShopName,
                });

            var record = await list.CountAsync();
            var data = await PaginatedList<ProductInBrandModel>.CreateAsync(list, pageindex, pagesize);
            var result = new PageResult<ProductInBrandModel>()
            {
                Items = data,
                CurrentRecord = (pageindex * pagesize) <= record ? (pageindex * pagesize) : record,
                TotalRecord = record,
                CurrentPage = pageindex,
                TotalPage = (int)Math.Ceiling(record / (double)pagesize)
            };

            return result;
        }
        public async Task<List<ProductInBrandModel>> getProductSuggestion()
        {
            // Query
            var query = (from product in _DbContext.Products
                         join brand in _DbContext.Brands on product.BrandId equals brand.BrandId
                         join shop in _DbContext.Shops on product.ShopId equals shop.ShopId
                         orderby product.SubCategoryId
                         select new { product, brand, shop }).AsQueryable();
            // Select from query
            var list = await query
                .Where(i => i.product.Highlights == true && i.product.Status == 1)
                .OrderByDescending(i => i.product.ProductAddedDate)
                .Take(12)
                .Select(i => new ProductInBrandModel() {
                    ProductId = i.product.ProductId,
                    ProductName = i.product.ProductName,
                    DiscountPercent = i.product.DiscountPercent,
                    Status = i.product.Status,
                    Highlights = i.product.Highlights,
                    New = i.product.New,
                    ProductImportDate = i.product.ProductImportDate,
                    SubCategoryId = i.product.SubCategoryId,
                    Type = (
                        from product in _DbContext.Products
                        from price in _DbContext.ProductPrices
                        from type in _DbContext.ProductTypes
                        where product.ProductId == i.product.ProductId &&
                              price.ProductTypeId == type.ProductTypeId && price.ProductId == product.ProductId
                        select new { id = type.ProductTypeId, name = type.ProductTypeName }
                    ).Select(t => new Dtos.Type()
                    {
                        ProductTypeId = t.id,
                        ProductTypeName = t.name
                    }).ToList(),

                    Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.product.ProductId).ToList(),
                    ProductImages = _DbContext.ProductImages.Where(img => img.ProductId == i.product.ProductId).Select(i => i.ProductImagePath).FirstOrDefault(),
                    BrandName = i.brand.BrandName,
                    ShopName = i.shop.ShopName,
                })
                .ToListAsync();

            return list;
        }
        public async Task<List<ProductShopListModel>> getProductByUser(int userId, int subcategoryId)
        {
            var query = from products in _DbContext.Products
                        where products.Shop.UserId == userId
                        select products;
            if(subcategoryId != 0)
            {
                query = query.Where(i => i.SubCategoryId == subcategoryId);
            }

            var result = await query
                .Where(i => i.Status == 1)
                .Select(i => new ProductShopListModel()
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    CreatedDate = i.ProductAddedDate,
                    Status = i.Status,
                    Stock = (int)i.ProductStock,
                    SubCategoryName = i.SubCategory.SubCategoryName,
                    CategoryName = i.SubCategory.Category.CategoryName,
                    BrandName = i.Brand.BrandName,
                    ProductImages = i.ProductImages.Select(i => i.ProductImagePath).FirstOrDefault(),
                    Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.ProductId).ToList()
                })
                .ToListAsync();
            var list = result.OrderByDescending(i => i.ProductId).ToList();
            return list;
        }
        public async Task<Price> getProductPrice(int productId, int typeId)
        {
            var result = await _DbContext.ProductPrices
                .Where(i => i.ProductId == productId && i.ProductTypeId == typeId)
                .Select(i => new Price()
                {
                    price = i.Price,
                    priceOnSell = i.PriceOnSell
                }).FirstOrDefaultAsync();
            return result;
        }
        public async Task<ApiResponse> AddProduct(ProductSaveRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.name))
                    return new ApiFailResponse("Vui lòng nhập tên sản phẩm");
                if (request.shopId == 0)
                    return new ApiFailResponse("Vui lòng chọn cửa hàng");
                if (request.brandId == 0)
                    return new ApiFailResponse("Vui lòng chọn thương hiệu");
                if (request.subCategoryId == 0)
                    return new ApiFailResponse("Vui lòng chọn loại sản phẩm");

                Price priceAvailable = JsonConvert.DeserializeObject<Price>(request.priceAvailable);
                Price pricePreorder = JsonConvert.DeserializeObject<Price>(request.pricePreorder);
                if (priceAvailable.price == null && priceAvailable.priceOnSell == null && pricePreorder.price == null && pricePreorder.priceOnSell == null)
                    return new ApiFailResponse("Vui lòng nhập giá sản phẩm");
                if (priceAvailable.price == null && priceAvailable.priceOnSell != null)
                    return new ApiFailResponse("Vui lòng nhập giá gốc trước khi nhập giảm giá !");
                if (priceAvailable.price < priceAvailable.priceOnSell)
                    return new ApiFailResponse("Giá giảm không thể lớn hơn giá gốc !");
                if (pricePreorder.price == null && pricePreorder.priceOnSell != null)
                    return new ApiFailResponse("Vui lòng nhập giá gốc trước khi nhập giảm giá !");
                if (pricePreorder.price < pricePreorder.priceOnSell)
                    return new ApiFailResponse("Giá giảm không thể lớn hơn giá gốc !");

                var code = await _DbContext.Products
                    .Where(i => request.code != null && i.ProductCode == request.code.Trim())
                    .FirstOrDefaultAsync();
                if (code != null)
                    return new ApiFailResponse("Mã này đã tồn tại !");

                var isAdmin = await _userService.getUserRole(request.userId) == "Admin";

                /*
                 * None relationship data
                 */
                var product = new Data.Models.Product
                {
                    ProductCode = request.code.Trim(),
                    ProductName = request.name.Trim(), // required
                    Ppc = await GetNewPPC(),
                    ProductDescription = request.description == null ? null : request.description,
                    ProductDescriptionMobile = request.descriptionMobile == null ? null : request.descriptionMobile,
                    SizeGuide = request.size == null ? null : request.size,
                    Note = string.IsNullOrEmpty(request.note) ? "" : request.note.Trim(),
                    Link = string.IsNullOrEmpty(request.link) ? "" : request.link.Trim(),
                    DiscountPercent = request.discountPercent,
                    Legit = request.isLegit,
                    Highlights = request.isHighlight,
                    FreeDelivery = request.isFreeDelivery,
                    ProductStock = request.stock,
                    FreeReturn = request.isFreeReturn,
                    Insurance = string.IsNullOrEmpty(request.insurance) ? "" : request.insurance.Trim(),
                    New = request.isNew,
                    ShopId = request.shopId,
                    BrandId = request.brandId,
                    ProductAddedDate = DateTime.Now, // default
                    SubCategoryId = request.subCategoryId,
                    Status = isAdmin ? (byte?)enumProductStatus.Available : (byte?)enumProductStatus.Pending
                };
                await _DbContext.Products.AddAsync(product);
                await _DbContext.SaveChangesAsync();

                /*
                 * Relationship data
                 */
                // Options
                List<int> optionValueIds = JsonConvert.DeserializeObject<List<int>>(request.currentOptions);
                if (optionValueIds.Count > 0)
                {
                    foreach (var id in optionValueIds)
                    {
                        var productOptionValue = new ProductOptionValue
                        {
                            ProductId = product.ProductId,
                            OptionValueId = id
                        };
                        await _DbContext.ProductOptionValues.AddAsync(productOptionValue);
                    }
                    await _DbContext.SaveChangesAsync();
                }
                // New option value
                List<Dtos.Option> newOptions = JsonConvert.DeserializeObject<List<Dtos.Option>>(request.newOptions);
                if (newOptions.Count > 0)
                {
                    foreach (var option in newOptions)
                    {
                        var optionValues = await _DbContext.OptionValues
                            .Where(i => i.OptionId == option.id)
                            .Select(i => i.OptionValueName)
                            .ToListAsync();

                        // Get new values of list;
                        // Lấy các giá trị khác với db;
                        var values = option.values.Except(optionValues).ToList();
                        foreach (var value in values)
                        {
                            var newOptionValue = new OptionValue
                            {
                                OptionId = option.id,
                                OptionValueName = value.Trim(),
                                IsBaseValue = false
                            };
                            await _DbContext.OptionValues.AddAsync(newOptionValue);
                            await _DbContext.SaveChangesAsync();

                            await AddProductOptionValueByProductId(product.ProductId, newOptionValue.OptionValueId);
                        }

                        // Get already existed values in db of list;
                        // Lấy các giá trị trùng với db;
                        var currentValues = option.values.Intersect(optionValues).ToList();
                        foreach (var value in currentValues)
                        {
                            var optValue = await _DbContext.OptionValues
                                .Where(i => i.OptionValueName == value && i.OptionId == option.id)
                                .FirstOrDefaultAsync();

                            await AddProductOptionValueByProductId(product.ProductId, optValue.OptionValueId);
                        }
                    }
                }

                // Attributes
                List<Dtos.ProductAttribute> attributes = JsonConvert.DeserializeObject<List<Dtos.ProductAttribute>>(request.attributes);
                if (attributes.Count > 0)
                {
                    foreach (var attr in attributes)
                    {
                        if (!string.IsNullOrEmpty(attr.value))
                        {
                            var attribute = new Data.Models.ProductAttribute
                            {
                                ProductId = product.ProductId,
                                AttributeId = attr.id,
                                Value = attr.value.Trim()
                            };
                            await _DbContext.ProductAttributes.AddAsync(attribute);
                            await _DbContext.SaveChangesAsync();
                        }
                    }
                }

                // Prices
                decimal discountAvailable = 0;
                decimal discountPreOrder = 0;
                if (request.discountPercent != null && priceAvailable.price != null)
                    discountAvailable = GetDiscountPrice(priceAvailable.price, request.discountPercent);
                if (request.discountPercent != null && pricePreorder.price != null)
                    discountPreOrder = GetDiscountPrice(pricePreorder.price, request.discountPercent);
                if (request.discountPercent == null && priceAvailable.price != null && priceAvailable.priceOnSell != null)
                    discountAvailable = (decimal)priceAvailable.priceOnSell;
                if (request.discountPercent == null && pricePreorder.price != null && pricePreorder.priceOnSell != null)
                    discountPreOrder = (decimal)pricePreorder.priceOnSell;

                // Available Price
                var availablePrice = new ProductPrice();
                    availablePrice.Price = priceAvailable.price == null ? null : priceAvailable.price;
                    availablePrice.ProductId = product.ProductId;
                    availablePrice.ProductTypeId = (int)enumProductType.Available;
                    if (discountAvailable == 0) availablePrice.PriceOnSell = null;
                    else availablePrice.PriceOnSell = discountAvailable;
                await _DbContext.ProductPrices.AddAsync(availablePrice);
                // Pre Order Price
                var preOrderPrice = new ProductPrice();
                    preOrderPrice.Price = pricePreorder.price == null ? null : pricePreorder.price;
                    preOrderPrice.ProductId = product.ProductId;
                    preOrderPrice.ProductTypeId = (int)enumProductType.PreOrder;
                    if (discountPreOrder == 0) preOrderPrice.PriceOnSell = null;
                    else preOrderPrice.PriceOnSell = discountPreOrder;
                await _DbContext.ProductPrices.AddAsync(preOrderPrice);
                await _DbContext.SaveChangesAsync();

                // Images
                if (request.systemFileName != null) 
                {
                    foreach (var file in request.systemFileName)
                    {
                        var systemImage = new Data.Models.ProductImage
                        {
                            ProductId = product.ProductId,
                            ProductImagePath = file
                        };
                        await _DbContext.ProductImages.AddAsync(systemImage);
                        await _DbContext.SaveChangesAsync();
                    }
                }
                if (request.userFileName != null)
                {
                    foreach (var file in request.userFileName)
                    {
                        var userImage = new Data.Models.ProductUserImage
                        {
                            ProductId = product.ProductId,
                            ProductUserImagePath = file
                        };
                        await _DbContext.ProductUserImages.AddAsync(userImage);
                        await _DbContext.SaveChangesAsync();
                    }
                }
                                
                return new ApiSuccessResponse("Thêm thành công");
            }
            catch(Exception e)
            {
                return new ApiFailResponse(e.ToString());
            }
        }
        public async Task<Response<ProductUpdateResponse>> UpdateProduct(ProductSaveRequest request)
        {
            try
            {
                if (request.id == 0)
                    return new FailResponse<ProductUpdateResponse>("Vui lòng chọn sản phẩm cần cập nhật");
                if (string.IsNullOrEmpty(request.name))
                    return new FailResponse<ProductUpdateResponse>("Vui lòng nhập tên sản phẩm");
                if (request.shopId == 0)
                    return new FailResponse<ProductUpdateResponse>("Vui lòng chọn cửa hàng");
                if (request.brandId == 0)
                    return new FailResponse<ProductUpdateResponse>("Vui lòng chọn thương hiệu");
                if (request.subCategoryId == 0)
                    return new FailResponse<ProductUpdateResponse>("Vui lòng chọn loại sản phẩm");

                Price priceAvailable = JsonConvert.DeserializeObject<Price>(request.priceAvailable);
                Price pricePreorder = JsonConvert.DeserializeObject<Price>(request.pricePreorder);
                if (priceAvailable.price == null && priceAvailable.priceOnSell == null && pricePreorder.price == null && pricePreorder.priceOnSell == null)
                    return new FailResponse<ProductUpdateResponse>("Vui lòng nhập giá sản phẩm");
                if (priceAvailable.price == null && priceAvailable.priceOnSell != null)
                    return new FailResponse<ProductUpdateResponse>("Vui lòng nhập giá gốc trước khi nhập giảm giá !");
                if (priceAvailable.price <= priceAvailable.priceOnSell)
                    return new FailResponse<ProductUpdateResponse>("Giá giảm không thể bằng hoặc lớn hơn giá gốc !");
                if (pricePreorder.price == null && pricePreorder.priceOnSell != null)
                    return new FailResponse<ProductUpdateResponse>("Vui lòng nhập giá gốc trước khi nhập giảm giá !");
                if (pricePreorder.price <= pricePreorder.priceOnSell)
                    return new FailResponse<ProductUpdateResponse>("Giá giảm không thể bằng hoặc lớn hơn giá gốc !");

                var product = await _DbContext.Products
                    .Where(i => i.ProductId == request.id)
                    .FirstOrDefaultAsync();
                if (product == null)
                    return new FailResponse<ProductUpdateResponse>("Sản phẩm không tồn tại");

                var productImage = await _DbContext.ProductImages
                    .Where(i => i.ProductId == request.id)
                    .FirstOrDefaultAsync();
                if (productImage == null && request.systemImage == null)
                    return new FailResponse<ProductUpdateResponse>("Vui lòng chọn ảnh");

                var code = await _DbContext.Products
                    .Where(i => request.code != null && i.ProductCode == request.code.Trim() && i.ProductId != request.id)
                    .FirstOrDefaultAsync();
                if (code != null)
                    return new FailResponse<ProductUpdateResponse>("Mã này đã tồn tại !");



                var isAdmin = await _userService.getUserRole(request.userId) == "Admin";
                /*
                 * None relationship data
                 */
                product.ProductCode = request.code.Trim();
                product.ProductName = request.name.Trim(); // required
                product.ProductDescription = request.description == null ? null : request.description;
                product.ProductDescriptionMobile = request.descriptionMobile == null ? null : request.descriptionMobile;
                product.SizeGuide = request.size == null ? null : request.size;
                product.Note = request.note == null ? "" : request.note.Trim();
                product.Link = request.link == null ? "" : request.link.Trim();
                product.DiscountPercent = request.discountPercent;
                product.Legit = request.isLegit;
                product.Highlights = request.isHighlight;
                product.FreeDelivery = request.isFreeDelivery;
                product.ProductStock = request.stock;
                product.FreeReturn = request.isFreeReturn;
                product.Insurance = request.insurance == null ? "" : request.insurance.Trim();
                product.New = request.isNew;
                product.ShopId = request.shopId;
                product.BrandId = request.brandId;
                product.SubCategoryId = request.subCategoryId;
                product.Status = isAdmin ? (byte?)enumProductStatus.Available : (byte?)enumProductStatus.Pending;

                await _DbContext.SaveChangesAsync();

                /*
                 * Relationship data
                 */
                // Attribute
                List<Dtos.ProductAttribute> attributes = JsonConvert.DeserializeObject<List<Dtos.ProductAttribute>>(request.attributes);
                if (attributes.Count > 0)
                {
                    foreach (var attr in attributes)
                    {
                        var attribute = await _DbContext.ProductAttributes
                                .Where(i => i.ProductId == product.ProductId && i.AttributeId == attr.id)
                                .FirstOrDefaultAsync();
                        if (attribute == null) // Add new if not existed
                        {
                            var newAttribute = new Data.Models.ProductAttribute
                            {
                                ProductId = product.ProductId,
                                AttributeId = attr.id,
                                Value = attr.value.Trim()
                            };
                            await _DbContext.ProductAttributes.AddAsync(newAttribute);
                        }
                        else // Update if existed
                        {
                            attribute.Value = attr.value.Trim();
                        }
                        await _DbContext.SaveChangesAsync();
                    }      
                }

                // Options
                List<int> optionValueIds = JsonConvert.DeserializeObject<List<int>>(request.currentOptions);
                // Remove previous option value to add new
                var previousOptVals = await _DbContext.ProductOptionValues
                    .Where(i => i.ProductId == product.ProductId)
                    .ToListAsync();
                _DbContext.ProductOptionValues.RemoveRange(previousOptVals);
                _DbContext.SaveChangesAsync().Wait();
                if (optionValueIds.Count > 0)
                {
                    // Add new for each option value
                    foreach (var id in optionValueIds)
                    {
                        var productOptionValue = new ProductOptionValue
                        {
                            ProductId = product.ProductId,
                            OptionValueId = id
                        };
                        await _DbContext.ProductOptionValues.AddAsync(productOptionValue);
                    }
                }
                // New option value
                List<Dtos.Option> newOptions = JsonConvert.DeserializeObject<List<Dtos.Option>>(request.newOptions);
                if (newOptions.Count > 0)
                {
                    foreach (var option in newOptions)
                    {
                        var optionValues = await _DbContext.OptionValues
                            .Where(i => i.OptionId == option.id)
                            .Select(i => i.OptionValueName)
                            .ToListAsync();

                        // Get new values of list;
                        // Lấy các giá trị khác với db;
                        var values = option.values.Except(optionValues).ToList();
                        foreach (var value in values)
                        {
                            var newOptionValue = new OptionValue
                            {
                                OptionId = option.id,
                                OptionValueName = value.Trim(),
                                IsBaseValue = false
                            };
                            await _DbContext.OptionValues.AddAsync(newOptionValue);
                            await _DbContext.SaveChangesAsync();

                            await AddProductOptionValueByProductId(product.ProductId, newOptionValue.OptionValueId);
                        }

                        // Get already existed values in db of list;
                        // Lấy các giá trị trùng với db;
                        var currentValues = option.values.Intersect(optionValues).ToList();
                        foreach (var value in currentValues)
                        {
                            var optValue = await _DbContext.OptionValues
                                .Where(i => i.OptionValueName == value && i.OptionId == option.id)
                                .FirstOrDefaultAsync();

                            await AddProductOptionValueByProductId(product.ProductId, optValue.OptionValueId);
                        }
                    }
                }

                // Prices
                // Discount
                decimal discountAvailable = 0;
                decimal discountPreOrder = 0;
                if (request.discountPercent != null && priceAvailable.price != null)
                    discountAvailable = GetDiscountPrice(priceAvailable.price, request.discountPercent);
                if (request.discountPercent != null && pricePreorder.price != null)
                    discountPreOrder = GetDiscountPrice(pricePreorder.price, request.discountPercent);
                if (request.discountPercent == null && priceAvailable.price != null && priceAvailable.priceOnSell != null)
                    discountAvailable = (decimal)priceAvailable.priceOnSell;
                if (request.discountPercent == null && pricePreorder.price != null && pricePreorder.priceOnSell != null)
                    discountPreOrder = (decimal)pricePreorder.priceOnSell;
                // Available price
                var availablePrice = await _DbContext.ProductPrices
                    .Where(i => i.ProductId == product.ProductId && i.ProductTypeId == (int)enumProductType.Available)
                    .FirstOrDefaultAsync();
                if (availablePrice != null)
                {
                    availablePrice.Price = priceAvailable.price == null ? null : priceAvailable.price;
                    if (discountAvailable == 0) availablePrice.PriceOnSell = null;
                    else availablePrice.PriceOnSell = discountAvailable;
                }
                else
                {
                    var newAvailablePrice = new ProductPrice();
                    newAvailablePrice.ProductId = product.ProductId;
                    newAvailablePrice.Price = priceAvailable.price;
                    newAvailablePrice.ProductTypeId = (int)enumProductType.Available;
                    if (discountAvailable == 0) newAvailablePrice.PriceOnSell = null;
                    else newAvailablePrice.PriceOnSell = discountAvailable;

                    await _DbContext.ProductPrices.AddAsync(newAvailablePrice);
                }
                await _DbContext.SaveChangesAsync();
                // PreOrder Price
                var preOrderPrice = await _DbContext.ProductPrices
                        .Where(i => i.ProductId == product.ProductId && i.ProductTypeId == (int)enumProductType.PreOrder)
                        .FirstOrDefaultAsync();
                if (preOrderPrice != null)
                {
                    preOrderPrice.Price = pricePreorder.price == null ? null : pricePreorder.price;
                    if (discountPreOrder == 0) preOrderPrice.PriceOnSell = null;
                    else preOrderPrice.PriceOnSell = discountPreOrder;
                }
                else
                {
                    var newPreOrderPrice = new ProductPrice();
                    newPreOrderPrice.ProductId = product.ProductId;
                    newPreOrderPrice.Price = pricePreorder.price;
                    newPreOrderPrice.ProductTypeId = (int)enumProductType.PreOrder;
                    if (discountPreOrder == 0) newPreOrderPrice.PriceOnSell = null;
                    else newPreOrderPrice.PriceOnSell = discountPreOrder;

                    await _DbContext.ProductPrices.AddAsync(newPreOrderPrice);
                }
                await _DbContext.SaveChangesAsync();

                // Images
                if (request.systemFileName != null)
                {
                    foreach (var file in request.systemFileName)
                    {
                        var systemImage = new Data.Models.ProductImage
                        {
                            ProductId = product.ProductId,
                            ProductImagePath = file
                        };
                        await _DbContext.ProductImages.AddAsync(systemImage);
                        await _DbContext.SaveChangesAsync();
                    }
                }
                if (request.userFileName != null)
                {
                    foreach (var file in request.userFileName)
                    {
                        var userImage = new Data.Models.ProductUserImage
                        {
                            ProductId = product.ProductId,
                            ProductUserImagePath = file
                        };
                        await _DbContext.ProductUserImages.AddAsync(userImage);
                        await _DbContext.SaveChangesAsync();
                    }
                }

                return new SuccessResponse<ProductUpdateResponse>("Cập nhật thành công");
            }
            catch(Exception e)
            {
                return new FailResponse<ProductUpdateResponse>(e.Message);
            }
        }
        public async Task<Response<string>> DeleteProductImage(int id)
        {
            try
            {
                var image = await _DbContext.ProductImages
                    .Where(i => i.ProductImageId == id)
                    .FirstOrDefaultAsync();
                if (image == null) 
                    return new FailResponse<string>("Không tìm thấy ảnh");

                _DbContext.ProductImages.Remove(image);
                _DbContext.SaveChangesAsync().Wait();
                return new SuccessResponse<string>("Xóa thành công", image.ProductImagePath);
            }
            catch
            {
                return new FailResponse<string>("Xóa thất bại, vui lòng thử lại sau");
            }
        }
        public async Task<Response<string>> DeleteProductUserImage(int id)
        {
            try
            {
                var image = await _DbContext.ProductUserImages
                    .Where(i => i.ProductUserImageId == id)
                    .FirstOrDefaultAsync();
                if (image == null) return new FailResponse<string>("Không tìm thấy ảnh");

                _DbContext.ProductUserImages.Remove(image);
                _DbContext.SaveChangesAsync().Wait();
                return new SuccessResponse<string>("Xóa thành công", image.ProductUserImagePath);
            }
            catch
            {
                return new FailResponse<string>("Xóa thất bại, vui lòng thử lại sau");
            }
        }
        private async Task AddProductOptionValueByProductId(int productId, int optValId)
        {
            var productOptionValue = new ProductOptionValue
            {
                ProductId = productId,
                OptionValueId = optValId
            };

            await _DbContext.ProductOptionValues.AddAsync(productOptionValue);
            await _DbContext.SaveChangesAsync();
        }
        public async Task<Response<ProductDeleteResponse>> DeleteProduct(int id)
        {
            try
            {
                // Check null product
                var product = await _DbContext.Products.Where(i => i.ProductId == id).FirstOrDefaultAsync();
                if (product == null) return new SuccessResponse<ProductDeleteResponse>("Sản phẩm không tồn tại");
                
                // System's iamges
                var sysImages = await _DbContext.ProductImages.Where(i => i.ProductId == id).ToListAsync();
                if (sysImages.Count > 0) _DbContext.ProductImages.RemoveRange(sysImages);
                // User's images
                var userImages = await _DbContext.ProductUserImages.Where(i => i.ProductId == id).ToListAsync();
                if (userImages.Count > 0) _DbContext.ProductUserImages.RemoveRange(userImages);
                // Prices
                var productPrices = await _DbContext.ProductPrices.Where(i => i.ProductId == id).ToListAsync();
                if (productPrices.Count > 0) _DbContext.ProductPrices.RemoveRange(productPrices);
                // Attributes
                var attrs = await _DbContext.ProductAttributes.Where(i => i.ProductId == id).ToListAsync();
                if (attrs.Count > 0) _DbContext.ProductAttributes.RemoveRange(attrs);
                // Options
                var opts = await _DbContext.ProductOptionValues.Where(i => i.ProductId == id).ToListAsync();
                if (opts.Count > 0) _DbContext.ProductOptionValues.RemoveRange(opts);
                // Product's comments
                await _rateService.deleteCommentByProductId(id);
                
                // Remove product and save changes
                _DbContext.Products.Remove(product);
                _DbContext.SaveChangesAsync().Wait();
                var data = new ProductDeleteResponse
                {
                    systemImages = sysImages.Select(i => i.ProductImagePath).ToList(),
                    userImages = userImages.Select(i => i.ProductUserImagePath).ToList()
                };
                return new SuccessResponse<ProductDeleteResponse>("Xóa thành công", data);
            }
            catch (Exception error)
            {
                return new FailResponse<ProductDeleteResponse>("Xóa thất bại " + error.Message);
            }
        }
        public async Task<Response<ProductDeleteResponse>> DeleteProducts(List<int> ids)
        {
            try
            {
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
                foreach (var id in ids)
                {
                    await _rateService.deleteCommentByProductId(id);
                }

                // Remove product and save changes
                _DbContext.Products.RemoveRange(products);
                _DbContext.SaveChangesAsync().Wait();

                var data = new ProductDeleteResponse
                {
                    systemImages = sysImages.Select(i => i.ProductImagePath).ToList(),
                    userImages = userImages.Select(i => i.ProductUserImagePath).ToList()
                };
                return new SuccessResponse<ProductDeleteResponse>("Xóa thành công", data);
            }
            catch (Exception error)
            {
                return new FailResponse<ProductDeleteResponse>("Xóa thất bại, vui lòng thử lại sau\n" + error.Message);
            }
        }
        public async Task<ApiResponse> DisableProducts(List<int> ids)
        {
            try
            {
                if (ids.Count == 0) return new ApiFailResponse("Vui lòng chọn sản phẩm cần cập nhật");
                foreach (var id in ids)
                {
                    var product = await _DbContext.Products.Where(i => i.ProductId == id).FirstOrDefaultAsync();
                    product.Status = (byte?)enumProductStatus.Disabled;
                }
                
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Cập nhật thất bại " + error);
            }
        }
        public async Task<ApiResponse> ApproveProducts(List<int> ids)
        {
            try
            {
                if (ids.Count == 0) return new ApiFailResponse("Vui lòng chọn sản phẩm cần cập nhật");
                foreach (var id in ids)
                {
                    var product = await _DbContext.Products.Where(i => i.ProductId == id).FirstOrDefaultAsync();
                    product.Status = (byte?)enumProductStatus.Available;
                }
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Cập nhật thất bại " + error);
            }
        }
        public async Task<ApiResponse> AddSizeGuide(SizeGuideAddRequest request)
        {
            try
            {
                if (request.ids == null || request.ids.Count == 0) return new ApiFailResponse("Chọn loại sản phẩm");
                if (string.IsNullOrEmpty(request.content)) return new ApiFailResponse("Chọn nội dung");

                var newSizeGuide = new SizeGuide
                {
                    SizeContent = request.content,
                    IsBaseSizeGuide = true
                };
                await _DbContext.SizeGuides.AddAsync(newSizeGuide);
                await _DbContext.SaveChangesAsync();

                var subs = await _DbContext.SubCategories
                    .Where(i => request.ids.Contains(i.SubCategoryId))
                    .ToListAsync();
                if (subs.Count > 0)
                {
                    foreach (var sub in subs)
                    {
                        sub.SizeGuideId = newSizeGuide.SizeGuideId;
                        await _DbContext.SaveChangesAsync();
                    }
                }

                return new ApiSuccessResponse("Thêm thành công");
            }
            catch
            {
                return new ApiFailResponse("Thêm thất bại, vui lòng thử lại sau");
            }
        }
        public async Task<List<SizeGuideModel>> SizeGuideList()
        {
            try
            {
                var list = await _DbContext.SizeGuides
                    .Select(i => new SizeGuideModel
                    {
                        id = i.SizeGuideId,
                        content = i.SizeContent,
                        subCategories = i.SubCategories
                        .Where(sc => sc.SizeGuideId == i.SizeGuideId)
                        .Select(sc => new SubCategoryModel {
                            SubCategoryId = sc.SubCategoryId,
                            SubCategoryName = sc.SubCategoryName,
                            SizeGuide = sc.SizeGuide
                        })
                        .ToList()
                    })
                    .ToListAsync();

                return list;
            }
            catch
            {
                return null;
            }
        }
        public async Task<Response<SizeGuide>> SizeGuideDetail(int id)
        {
            try
            {
                if (id == 0) return new FailResponse<SizeGuide>("Vui lòng chọn loại sản phẩm");

                var sizeGuide = await _DbContext.SizeGuides
                    .Where(i => i.SizeGuideId == id)
                    .Select(i => new SizeGuide {
                        SizeGuideId = i.SizeGuideId,
                        SizeContent = i.SizeContent,
                        IsBaseSizeGuide = i.IsBaseSizeGuide
                    })
                    .FirstOrDefaultAsync();
                if (sizeGuide == null) return new FailResponse<SizeGuide>("Không tìm thấy");

                return new SuccessResponse<SizeGuide>("Lấy thông tin thành công", sizeGuide);
            }
            catch
            {
                return new FailResponse<SizeGuide>("Lấy thông tin thất bại, thử lại sau");
            }
        }
        public async Task<ApiResponse> DeleteSizeGuide(int id)
        {
            try
            {
                if (id == 0)
                    return new ApiFailResponse("Vui lòng chọn bảng");

                var subCategories = await _DbContext.SubCategories
                    .Where(i => i.SizeGuideId == id)
                    .ToListAsync();
                if (subCategories != null)
                {
                    foreach (var subc in subCategories)
                    {
                        subc.SizeGuideId = null;
                    }
                    _DbContext.SubCategories.UpdateRange(subCategories);
                }

                var sizeGuide = await _DbContext.SizeGuides
                    .Where(i => i.SizeGuideId == id)
                    .FirstOrDefaultAsync();
                if (sizeGuide != null)
                {
                    _DbContext.SizeGuides.Remove(sizeGuide);
                }

                _DbContext.SaveChangesAsync().Wait();

                return new ApiSuccessResponse("Xóa thành công");
            }
            catch(Exception error)
            {
                return new ApiFailResponse(error.ToString());
            }
        }
        public async Task<Response<SizeGuide>> GetSizeGuideBySub(int id)
        {
            var size = await _DbContext.SubCategories
                .Where(i => i.SubCategoryId == id)
                .Select(i => new SizeGuide
                {
                    SizeGuideId = i.SizeGuide.SizeGuideId,
                    SizeContent = i.SizeGuide.SizeContent,
                    IsBaseSizeGuide = i.SizeGuide.IsBaseSizeGuide
                })
                .FirstOrDefaultAsync();
            return new SuccessResponse<SizeGuide>("Lấy thông tin thành công", size);

        }
        public async Task<ApiResponse> UpdateSizeGuide(SizeGuideAddRequest request)
        {
            try
            {
                if (request.id == 0) 
                    return new ApiFailResponse("Vui lòng chọn bảng chọn size"); 
                if (request.ids == null || request.ids.Count == 0) 
                    return new ApiFailResponse("Chọn loại sản phẩm");
                if (string.IsNullOrEmpty(request.content)) 
                    return new ApiFailResponse("Chọn nội dung");

                // Update size content
                var sizeGuide = await _DbContext.SizeGuides
                    .Where(i => i.SizeGuideId == request.id)
                    .FirstOrDefaultAsync();
                if (sizeGuide != null)
                    sizeGuide.SizeContent = request.content;
                await _DbContext.SaveChangesAsync();

                // Remove previous size id in subcategories
                var subs = await _DbContext.SubCategories
                    .Where(i => i.SizeGuideId == request.id)
                    .ToListAsync();
                foreach (var sub in subs)
                {
                    sub.SizeGuideId = null;
                    await _DbContext.SaveChangesAsync();
                }

                // Update size id for Subcategory
                var _subs = await _DbContext.SubCategories
                    .Where(i => request.ids.Contains(i.SubCategoryId))
                    .ToListAsync();
                if (_subs.Count > 0)
                {
                    foreach (var sub in _subs)
                    {
                        sub.SizeGuideId = sizeGuide.SizeGuideId;
                    }
                    await _DbContext.SaveChangesAsync();
                }

                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Thêm thất bại, vui lòng thử lại sau");
            }
        }
        private decimal GetDiscountPrice(decimal? price, byte? percent)
        {
            var priceDiscounted = price - (price * percent / 100);
            return (decimal)priceDiscounted;
        }
        private async Task<string> GetNewPPC()
        {
            var newestId = await _DbContext.Products
                .OrderByDescending(s => s.ProductId)
                .Select(i => i.ProductId)
                .FirstOrDefaultAsync();
            
            byte[] buffer = Guid.NewGuid().ToByteArray();
            string guid = BitConverter.ToUInt32(buffer, 8).ToString();

            int len = (newestId + 1).ToString().Length;
            var ppc = guid.Substring(0, 8 - len) + (newestId + 1).ToString();

            return ppc;
        }
    }
}
