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
        public async Task<List<ProductInBrandModel>> getProductsInBrand(int BrandId)
        {
            var query = from product in _DbContext.Products
                        join brand in _DbContext.Brands on product.BrandId equals brand.BrandId
                        join shop in _DbContext.Shops on product.ShopId equals shop.ShopId
                        where brand.BrandId == BrandId
                        select new { 
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

            var list = await query.Select(i => new ProductInBrandModel()
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
                ).Select(t => new Dtos.Type() { 
                    ProductTypeId = t.id,
                    ProductTypeName = t.name
                }).ToList(),

                Price = _DbContext.ProductPrices.Where(price => price.ProductId == i.id).ToList(),
                ProductImages = _DbContext.ProductImages.Where(img => img.ProductId == i.id).Select(i => i.ProductImagePath).FirstOrDefault(),

                Brand = i.brandName,
                Shop = i.shopName,
            }).ToListAsync();

            return list;
        }
    }
}
