using ECommerce.Application.Common;
using ECommerce.Application.Services.Shop.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Shop
{
    public interface IShopService
    {
        Task<ApiResponse> AddShop(ShopAddRequest request);
        Task<List<ShopGetModel>> getShopList();
        Task<List<ShopGetModel>> getShopListWithNoUser();
        Task<List<ShopGetModel>> getShopListBySystemUserAccount();
        Task<List<ShopGetModel>> getUnconfirmedShop();
        Task<ApiResponse> updateShopStatus(int ShopId, byte status);
        Task<ApiResponse> SaleRegistration(SaleRegistrationRequest request);
        Task<ApiResponse> isRegisted(int id);
        Task<ApiResponse> deleteShop(int id);
        Task<ShopDetailManagedModel> getShopDetailManage(int shopId);
        Task<ApiResponse> updateShop(ShopUpdateRequest request);
    }
}
