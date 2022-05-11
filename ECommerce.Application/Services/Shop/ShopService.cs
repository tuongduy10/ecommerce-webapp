using ECommerce.Application.Common;
using ECommerce.Application.Services.Shop.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Shop
{
    public class ShopService : IShopService
    {
        private ECommerceContext _DbContext; 
        public ShopService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<List<ShopGetModel>> getUnconfirmedShop()
        {
            var result = await _DbContext.Shops
                                    .Where(i => i.Status == 2)
                                    .Select(i => new ShopGetModel()
                                    {
                                        ShopId = i.ShopId,
                                        ShopName = i.ShopName,
                                        ShopPhoneNumber = i.ShopPhoneNumber,
                                        ShopMail = i.ShopMail,
                                        ShopAddress = i.ShopAddress,
                                        ShopCityCode = i.ShopCityCode,
                                        ShopDistrictCode = i.ShopDistrictCode,
                                        ShopJoinDate = i.ShopJoinDate,
                                        ShopWardCode = i.ShopWardCode,
                                        Status = i.Status,
                                        UserName = _DbContext.Users
                                                    .Where(u => u.UserId == i.UserId)
                                                    .Select(u => u.UserFullName)
                                                    .FirstOrDefault(),
                                    }).ToListAsync();

            return result;
        }

        public async Task<ApiResponse> updateShopStatus(int ShopId, byte status)
        {
            var shop = await _DbContext.Shops.Where(i => i.ShopId == ShopId).FirstOrDefaultAsync();
            if (shop != null)
            {
                shop.Status = status;
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Cập nhật trạng thái thành công");
            }
            return new ApiFailResponse("Cập nhật trạng thái thất bại");
        }
    }
}
