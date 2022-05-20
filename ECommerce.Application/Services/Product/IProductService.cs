using ECommerce.Application.Common;
using ECommerce.Application.Services.Product.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product
{
    public interface IProductService
    {
        Task<PageResult<ProductInBrandModel>> getProductPaginated(ProductGetRequest request);
        Task<List<ProductInBrandModel>> getProductSuggestion();
        Task<ProductDetailModel> getProductDeatil(int id);
        Task<List<Option>> getProductOption(int id);
        Task<List<ProductShopListModel>> getProductByUser(int shopId, int subcategoryId);
    }
}
