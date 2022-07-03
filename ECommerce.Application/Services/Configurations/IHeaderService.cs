using ECommerce.Application.Common;
using ECommerce.Application.Services.Configurations.Dtos.Header;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public interface IHeaderService
    {
        Task<ApiResponse> updateHeaderMenu(HeaderUpdateRequest request);
        Task<List<HeaderModel>> getAll();
        Task<List<HeaderModel>> getAllManage();
        Task<ApiResponse> addBanner(List<string> listName);
        Task<ApiResponse> deleteBanner(int id);
        Task<ApiResponse> updateLogo(string path);
        Task<ApiResponse> updateFavicon(string path);
    }
}
