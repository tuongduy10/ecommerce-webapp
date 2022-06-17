using ECommerce.Application.Common;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.Product.Enum;
using ECommerce.Application.Services.User;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product
{
    public class ProductService : IProductService
    {
        private ECommerceContext _DbContext;
        private IUserService _userService;
        public ProductService(ECommerceContext DbContext, IUserService userService)
        {
            _DbContext = DbContext;
            _userService = userService;
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
                                        Name = i.OptionName,
                                        Value =_DbContext.ProductOptionValues
                                                .Where(pov => pov.ProductId == productId && pov.OptionValue.OptionId == i.OptionId)
                                                .Select(pov => pov.OptionValue.OptionValueName)
                                                .ToList()
                                    })
                                    .FirstOrDefaultAsync();
                result.Add(opt_v);
            }

            return result;
        }
        public async Task<ProductDetailModel> getProductDeatil(int id)
        {
            var attr = await _DbContext.ProductAttributes
                                            .Where(i => i.ProductId == id)
                                            .Select(i => new Dtos.Attribute() { 
                                                Value = i.Value,
                                                AttrName = i.Attribute.AttributeName
                                            })
                                            .ToListAsync();

            var rate = await _DbContext.Rates.Where(i => i.ProductId == id).ToListAsync();

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
                                                ProductName = i.ProductName,
                                                ProductDescription = i.ProductDescription,

                                                FreeDelivery = i.FreeDelivery,
                                                FreeReturn = i.FreeReturn,
                                                Legit = i.Legit,
                                                Insurance = i.Insurance,

                                                BrandId = i.Brand.BrandId,
                                                BrandName = i.Brand.BrandName,
                                                ShopName = i.Shop.ShopName,
                                                ShopId = i.Shop.ShopId,
                                                ProductRate = productRate,

                                                Attributes = attr,
                                                ProductImportDate = i.ProductImportDate,
                                                DiscountPercent = i.DiscountPercent,
                                                Prices = _DbContext.ProductPrices.Where(price => price.ProductId == id).ToList(),
                                                Types = _DbContext.ProductTypes
                                                                    .Select(type => new Dtos.Type() { 
                                                                        ProductTypeName = type.ProductTypeName,
                                                                        ProductTypeId = type.ProductTypeId
                                                                    }).ToList(),

                                                ProductImages = _DbContext.ProductImages.Where(img => img.ProductId == id).Select(img => img.ProductImagePath).ToList(),
                                                ProductUserImages = _DbContext.ProductUserImages.Where(img => img.ProductId == id).Select(img => img.ProductUserImagePath).ToList()
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
                             where product.BrandId == BrandId
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
                        .Select(i => i.ProductId)
                        .ToListAsync();
                    optProIds = optProIds.Distinct().ToList(); // 

                    var listProIdsQuery = new List<int>();
                    foreach(var id in optProIds)
                    {
                        var optValIds = await _DbContext.ProductOptionValues
                            .Where(i => i.ProductId == id)
                            .Select(i => i.OptionValueId)
                            .ToListAsync();
                        var hasProduct = listOptionValueId.All(i => optValIds.Any(j => j == i));
                        if (hasProduct) listProIdsQuery.Add(id);
                    }
                    listProIdsQuery = listProIdsQuery.Distinct().ToList();

                    query = query.Where(q => listProIdsQuery.Any(l => l == q.product.ProductId));
                }

                // Order by... request
                if (orderBy == "Newest") query = query.Where(q => q.product.New == true);
                if (orderBy == "Discount") query = query.Where(q => q.product.DiscountPercent > 0);

                // Select from query
                var list = query
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
                .Where(i => i.product.Highlights == true)
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
                .Select(i => new ProductShopListModel()
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Status = i.Status,
                    Stock = (int)i.ProductStock,
                    SubCategoryName = i.SubCategory.SubCategoryName,
                    CategoryName = i.SubCategory.Category.CategoryName,
                    BrandName = i.Brand.BrandName,
                    ProductImages = i.ProductImages.Select(i => i.ProductImagePath).FirstOrDefault(),
                    Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.ProductId).ToList()
                })
                .ToListAsync();

            return result;
        }
        public async Task<Price> getProductPirce(int productId, int typeId)
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
        public async Task<ApiResponse> AddProduct(ProductAddRequest request)
        {
            if(string.IsNullOrEmpty(request.name)) return new ApiFailResponse("Vui lòng nhập tên sản phẩm");
            if (request.priceAvailable == null && request.pricePreorder == null)
            {
                return new ApiFailResponse("Vui lòng nhập giá sản phẩm");
            }

            try
            {
                var userRole = await _userService.getUserRole(request.userId);
                var shopId = await _DbContext.Shops.Where(i => i.UserId == request.userId).Select(i => i.ShopId).FirstOrDefaultAsync();
                /*
                 * None relationship data
                 */
                var product = new Data.Models.Product
                {
                    ProductName = request.name,
                    ProductDescription = request.description,
                    Note = request.note,
                    DiscountPercent = request.discountPercent,
                    Legit = request.isLegit,
                    FreeDelivery = request.isFreeDelivery,
                    ProductStock = request.stock,
                    FreeReturn = request.isFreeReturn,
                    Insurance = request.insurance,
                    New = request.isNew,
                    ShopId = shopId,
                    BrandId = request.brandId,
                    ProductAddedDate = DateTime.Now,
                    SubCategoryId = request.subCategoryId,
                    Status = userRole == "Admin" ? (byte?)enumProductStatus.Available : (byte?)enumProductStatus.Pending
                };
                await _DbContext.Products.AddAsync(product);
                await _DbContext.SaveChangesAsync();

                /*
                 * Relationship data
                 */
                // Options
                List<int> options = JsonConvert.DeserializeObject<List<int>>(request.currentOptions);
                if (options.Count > 0)
                {
                    foreach (var id in options)
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
                List<NewOption> newOptions = JsonConvert.DeserializeObject<List<NewOption>>(request.newOptions);
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
                            var optionValue = new OptionValue
                            {
                                OptionId = option.id,
                                OptionValueName = value,
                            };
                            await _DbContext.OptionValues.AddAsync(optionValue); // Add new value
                            await _DbContext.SaveChangesAsync();

                            await AddOptionValueByProductId(product.ProductId, value);
                        }

                        // Get already existed values in db of list;
                        // Lấy các giá trị trùng với db;
                        var currentValues = option.values.Intersect(optionValues).ToList();
                        foreach (var value in currentValues)
                        {
                            await AddOptionValueByProductId(product.ProductId, value);
                        }
                    }
                }
                
                // Attributes
                List<Dtos.ProductAttribute> attributes = JsonConvert.DeserializeObject<List<Dtos.ProductAttribute>>(request.attributes);
                if (attributes.Count > 0)
                {
                    foreach (var attr in attributes)
                    {
                        var attribute = new Data.Models.ProductAttribute
                        {
                            ProductId = product.ProductId,
                            AttributeId = attr.id,
                            Value = attr.value
                        };
                        await _DbContext.ProductAttributes.AddAsync(attribute);
                        await _DbContext.SaveChangesAsync();
                    }
                }
                
                // Prices
                Price priceAvailable = JsonConvert.DeserializeObject<Price>(request.priceAvailable);
                if (priceAvailable.price == null && priceAvailable.priceOnSell != null) return new ApiFailResponse("Vui lòng nhập giá gốc trước khi nhập giảm giá !");
                if (priceAvailable.price < priceAvailable.priceOnSell) return new ApiFailResponse("Giá giảm không thể lớn hơn giá gốc !");
                var availablePrice = new ProductPrice
                {
                    ProductId = product.ProductId,
                    Price = priceAvailable.price,
                    PriceOnSell = priceAvailable.priceOnSell != null ? priceAvailable.priceOnSell : GetDiscountPrice(priceAvailable.price, request.discountPercent),
                    ProductTypeId = (int)enumProductType.Available,
                };

                Price pricePreorder = JsonConvert.DeserializeObject<Price>(request.pricePreorder);
                if (pricePreorder.price == null && pricePreorder.priceOnSell != null) return new ApiFailResponse("Vui lòng nhập giá gốc trước khi nhập giảm giá !");
                if (pricePreorder.price < pricePreorder.priceOnSell) return new ApiFailResponse("Giá giảm không thể lớn hơn giá gốc !");
                var preOrderPrice = new ProductPrice
                {
                    ProductId = product.ProductId,
                    Price = pricePreorder.price,
                    PriceOnSell = pricePreorder.priceOnSell != null ? pricePreorder.priceOnSell : GetDiscountPrice(pricePreorder.price, request.discountPercent),
                    ProductTypeId = (int)enumProductType.PreOrder,
                };
                await _DbContext.ProductPrices.AddAsync(availablePrice);
                await _DbContext.ProductPrices.AddAsync(preOrderPrice);
                await _DbContext.SaveChangesAsync();

                // Images
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
                
                return new ApiSuccessResponse("Thêm thành công");
            }
            catch(Exception e)
            {
                return new ApiFailResponse(e.Message);
            }
        }
        private async Task AddOptionValueByProductId(int productId, string value)
        {
            var id = await _DbContext.OptionValues
                            .Where(i => i.OptionValueName == value)
                            .Select(i => i.OptionValueId)
                            .FirstOrDefaultAsync();

            var productOptionValue = new ProductOptionValue
            {
                ProductId = productId,
                OptionValueId = id
            };

            await _DbContext.ProductOptionValues.AddAsync(productOptionValue);
            await _DbContext.SaveChangesAsync();
        }
        private decimal GetDiscountPrice(decimal? price, byte? percent)
        {
            var priceDiscounted = price - (price * percent / 100);
            return (decimal)priceDiscounted;
        }
    }
}
