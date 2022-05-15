using ECommerce.Application.Common;
using ECommerce.Application.Services.Configurations.Dtos;
using ECommerce.Application.Services.Configurations.Dtos.Footer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public interface IConfigurationService
    {
        Task<ConfigurationModel> getConfiguration();
        Task<List<BannerModel>> getBanner();
        Task<ApiResponse> UpdateAddress(AddressUpdateRequest request);
        Task<ApiResponse> UpdateTime(TimeUpdateRequest request);
        Task<ApiResponse> updateConfig(UpdateConfigRequest request);
    }
}
