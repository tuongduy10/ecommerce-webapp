using ECommerce.Application.BaseServices.Discount.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Discount
{
    public class DiscountService : IDiscountService
    {
        private ECommerceContext _DbContext;
        public DiscountService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<DiscountGetModel> getDisount(int shopId, int brandId)
        {
            var now = DateTime.Now;

            // Disable all discount out of date or out of stock
            var list = await _DbContext.Discounts.ToListAsync();
            foreach (var item in list)
            {
                if (item.EndDate < now || item.DiscountStock == 0)
                {
                    item.Status = 0;
                }
            }
            _DbContext.SaveChangesAsync().Wait();

            // Global discount
            var forGlobalDiscount = await _DbContext.Discounts
                .Where( i => i.StartDate <= now && 
                    i.EndDate >= now && 
                    i.ForGlobal == true &&
                    i.Status == 1
                )
                .Select(i => new DiscountGetModel
                {
                    DiscountId = i.DiscountId,
                    DiscountCode = i.DiscountCode,
                    DiscountValue = i.DiscountValue,
                    IsPercent = i.IsPercent
                })
                .FirstOrDefaultAsync();
            if (forGlobalDiscount != null) 
                return forGlobalDiscount;


            // Shop discount
            var forShopDiscount = await _DbContext.Discounts
                .Where(
                    i => i.StartDate <= now && 
                    i.EndDate >= now && 
                    i.ForShop == true && 
                    i.Status == 1 && 
                    i.Shops.Where(s => s.ShopId == shopId).Count() > 0
                )
                .Select(i => new DiscountGetModel
                {
                    DiscountId = i.DiscountId,
                    DiscountCode = i.DiscountCode,
                    DiscountValue = i.DiscountValue,
                    IsPercent = i.IsPercent
                })
                .FirstOrDefaultAsync();
            if (forShopDiscount != null) 
                return forShopDiscount;

            // Brand Discount
            var forBrandDiscount = await _DbContext.Discounts
                .Where(
                    i => i.StartDate <= now &&
                    i.EndDate >= now &&
                    i.ForBrand == true &&
                    i.Status == 1 &&
                    i.Brands.Where(s => s.BrandId == brandId).Count() > 0
                )
                .Select(i => new DiscountGetModel {
                    DiscountId = i.DiscountId,
                    DiscountCode = i.DiscountCode,
                    DiscountValue = i.DiscountValue,
                    IsPercent = i.IsPercent
                })
                .FirstOrDefaultAsync();
            if (forBrandDiscount != null) 
                return forBrandDiscount;

            return null;
        }
        public async Task<DiscountValue> getDiscountValue(string code)
        {
            var result = await _DbContext.Discounts
                .Where(
                    i => i.DiscountCode == code && 
                    i.EndDate >= DateTime.Now &&
                    i.Status == 1
                )
                .Select(i => new DiscountValue { 
                    value = (int)i.DiscountValue,
                    isPercent = (bool)i.IsPercent
                })
                .FirstOrDefaultAsync();            
            return result;
        }
    }
}
