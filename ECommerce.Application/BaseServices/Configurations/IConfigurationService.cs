using ECommerce.Application.Common;
using ECommerce.Application.BaseServices.Configurations.Dtos;
using ECommerce.Application.BaseServices.Configurations.Dtos.Footer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Configurations
{
    public interface IConfigurationService
    {
        Task<string> getPhoneNumber();
        Task<ConfigurationModel> getConfiguration();
        Task<List<BannerModel>> getBanner();
        Task<ApiResponse> UpdateAddress(AddressUpdateRequest request);
        Task<ApiResponse> UpdateTime(TimeUpdateRequest request);
        Task<ApiResponse> updateConfig(UpdateConfigRequest request);
    }
}
