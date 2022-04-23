using ECommerce.Application.Dtos;
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
        public async Task<PageResult<ProductInBrandModel>> getProductPaginated(int BrandId, int pageindex, int pagesize)
        {
            // Query get products with brand, shop, subcategoryid
            var query = from product in _DbContext.Products
                        join brand in _DbContext.Brands on product.BrandId equals brand.BrandId
                        join shop in _DbContext.Shops on product.ShopId equals shop.ShopId
                        where brand.BrandId == BrandId
                        orderby product.SubCategoryId
                        select new
                        {
                            id = product.ProductId,
                            name = product.ProductName,
                            discount = product.DiscountPercent,
                            status = product.Status,
                            highlights = product.Highlights,
                            newPro = product.New,
                            importDate = product.ProductImportDate,
                            subcategory = product.SubCategoryId,

                            brandName = brand.BrandName,
                            shopName = shop.ShopName,
                        };
            // Excute query to list
            var list = query.AsQueryable().Select(i => new ProductInBrandModel()
            {
                ProductId = i.id,
                ProductName = i.name,
                DiscountPercent = i.discount,
                Status = i.status,
                Highlights = i.highlights,
                New = i.newPro,
                ProductImportDate = i.importDate,
                SubCategoryId = i.subcategory,

                Type = (
                    from product in _DbContext.Products
                    from price in _DbContext.ProductPrices
                    from type in _DbContext.ProductTypes
                    where product.ProductId == i.id &&
                          price.ProductTypeId == type.ProductTypeId && price.ProductId == product.ProductId
                    select new { id = type.ProductTypeId, name = type.ProductTypeName }
                ).Select(t => new Dtos.Type()
                {
                    ProductTypeId = t.id,
                    ProductTypeName = t.name
                }).ToList(),

                Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.id).ToList(),
                ProductImages = _DbContext.ProductImages.Where(img => img.ProductId == i.id).Select(i => i.ProductImagePath).FirstOrDefault(),

                Brand = i.brandName,
                Shop = i.shopName,
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
        public async Task<List<ProductInBrandModel>> getProductsInBrand(int BrandId, int pageindex, int pagesize)
        {
            var query = from product in _DbContext.Products
                        join brand in _DbContext.Brands on product.BrandId equals brand.BrandId
                        join shop in _DbContext.Shops on product.ShopId equals shop.ShopId
                        where brand.BrandId == BrandId
                        orderby product.SubCategoryId
                        select new
                        {
                            id = product.ProductId,
                            name = product.ProductName,
                            discount = product.DiscountPercent,
                            status = product.Status,
                            highlights = product.Highlights,
                            newPro = product.New,
                            importDate = product.ProductImportDate,
                            subcategory = product.SubCategoryId,

                            brandName = brand.BrandName,
                            shopName = shop.ShopName,
                        };

            var list = query.AsQueryable().Select(i => new ProductInBrandModel()
            {
                ProductId = i.id,
                ProductName = i.name,
                DiscountPercent = i.discount,
                Status = i.status,
                Highlights = i.highlights,
                New = i.newPro,
                ProductImportDate = i.importDate,
                SubCategoryId = i.subcategory,

                Type = (
                    from product in _DbContext.Products
                    from price in _DbContext.ProductPrices
                    from type in _DbContext.ProductTypes
                    where product.ProductId == i.id &&
                          price.ProductTypeId == type.ProductTypeId && price.ProductId == product.ProductId
                    select new { id = type.ProductTypeId, name = type.ProductTypeName }
                ).Select(t => new Dtos.Type()
                {
                    ProductTypeId = t.id,
                    ProductTypeName = t.name
                }).ToList(),

                Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.id).ToList(),
                ProductImages = _DbContext.ProductImages.Where(img => img.ProductId == i.id).Select(i => i.ProductImagePath).FirstOrDefault(),

                Brand = i.brandName,
                Shop = i.shopName,
            });

            var result = await PaginatedList<ProductInBrandModel>.CreateAsync(list, pageindex, pagesize);

            return result;
        }
    
        public async Task<PageResult<ProductInBrandModel>> getProductBySubCategoryPaginated(int BrandId, int SubCategoryId, int pageindex, int pagesize)
        {
            return null;
        }
    }
}
