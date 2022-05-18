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

        public async Task<int> getProductOption(int id)
        {
            var opt = await _DbContext.Options.ToListAsync();
            var optValue = await _DbContext.OptionValues.ToListAsync();

            var pro_optValue = await _DbContext.ProductOptionValues.Where(i => i.ProductId == id).ToListAsync();
            pro_optValue = pro_optValue.Distinct().ToList();

            return 0;
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
                         orderby product.SubCategoryId
                         select new { product, brand, shop }).AsQueryable();

            var queryAble = query;
            // Query get products by brandid
            if (BrandId > 0 && SubCategoryId == 0)
            {
                queryAble = query.Where(q => q.product.BrandId == BrandId);
                if (orderBy == "Newest") queryAble = query.Where(q => q.product.New == true);
                if (orderBy == "Discount") queryAble = query.Where(q => q.product.DiscountPercent > 0);
            }
            // Query get products by brandid and subcategoryid
            if (BrandId > 0 && SubCategoryId > 0)
            {
                queryAble = query.Where(q => q.product.BrandId == BrandId && q.product.SubCategoryId == SubCategoryId);
                if (orderBy == "Newest") queryAble = query.Where(q => q.product.New == true);
                if (orderBy == "Discount") queryAble = query.Where(q => q.product.DiscountPercent > 0);
            }
            // Query get products by optionvalue
            if (BrandId > 0 && listOptionValueId != null)
            { 
                var listProductId = await _DbContext.ProductOptionValues
                                            .Where(i => listOptionValueId.Any(l => l == i.OptionValueId))
                                            .Select(i => i.ProductId)
                                            .Distinct()
                                            .ToListAsync();
                queryAble = query.Where(q => listProductId.Any(l => l == q.product.ProductId));
                if (orderBy == "Newest") queryAble = query.Where(q => q.product.New == true);
                if (orderBy == "Discount") queryAble = query.Where(q => q.product.DiscountPercent > 0);
            }

            // Select from query
            var list = queryAble
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
    }
}
