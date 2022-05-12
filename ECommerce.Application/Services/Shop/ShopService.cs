using ECommerce.Application.Common;
using ECommerce.Application.Services.Shop.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Shop
{
    public class ShopService : IShopService
    {
        private ECommerceContext _DbContext; 
        public ShopService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<List<ShopGetModel>> getShopList()
        {
            var list = await _DbContext.Shops
                                    .Where(i => i.Status != 4 && i.Status != 2)
                                    .Select(i => new ShopGetModel() {
                                        ShopId = i.ShopId,
                                        ShopName = i.ShopName,
                                        ShopPhoneNumber = i.ShopPhoneNumber,
                                        ShopMail = i.ShopMail,
                                        ShopAddress = i.ShopAddress,
                                        ShopCityCode = i.ShopCityCode,
                                        ShopDistrictCode = i.ShopDistrictCode,
                                        ShopJoinDate = i.ShopJoinDate,
                                        ShopWardCode = i.ShopWardCode,
                                        Status = i.Status,
                                        UserName = _DbContext.Users
                                                    .Where(u => u.UserId == i.UserId)
                                                    .Select(u => u.UserFullName)
                                                    .FirstOrDefault(),
                                    })
                                    .OrderByDescending(i => i.ShopJoinDate)
                                    .ToListAsync();
            return list;
        }
        public async Task<List<ShopGetModel>> getUnconfirmedShop()
        {
            var result = await _DbContext.Shops
                                    .Where(i => i.Status == 2 || i.Status == 4)
                                    .Select(i => new ShopGetModel()
                                    {
                                        ShopId = i.ShopId,
                                        ShopName = i.ShopName,
                                        ShopPhoneNumber = i.ShopPhoneNumber,
                                        ShopMail = i.ShopMail,
                                        ShopAddress = i.ShopAddress,
                                        ShopCityCode = i.ShopCityCode,
                                        ShopDistrictCode = i.ShopDistrictCode,
                                        ShopJoinDate = i.ShopJoinDate,
                                        ShopWardCode = i.ShopWardCode,
                                        Status = i.Status,
                                        UserName = _DbContext.Users
                                                        .Where(u => u.UserId == i.UserId)
                                                        .Select(u => u.UserFullName)
                                                        .FirstOrDefault(),
                                    })
                                    .ToListAsync();

            return result;
        }
        public async Task<ApiResponse> updateShopStatus(int ShopId, byte status)
        {
            var shop = await _DbContext.Shops.Where(i => i.ShopId == ShopId).FirstOrDefaultAsync();
            if (shop != null)
            {
                shop.Status = status;
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Cập nhật trạng thái thành công");
            }
            return new ApiFailResponse("Cập nhật trạng thái thất bại");
        }
        public async Task<ApiResponse> SaleRegistration(SaleRegistrationRequest request)
        {
            // Check null
            if (string.IsNullOrEmpty(request.ShopName)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.ShopPhoneNumber)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.ShopAddress)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.ShopCityCode)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.ShopDistrictCode)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.ShopWardCode)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.ShopBankName)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.ShopAccountNumber)) return new ApiFailResponse("Thông tin không được để trống");

            // Check already exist
            var checkName = await _DbContext.Shops.Where(s => s.ShopName == request.ShopName).FirstOrDefaultAsync();
            if (checkName != null) return new ApiFailResponse("Tên đã tồn tại");
            var checkPhone = await _DbContext.Shops.Where(s => s.ShopPhoneNumber == request.ShopPhoneNumber).FirstOrDefaultAsync();
            if (checkPhone != null) return new ApiFailResponse("Số điện thoại đã tồn tại");
            var checkMail = await _DbContext.Shops.Where(s => s.ShopMail == request.ShopMail).FirstOrDefaultAsync();
            if (checkMail != null) return new ApiFailResponse("Mail đã tồn tại");

            Data.Models.Shop shop = new Data.Models.Shop();
            shop.ShopName = request.ShopName;
            shop.ShopPhoneNumber = request.ShopPhoneNumber;
            shop.ShopAddress = request.ShopAddress;
            shop.ShopCityCode = request.ShopCityCode;
            shop.ShopDistrictCode = request.ShopDistrictCode;
            shop.ShopWardCode = request.ShopWardCode;
            shop.ShopMail = request.ShopMail;
            shop.ShopJoinDate = DateTime.Now;
            shop.UserId = request.UserId;
            shop.Status = 2; // waiting..
            await _DbContext.Shops.AddAsync(shop);
            await _DbContext.SaveChangesAsync();

            Data.Models.ShopBank bank = new Data.Models.ShopBank();
            bank.ShopId = shop.ShopId;
            bank.ShopBankName = request.ShopBankName;
            bank.ShopAccountNumber = request.ShopAccountNumber;
            await _DbContext.ShopBanks.AddAsync(bank);
            await _DbContext.SaveChangesAsync();

            return new ApiSuccessResponse("Đăng ký thành công");
        }
        public async Task<ApiResponse> isRegisted(int id)
        {
            var result = await _DbContext.Shops.Where(i => i.UserId == id).FirstOrDefaultAsync();
            if (result == null)
            {
                return new ApiFailResponse("Chưa đăng ký");
            }

            return new ApiSuccessResponse("Trạng thái đăng ký", result);
        }
        public async Task<ApiResponse> deleteShop(int userid)
        {
            var shop = await _DbContext.Shops.Where(i => i.UserId == userid).FirstOrDefaultAsync();
            if (shop == null)
            {
                return new ApiFailResponse("Chưa đăng ký");
            }
            if (shop.Status != 4)
            {
                return new ApiFailResponse("Không thể xóa");
            }

            _DbContext.Remove(shop);
            await _DbContext.SaveChangesAsync();
            return new ApiSuccessResponse("Xóa thành công");
        }

        public async Task<ShopDetailModel> getShopDetail(int ShopId)
        {
            var shop = await _DbContext.Shops
                                    .Where(i => i.Status != 4 && i.Status != 2)
                                    .Select(i => new ShopDetailModel()
                                    {
                                        // Shop
                                        ShopId = i.ShopId,
                                        ShopName = i.ShopName,
                                        ShopPhoneNumber = i.ShopPhoneNumber,
                                        ShopMail = i.ShopMail,
                                        ShopAddress = i.ShopAddress,
                                        ShopCityCode = i.ShopCityCode,
                                        ShopDistrictCode = i.ShopDistrictCode,
                                        ShopJoinDate = i.ShopJoinDate,
                                        ShopWardCode = i.ShopWardCode,
                                        Status = i.Status,
                                        Tax = i.Tax,

                                        // Shop's owner
                                        User = _DbContext.Users
                                                    .Where(u => u.UserId == i.UserId)
                                                    .Select(u => new Dtos.User()
                                                    {
                                                        UserId = u.UserId,
                                                        UserName = u.UserFullName,
                                                        UserJoinDate = u.UserJoinDate,
                                                        UserMail = u.UserMail,
                                                        UserPhone = u.UserPhone,
                                                        UserAddress = u.UserAddress,
                                                        UserWardCode = u.UserWardCode,
                                                        UserDistrictCode = u.UserDistrictCode,
                                                        UserCityCode = u.UserCityCode
                                                    })
                                                    .FirstOrDefault(),
                                        ShopBank = _DbContext.ShopBanks
                                                        .Where(b => b.ShopId == i.ShopId)
                                                        .Select(b => new ShopBank()
                                                        {
                                                            ShopAccountName = b.ShopAccountName,
                                                            ShopBankName = b.ShopBankName,
                                                            ShopAccountNumber = b.ShopAccountNumber
                                                        })
                                                        .FirstOrDefault(),
                                        Brands = _DbContext.ShopBrands
                                                        .Where(br => br.ShopId == i.ShopId)
                                                        .Select(br => new Dtos.Brand()
                                                        {
                                                            BrandId = br.Brand.BrandId,
                                                            BrandName = br.Brand.BrandName
                                                        })
                                                        .ToList()
                                        // Shop's bank account
                                    })
                                    .FirstOrDefaultAsync();

            return shop;
        }
    }
}
