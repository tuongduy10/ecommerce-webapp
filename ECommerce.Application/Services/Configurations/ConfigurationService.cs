using ECommerce.Application.Common;
using ECommerce.Application.Services.Configurations.Dtos;
using ECommerce.Application.Services.Configurations.Dtos.Footer;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ECommerceContext _DbContext;
        public ConfigurationService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<string> getPhoneNumber()
        {
            var query = from con in _DbContext.Configurations select con;
            var result = await query.Select(i => i.PhoneNumber).FirstAsync();

            return result;
        }
        public async Task<List<BannerModel>> getBanner()
        {
            var query = from banner in _DbContext.Banners select banner;
            var result = await query.Select(i => new BannerModel()
            {
                BannerId = i.BannerId,
                BannerPath = i.BannerPath,
                Status = i.Status
            }).ToListAsync();

            return result;
        }
        public async Task<ConfigurationModel> getConfiguration()
        {
            var query = from con in _DbContext.Configurations select con;
            var result = await query.Select(i => new ConfigurationModel()
            {
                Id = i.Id,
                WebsiteName = i.WebsiteName,
                LogoPath = i.LogoPath,
                FaviconPath = i.FaviconPath,
                StartTime = i.StartTime,
                EndTime = i.EndTime,
                Owner = i.Owner,
                FacebookUrl = i.FacebookUrl,
                Mail = i.Mail,
                PhoneNumber = i.PhoneNumber,
                Address = i.Address,
                AddressUrl = i.AddressUrl,
            }).FirstAsync();

            return result;
        }
        public async Task<ApiResponse> UpdateAddress(AddressUpdateRequest request)
        {
            var config = await _DbContext.Configurations.Where(i => i.Id == request.Id).FirstOrDefaultAsync();
            if (config != null)
            {
                config.Address = request.Address;
                config.AddressUrl = request.AddressUrl;
                _DbContext.SaveChangesAsync().Wait();

                return new ApiSuccessResponse("Cập nhật thành công");
            }

            return new ApiFailResponse("Cập nhật không thành công");
        }
        public async Task<ApiResponse> UpdateTime(TimeUpdateRequest request)
        {
            var config = await _DbContext.Configurations.Where(i => i.Id == request.Id).FirstOrDefaultAsync();
            if (config != null)
            {
                config.StartTime = TimeSpan.Parse(request.StartTime);
                config.EndTime = TimeSpan.Parse(request.EndTime);
                _DbContext.SaveChangesAsync().Wait();

                return new ApiSuccessResponse("Cập nhật thành công");
            }

            return new ApiFailResponse("Cập nhật không thành công");
        }
        public async Task<ApiResponse> updateConfig(UpdateConfigRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Mail)) return new ApiFailResponse("Thông tin không được để trống");
                if (string.IsNullOrEmpty(request.Owner)) return new ApiFailResponse("Thông tin không được để trống");
                if (string.IsNullOrEmpty(request.PhoneNumber)) return new ApiFailResponse("Thông tin không được để trống");
                if (string.IsNullOrEmpty(request.WebsiteName)) return new ApiFailResponse("Thông tin không được để trống");

                var config = await _DbContext.Configurations.Where(i => i.Id == 1).FirstOrDefaultAsync();
                config.Mail = request.Mail;
                config.Owner = request.Owner;
                config.FacebookUrl = request.FacebookUrl == "" ? "www.facebook.com" : request.FacebookUrl;
                config.PhoneNumber = request.PhoneNumber;
                config.WebsiteName = request.WebsiteName;

                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại");
            }
        }
    }
}
