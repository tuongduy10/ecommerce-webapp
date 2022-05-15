using ECommerce.Application.Common;
using ECommerce.Application.Services.Configurations.Dtos.Header;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public interface IHeaderService
    {
        Task<ApiResponse> Update(HeaderUpdateRequest request);
        Task<List<HeaderModel>> getAll();
        Task<ApiResponse> addBanner(List<string> listName);
        Task<ApiResponse> deleteBanner(int id);
        Task<ApiResponse> updateLogo(string path);
        Task<ApiResponse> updateWebName(string name);
    }
}
