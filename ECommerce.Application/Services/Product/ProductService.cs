using ECommerce.Application.Common;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.Product.Enum;
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
                    Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.ProductId).ToList(),
                    ShopName = i.Shop.ShopName,
                    ShopId = i.ShopId,
                    BrandId = i.BrandId,
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
                                                ProductName = i.ProductName,
                                                ProductDescription = i.ProductDescription,
                                                Note = i.Note,
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
        public async Task<ApiResponse> AddProduct(ProductAddRequest request)
        {
            if (string.IsNullOrEmpty(request.name)) 
                return new ApiFailResponse("Vui lòng nhập tên sản phẩm");
            if (request.shopId == 0) 
                return new ApiFailResponse("Vui lòng chọn cửa hàng");
            if (request.brandId == 0) 
                return new ApiFailResponse("Vui lòng chọn thương hiệu");
            if (request.subCategoryId == 0)
                return new ApiFailResponse("Vui lòng chọn danh mục sản phẩm");

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

            try
            {
                var isAdmin = await _userService.getUserRole(request.userId) == "Admin";            
                
                /*
                 * None relationship data
                 */
                var product = new Data.Models.Product
                {
                    ProductName = request.name.Trim(), // required
                    ProductDescription = request.description == null ? null : request.description,
                    Note = request.note == null ? null : request.note.Trim(),
                    DiscountPercent = request.discountPercent,
                    Legit = request.isLegit,
                    Highlights = request.isHighlight,
                    FreeDelivery = request.isFreeDelivery,
                    ProductStock = request.stock,
                    FreeReturn = request.isFreeReturn,
                    Insurance = request.insurance == null ? null : request.insurance.Trim(),
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
                }

                // New option value
                //List<Dtos.Option> newOptions = JsonConvert.DeserializeObject<List<Dtos.Option>>(request.newOptions);
                //if (newOptions.Count > 0)
                //{
                //    foreach (var option in newOptions)
                //    {
                //        var optionValues = await _DbContext.OptionValues
                //            .Where(i => i.OptionId == option.id)
                //            .Select(i => i.OptionValueName)
                //            .ToListAsync();

                //        // Get new values of list;
                //        // Lấy các giá trị khác với db;
                //        var values = option.values.Except(optionValues).ToList();
                //        foreach (var value in values)
                //        {
                //            var optionValue = new OptionValue
                //            {
                //                OptionId = option.id,
                //                OptionValueName = value,
                //                IsBaseValue = false
                //            };
                //            await _DbContext.OptionValues.AddAsync(optionValue); // Add new value
                //            await _DbContext.SaveChangesAsync();

                //            await AddOptionValueByProductId(product.ProductId, value);
                //        }

                //        // Get already existed values in db of list;
                //        // Lấy các giá trị trùng với db;
                //        var currentValues = option.values.Intersect(optionValues).ToList();
                //        foreach (var value in currentValues)
                //        {
                //            await AddOptionValueByProductId(product.ProductId, value);
                //        }
                //    }
                //}
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
                                OptionValueName = value,
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
                                Value = attr.value
                            };
                            await _DbContext.ProductAttributes.AddAsync(attribute);
                            await _DbContext.SaveChangesAsync();
                        }
                    }
                }

                // Prices
                decimal discountAvailable = 0;
                decimal discountPreOrder = 0;
                if (request.discountPercent != null)
                {
                    discountAvailable = GetDiscountPrice(priceAvailable.price, request.discountPercent);
                    discountPreOrder = GetDiscountPrice(pricePreorder.price, request.discountPercent);
                }
                if (priceAvailable.price != null)
                {
                    var availablePrice = new ProductPrice
                    {
                        ProductId = product.ProductId,
                        Price = priceAvailable.price,
                        PriceOnSell = discountAvailable == 0 ? null : discountAvailable,
                        ProductTypeId = (int)enumProductType.Available,
                    };
                    await _DbContext.ProductPrices.AddAsync(availablePrice);
                }
                if (pricePreorder.price != null)
                {
                    var preOrderPrice = new ProductPrice
                    {
                        ProductId = product.ProductId,
                        Price = pricePreorder.price,
                        PriceOnSell = discountPreOrder == 0 ? null : discountPreOrder,
                        ProductTypeId = (int)enumProductType.PreOrder,
                    };
                    await _DbContext.ProductPrices.AddAsync(preOrderPrice);
                }               
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
                return new ApiFailResponse(e.Message);
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
                    Content = request.content,
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
        public async Task<Response<SizeGuide>> SizeGuideDetail(int id)
        {
            try
            {
                if (id == 0) return new FailResponse<SizeGuide>("Vui lòng chọn loại sản phẩm");

                var sizeGuide = await _DbContext.SubCategories
                    .Where(i => i.SubCategoryId == id)
                    .Select(i => new SizeGuide {
                        SizeGuideId = i.SizeGuide.SizeGuideId,
                        Content = i.SizeGuide.Content,
                        IsBaseSizeGuide = i.SizeGuide.IsBaseSizeGuide
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
        public async Task<ApiResponse> UpdateSizeGuide(SizeGuideAddRequest request)
        {
            try
            {
                if (request.id == 0) return new ApiFailResponse("Vui lòng chọn bảng chọn size"); 
                if (request.ids == null || request.ids.Count == 0) return new ApiFailResponse("Chọn loại sản phẩm");
                if (string.IsNullOrEmpty(request.content)) return new ApiFailResponse("Chọn nội dung");

                var sizeGuide = await _DbContext.SizeGuides
                    .Where(i => i.SizeGuideId == request.id)
                    .FirstOrDefaultAsync();
                if (sizeGuide != null)
                {
                    sizeGuide.Content = request.content;
                }
                await _DbContext.SaveChangesAsync();


                var subs = await _DbContext.SubCategories
                    .Where(i => request.ids.Contains(i.SubCategoryId))
                    .ToListAsync();
                if (subs.Count > 0)
                {
                    foreach (var sub in subs)
                    {
                        sub.SizeGuideId = sizeGuide.SizeGuideId;
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
        private decimal GetDiscountPrice(decimal? price, byte? percent)
        {
            var priceDiscounted = price - (price * percent / 100);
            return (decimal)priceDiscounted;
        }
    }
}
