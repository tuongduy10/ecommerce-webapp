using ECommerce.Application.Common;
using ECommerce.Application.Services.Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Shop
{
    public interface IShopService
    {
        Task<List<ShopGetModel>> getUnconfirmedShop();
        Task<ApiResponse> updateShopStatus(int ShopId, byte status);
    }
}
