using ECommerce.Application.Common;
using ECommerce.Application.Services.Shop.Dtos;
using ECommerce.Application.Services.Shop.Enums;
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
        public async Task<ApiResponse> AddShop(ShopAddRequest request)
        {
            try
            {
                // Check null
                if (string.IsNullOrEmpty(request.name.Trim())) return new ApiFailResponse("Thông tin không được để trống");
                if (string.IsNullOrEmpty(request.phone.Trim())) return new ApiFailResponse("Thông tin không được để trống");

                // Check already exist
                var checkName = await _DbContext.Shops.Where(s => s.ShopName == request.name.Trim()).FirstOrDefaultAsync();
                if (checkName != null) return new ApiFailResponse("Tên đã tồn tại");
                var checkPhone = await _DbContext.Shops.Where(s => s.ShopPhoneNumber == request.phone.Trim()).FirstOrDefaultAsync();
                if (checkPhone != null) return new ApiFailResponse("Số điện thoại đã tồn tại");
                if (request.mail != null)
                {
                    var checkMail = await _DbContext.Shops.Where(s => s.ShopMail == request.mail.Trim()).FirstOrDefaultAsync();
                    if (checkMail != null) return new ApiFailResponse("Mail đã tồn tại");
                }

                Data.Models.Shop shop = new Data.Models.Shop();
                shop.ShopName = request.name.Trim();
                shop.ShopPhoneNumber = request.phone.Trim();
                shop.ShopAddress = request.address.Trim();
                shop.ShopCityCode = request.cityCode;
                shop.ShopDistrictCode = request.districtCode;
                shop.ShopWardCode = request.wardCode;
                shop.ShopMail = request.mail.Trim();
                shop.ShopJoinDate = DateTime.Now;
                shop.Tax = (byte)request.tax;
                shop.Status = (int)enumShopStatus.Available; // Available..

                await _DbContext.Shops.AddAsync(shop);
                await _DbContext.SaveChangesAsync();

                if (request.brandIds.Count > 0)
                {
                    foreach (var id in request.brandIds)
                    {
                        Data.Models.ShopBrand brand = new Data.Models.ShopBrand
                        {
                            BrandId = id,
                            ShopId = shop.ShopId
                        };
                        await _DbContext.ShopBrands.AddAsync(brand);
                    }
                    await _DbContext.SaveChangesAsync();
                }

                return new ApiSuccessResponse("Thêm thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Thêm thất bại, lỗi: " + error.Message);
            }
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
                                        ShopJoinDate = (DateTime)i.ShopJoinDate,
                                        ShopWardCode = i.ShopWardCode,
                                        Status = (byte)i.Status,
                                        UserName = _DbContext.Users
                                                    .Where(u => u.UserId == i.UserId)
                                                    .Select(u => u.UserFullName)
                                                    .FirstOrDefault(),
                                        UserId = i.UserId == null ? 0 : (int)i.UserId
                                    })
                                    .ToListAsync();
            var result = list.OrderByDescending(i => i.ShopId).ToList();
            return result;
        }
        public async Task<List<ShopGetModel>> getShopListWithNoUser()
        {
            var list = await _DbContext.Shops
                                    .Where(i => i.Status != 4 && 
                                           i.Status != 2 &&
                                           i.User == null
                                    )
                                    .Select(i => new ShopGetModel()
                                    {
                                        ShopId = i.ShopId,
                                        ShopName = i.ShopName,
                                        ShopPhoneNumber = i.ShopPhoneNumber,
                                        ShopMail = i.ShopMail,
                                        ShopAddress = i.ShopAddress,
                                        ShopCityCode = i.ShopCityCode,
                                        ShopDistrictCode = i.ShopDistrictCode,
                                        ShopJoinDate = (DateTime)i.ShopJoinDate,
                                        ShopWardCode = i.ShopWardCode,
                                        Status = (byte)i.Status,
                                        UserName = _DbContext.Users
                                                    .Where(u => u.UserId == i.UserId)
                                                    .Select(u => u.UserFullName)
                                                    .FirstOrDefault(),
                                    })
                                    .ToListAsync();
            var result = list.OrderByDescending(i => i.ShopId).ToList();
            return result;
        }
        public async Task<List<ShopGetModel>> getShopListBySystemUserAccount()
        {
            var list = await _DbContext.Shops
                                    .Where(i => i.Status != 4 &&
                                           i.Status != 2 &&
                                           (i.User.IsSystemAccount == true || i.UserId == null)
                                    )
                                    .Select(i => new ShopGetModel()
                                    {
                                        ShopId = i.ShopId,
                                        ShopName = i.ShopName,
                                        ShopPhoneNumber = i.ShopPhoneNumber,
                                        ShopMail = i.ShopMail,
                                        ShopAddress = i.ShopAddress,
                                        ShopCityCode = i.ShopCityCode,
                                        ShopDistrictCode = i.ShopDistrictCode,
                                        ShopJoinDate = (DateTime)i.ShopJoinDate,
                                        ShopWardCode = i.ShopWardCode,
                                        Status = (byte)i.Status,
                                        UserName = _DbContext.Users
                                                    .Where(u => u.UserId == i.UserId)
                                                    .Select(u => u.UserFullName)
                                                    .FirstOrDefault(),
                                    })
                                    .ToListAsync();
            var result = list.OrderByDescending(i => i.ShopId).ToList();
            return result;
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
                                        ShopJoinDate = (DateTime)i.ShopJoinDate,
                                        ShopWardCode = i.ShopWardCode,
                                        Status = (byte)i.Status,
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
            // Missing updating user role (buyer to seller)
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
            if (string.IsNullOrEmpty(request.ShopAccountName)) return new ApiFailResponse("Thông tin không được để trống");

            // Check already exist
            var checkName = await _DbContext.Shops.Where(s => s.ShopName == request.ShopName).FirstOrDefaultAsync();
            if (checkName != null) return new ApiFailResponse("Tên đã tồn tại");
            var checkPhone = await _DbContext.Shops.Where(s => s.ShopPhoneNumber == request.ShopPhoneNumber).FirstOrDefaultAsync();
            if (checkPhone != null) return new ApiFailResponse("Số điện thoại đã tồn tại");
            var checkMail = await _DbContext.Shops.Where(s => s.ShopMail == request.ShopMail).FirstOrDefaultAsync();
            if (checkMail != null) return new ApiFailResponse("Mail đã tồn tại");
            var checkUser = await _DbContext.Shops.Where(s => s.UserId == request.UserId).FirstOrDefaultAsync();
            if (checkUser != null) return new ApiFailResponse("Tài khoản này đã đăng ký bán hàng");

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
            shop.Status = (byte?)enumShopStatus.Pending; // waiting..

            Data.Models.ShopBank bank = new Data.Models.ShopBank();
            bank.ShopBankName = request.ShopBankName;
            bank.ShopAccountNumber = request.ShopAccountNumber;
            bank.ShopAccountName = request.ShopAccountName;
            shop.ShopBanks.Add(bank);

            await _DbContext.Shops.AddAsync(shop);
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
        public async Task<ShopDetailManagedModel> getShopDetailManage(int shopId)
        {
            var shop = await _DbContext.Shops
                .Where(i => i.ShopId == shopId)
                .Select(i => new ShopDetailManagedModel
                {
                    // Shop
                    ShopId = i.ShopId,
                    ShopName = i.ShopName,
                    ShopPhoneNumber = i.ShopPhoneNumber,
                    ShopMail = i.ShopMail,
                    ShopAddress = i.ShopAddress,
                    ShopCityCode = i.ShopCityCode,
                    ShopDistrictCode = i.ShopDistrictCode,
                    ShopJoinDate = (DateTime)i.ShopJoinDate,
                    ShopWardCode = i.ShopWardCode,
                    Status = (byte)i.Status,
                    Tax = (byte)i.Tax,
                    ShopBank = _DbContext.ShopBanks
                        .Where(b => b.ShopId == shopId)
                        .Select(b => new ShopBankModel()
                        {
                            ShopAccountName = b.ShopAccountName,
                            ShopBankName = b.ShopBankName,
                            ShopAccountNumber = b.ShopAccountNumber
                        })
                        .FirstOrDefault(),
                    ShopBrands = _DbContext.ShopBrands
                        .Where(br => br.ShopId == shopId)
                        .Select(br => br.BrandId)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return shop;
        }
        public async Task<ApiResponse> updateShop(ShopUpdateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.name)) return new ApiFailResponse("Tên không được để trống");
                if (string.IsNullOrEmpty(request.phone)) return new ApiFailResponse("Số điện thoại không được để trống");

                var shop = await _DbContext.Shops.Where(i => i.ShopId == request.id).FirstOrDefaultAsync();
                if (shop == null) return new ApiFailResponse("Shop không tồn tại");
                
                if (shop.ShopName != request.name.Trim()) // required
                {
                    var hasName = await _DbContext.Shops.Where(i => i.ShopName == request.name).FirstOrDefaultAsync() != null;
                    if (hasName) return new ApiFailResponse("Tên đã tồn tại");
                    shop.ShopName = request.name.Trim(); 
                }
                if (shop.ShopPhoneNumber != request.phone.Trim()) // required
                {
                    var hasPhone = await _DbContext.Shops.Where(i => i.ShopPhoneNumber == request.phone.Trim() && i.ShopId != request.id).FirstOrDefaultAsync() != null;
                    if (hasPhone) return new ApiFailResponse("Số điện thoại đã tồn tại");
                    shop.ShopPhoneNumber = request.phone.Trim();
                }
                shop.ShopMail = null;
                if (!string.IsNullOrEmpty(request.mail))
                {
                    var hasMail = await _DbContext.Shops.Where(i => i.ShopMail == request.mail.Trim() && i.ShopId != request.id).FirstOrDefaultAsync() != null;
                    if (hasMail) return new ApiFailResponse("Mail đã tồn tại");
                    shop.ShopMail = request.mail.Trim();
                }

                shop.ShopAddress = "";
                if(!string.IsNullOrEmpty(request.address)) 
                    shop.ShopAddress = request.address.Trim();
                shop.ShopCityCode = "";
                if(!string.IsNullOrEmpty(request.cityCode)) 
                    shop.ShopCityCode = request.cityCode.Trim();
                shop.ShopDistrictCode = "";
                if(!string.IsNullOrEmpty(request.districtCode))
                    shop.ShopDistrictCode = request.districtCode.Trim();
                shop.ShopWardCode = "";
                if(!string.IsNullOrEmpty(request.wardCode))
                    shop.ShopWardCode = request.wardCode.Trim();

                shop.Tax = (byte?)request.tax;
                await _DbContext.SaveChangesAsync();

                var bank = await _DbContext.ShopBanks.Where(i => i.ShopId == request.id).FirstOrDefaultAsync();
                // Add new if not exist
                if (bank == null)
                {
                    string accountName = "";
                    if (!string.IsNullOrEmpty(request.shopbank.ShopAccountName))
                        accountName = request.shopbank.ShopAccountName.Trim();
                    string bankName = "";
                    if (!string.IsNullOrEmpty(request.shopbank.ShopBankName))
                        bankName = request.shopbank.ShopBankName.Trim();
                    string accountNumber = "";
                    if (!string.IsNullOrEmpty(request.shopbank.ShopAccountNumber))
                        accountNumber = request.shopbank.ShopAccountNumber.Trim();

                    var newBank = new Data.Models.ShopBank
                    {
                        ShopAccountName = accountName,
                        ShopBankName = bankName,
                        ShopAccountNumber = accountNumber,
                        ShopId = shop.ShopId,
                    };

                    await _DbContext.ShopBanks.AddAsync(newBank);
                    await _DbContext.SaveChangesAsync();
                }
                // Update if it exist
                else
                {
                    bank.ShopAccountName = "";
                    if (!string.IsNullOrEmpty(request.shopbank.ShopAccountName))
                        bank.ShopAccountName = request.shopbank.ShopAccountName.Trim();
                    bank.ShopBankName = "";
                    if (!string.IsNullOrEmpty(request.shopbank.ShopBankName))
                        bank.ShopBankName = request.shopbank.ShopBankName.Trim();
                    bank.ShopAccountNumber = "";
                    if (!string.IsNullOrEmpty(request.shopbank.ShopAccountNumber))
                        bank.ShopAccountNumber = request.shopbank.ShopAccountNumber.Trim();

                    await _DbContext.SaveChangesAsync();
                }
                

                var brands = await _DbContext.ShopBrands.Where(i => i.ShopId == request.id).ToListAsync();
                // Remove previous brands if exist
                if (brands.Count > 0)
                {
                    _DbContext.ShopBrands.RemoveRange(brands);
                    await _DbContext.SaveChangesAsync();
                }
                // Add new brands if has items
                if (request.shopBrands != null)
                {
                    foreach (var id in request.shopBrands)
                    {
                        var brand = new Data.Models.ShopBrand
                        {
                            BrandId = id,
                            ShopId = shop.ShopId
                        };
                        await _DbContext.ShopBrands.AddAsync(brand);
                    }
                    await _DbContext.SaveChangesAsync();
                }
                
                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Cập nhật không thành công, lỗi: " + error.Message);
            }
        }
    }
}
