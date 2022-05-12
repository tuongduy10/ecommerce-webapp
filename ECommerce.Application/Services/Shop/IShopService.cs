using ECommerce.Application.Common;
using ECommerce.Application.Services.Shop.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Shop
{
    public interface IShopService
    {
        Task<List<ShopGetModel>> getShopList();
        Task<List<ShopGetModel>> getUnconfirmedShop();
        Task<ShopDetailModel> getShopDetail(int ShopId);
        Task<ApiResponse> updateShopStatus(int ShopId, byte status);
        Task<ApiResponse> SaleRegistration(SaleRegistrationRequest request);
        Task<ApiResponse> isRegisted(int id);
        Task<ApiResponse> deleteShop(int id);
    }
}
