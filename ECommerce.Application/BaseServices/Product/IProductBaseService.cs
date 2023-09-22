using ECommerce.Application.Common;
using ECommerce.Application.BaseServices.Product.Dtos;
using ECommerce.Application.BaseServices.Product.Models;
using ECommerce.Application.BaseServices.Product.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Product
{
    public interface IProductBaseService
    {
        Task<PageResult<ProductInBrandModel>> getProductPaginated(ProductGetRequest request);
        Task<List<ProductInBrandModel>> getProductSuggestion();
        Task<ProductDetailModel> getProductDetail(int id);
        Task<ProductDetailModel> GetProductDetailManaged(int id);
        Task<List<Option>> getProductOption(int id);
        Task<List<ProductShopListModel>> getProductByUser(int shopId, int subcategoryId);
        Task<List<ProductShopListModel>> getAllManaged(int subcategoryId);
        Task<PageResult<ProductInBrandModel>> getProductInPagePaginated(ProductGetRequest request);
        Task<Price> getProductPrice(int productId, int typeId);
        Task<ApiResponse> AddProduct(ProductSaveRequest request);
        Task<Response<ProductUpdateResponse>> UpdateProduct(ProductSaveRequest request);
        Task<Response<ProductDeleteResponse>> DeleteProduct(int id);
        Task<Response<ProductDeleteResponse>> DeleteProducts(List<int> ids);
        Task<ApiResponse> DisableProducts(List<int> ids);
        Task<ApiResponse> ApproveProducts(List<int> ids);
        Task<List<SizeGuideModel>> SizeGuideList();
        Task<ApiResponse> AddSizeGuide(SizeGuideAddRequest request);
        Task<ApiResponse> UpdateSizeGuide(SizeGuideAddRequest request);
        Task<Response<Data.Models.SizeGuide>> SizeGuideDetail(int id);
        Task<ApiResponse> DeleteSizeGuide(int id);
        Task<Response<Data.Models.SizeGuide>> GetSizeGuideBySub(int id);
        Task<Response<string>> DeleteProductImage(int id);
        Task<Response<string>> DeleteProductUserImage(int id);
    }
}
