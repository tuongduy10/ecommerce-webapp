using ECommerce.Application.Common;
using ECommerce.Application.Services.Configurations.Dtos.Header;
using ECommerce.Data.Context;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public class HeaderService : IHeaderService
    {
        private readonly ECommerceContext _DbContext;
        public HeaderService(ECommerceContext db)
        {
            _DbContext = db;
        }
        public async Task<List<HeaderModel>> getAll()
        {
            var query = from header in _DbContext.Headers 
                        orderby header.HeaderPosition
                        where header.Status == 1
                        select header;
            var list = await query.Select(i => new HeaderModel()
            {
                HeaderId = i.HeaderId,
                HeaderName = i.HeaderName,
                HeaderPosition = i.HeaderPosition,
                HeaderUrl = i.HeaderUrl,
                Status = i.Status
            })
            .OrderBy(i => i.HeaderPosition)
            .ToListAsync();
            return list;
        }
        public async Task<List<HeaderModel>> getAllManage()
        {
            var query = from header in _DbContext.Headers
                        orderby header.HeaderPosition
                        select header;
            var list = await query.Select(i => new HeaderModel()
            {
                HeaderId = i.HeaderId,
                HeaderName = i.HeaderName,
                HeaderPosition = i.HeaderPosition,
                HeaderUrl = i.HeaderUrl,
                Status = i.Status
            })
            .OrderBy(i => i.HeaderPosition)
            .ToListAsync();
            return list;
        }
        public async Task<ApiResponse> updateHeaderMenu(HeaderUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.HeaderPosition)) return new ApiFailResponse("Thông không được để trống");
            if (string.IsNullOrEmpty(request.HeaderName)) return new ApiFailResponse("Thông không được để trống");

            try
            {
                var config = await _DbContext.Headers.Where(i => i.HeaderId == request.HeaderId).FirstOrDefaultAsync();
                config.HeaderName = request.HeaderName;
                config.HeaderPosition = byte.Parse(request.HeaderPosition);
                config.Status = request.Status;
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại");
            }
        }
        public async Task<ApiResponse> updateLogo(string path)
        {
            try
            {
                var config = await _DbContext.Configurations
                                        .Where(i => i.Id == 1)
                                        .FirstOrDefaultAsync();
                config.LogoPath = path;
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại");
            }
        }
        public async Task<ApiResponse> updateFavicon(string path)
        {
            try
            {
                var config = await _DbContext.Configurations
                                        .Where(i => i.Id == 1)
                                        .FirstOrDefaultAsync();
                config.FaviconPath = path;
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch
            {
                return new ApiFailResponse("Cập nhật thất bại");
            }
        }
        public async Task<ApiResponse> addBanner(List<string> listName)
        {
            try
            {
                foreach (var name in listName)
                {
                    var banner = new Banner();
                    banner.BannerPath = name;
                    await _DbContext.Banners.AddAsync(banner);
                }

                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Thêm thành công");
            }
            catch
            {
                return new ApiFailResponse("Thêm thất bại công");
            }
        }
        public async Task<ApiResponse> deleteBanner(int id)
        {
            try
            {
                var banner = await _DbContext.Banners.Where(b => b.BannerId == id).FirstOrDefaultAsync();
                _DbContext.Banners.Remove(banner);
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Xóa thành công");
            }
            catch
            {
                return new ApiFailResponse("Xóa thất bại");
            }
        }
    }
}
