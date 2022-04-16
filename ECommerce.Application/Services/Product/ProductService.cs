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
                        from type in _DbContext.ProductTypes
                        join brand in _DbContext.Brands on product.BrandId equals brand.BrandId
                        join price in _DbContext.ProductPrices on product.ProductId equals price.ProductId
                        join shop in _DbContext.Shops on product.ShopId equals shop.ShopId
                        join subcategory in _DbContext.SubCategories on product.SubCategoryId equals subcategory.SubCategoryId
                        where type.ProductTypeId == price.ProductTypeId &&
                                type.ProductTypeId == 2 &&
                                brand.BrandId == BrandId
                        select new { product, price, brand, shop, subcategory };

            var list = await query.Select(i => new ProductInBrandModel()
            {
                ProductId = i.product.ProductId,
                ProductName = i.product.ProductName,
                DiscountPercent = i.product.DiscountPercent,
                Status = i.product.Status,
                Highlights = i.product.Highlights,
                New = i.product.New,
                ProductImportDate = i.product.ProductImportDate,
                ProductPrices = i.price.Price,

                ProductImages = _DbContext.ProductImages.Where(img => img.ProductId == i.product.ProductId).Select(i => i.ProductImagePath).ToList(),

                Brand = i.brand.BrandName,
                Shop = i.shop.ShopName,
                SubCategoryId = i.subcategory.SubCategoryId,

            }).ToListAsync();

            return list;
        }
    }
}
