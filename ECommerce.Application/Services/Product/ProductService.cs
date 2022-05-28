using ECommerce.Application.Common;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;
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
        public ProductService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
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
                var listProductId = await _DbContext.ProductOptionValues
                                            .Where(i => listOptionValueId.Any(l => l == i.OptionValueId))
                                            .Select(i => i.ProductId)
                                            .Distinct()
                                            .ToListAsync();
                query = query.Where(q => listProductId.Any(l => l == q.product.ProductId));
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
    }
}
