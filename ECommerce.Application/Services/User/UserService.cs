using ECommerce.Application.Common;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Application.Services.User.Enums;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly ECommerceContext _DbContext;
        public UserService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<List<UserGetModel>> getAll()
        {
            var list = await _DbContext.Users.Select(i => new UserGetModel() { 
                UserId = i.UserId,
                UserFullName = i.UserFullName,
                UserJoinDate = (DateTime)i.UserJoinDate,
                UserMail = i.UserMail,
                UserAddress = i.UserAddress,
                UserWardCode = i.UserWardCode,
                UserDistrictCode = i.UserDistrictCode,
                UserCityCode = i.UserCityCode,
                UserPhone = i.UserPhone,
                Status = (bool)i.Status,
            }).ToListAsync();
            var result = list.OrderByDescending(i => i.UserJoinDate).ToList();
            return result;
        }
        public async Task<List<UserGetModel>> getUsersByFiltered(UserGetRequest request)
        {
            var list = await _DbContext.Users
                .Where(i => i.UserId != request.userId)
                .Select(i => new UserGetModel() {
                    UserId = i.UserId,
                    UserFullName = i.UserFullName,
                    UserJoinDate = (DateTime)i.UserJoinDate,
                    UserMail = i.UserMail,
                    UserAddress = i.UserAddress,
                    UserWardCode = i.UserWardCode,
                    UserDistrictCode = i.UserDistrictCode,
                    UserCityCode = i.UserCityCode,
                    UserPhone = i.UserPhone,
                    isSystemAccount = i.IsSystemAccount == null ? false : (bool)i.IsSystemAccount,
                    Status = i.Status == null ? false : (bool)i.Status,
                    IsOnline = i.IsOnline == null ? false : (bool)i.IsOnline,
                    LastOnline = (DateTime)i.LastOnline
                })
                .ToListAsync();

            if (request.all == true) list = list.ToList();
            if (request.isSystemAccount == true) list = list.Where(i => i.isSystemAccount == true).ToList();
            
            var result = list.OrderByDescending(i => i.UserJoinDate).ToList();
            return result;
        }
        public async Task<ApiResponse> CheckUserPhoneNumber(string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber)) return new ApiFailResponse("Thông tin không được để trống");

            var phonenumber = PhoneNumber;
            if (phonenumber.Contains("+84"))
            {
                phonenumber = phonenumber.Replace("+84", "");
                if(!phonenumber.StartsWith("0"))
                {
                    phonenumber = "0" + phonenumber;
                }
            }

            var result = await _DbContext.Users.Where(i => i.UserPhone == phonenumber).FirstOrDefaultAsync();
            if (result != null)
            {
                return new ApiFailResponse("Số điện thoại đã tồn tại");
            }

            return new ApiSuccessResponse("Có thể tạo tài khoản với số điện thoại này");
        }
        public async Task<ApiResponse> SignIn(SignInRequest request)
        {
            if (string.IsNullOrEmpty(request.UserPhone) || string.IsNullOrEmpty(request.Password)) return new ApiFailResponse("Thông tin không được để trống");

            var phonenumber = request.UserPhone;
            if (phonenumber.Contains("+84"))
            {
                phonenumber = phonenumber.Replace("+84", "");
                if (!phonenumber.StartsWith("0"))
                {
                    phonenumber = "0" + phonenumber;
                }
            }

            var result = await _DbContext.Users
                .Where(i => i.UserPhone == phonenumber && i.Password == request.Password)
                .FirstOrDefaultAsync();
            
            if (result == null) return new ApiFailResponse("Mật khẩu hoặc tài khoản không đúng");
            if (result.Status == false) return new ApiFailResponse("Tài khoản đã bị khóa");

            var roles = (
                from role in _DbContext.Roles
                from userrole in _DbContext.UserRoles
                where role.RoleId == userrole.RoleId && userrole.UserId == result.UserId
                select role.RoleName
            ).Distinct().ToList();

            UserGetModel user = new UserGetModel();
            user.UserId = result.UserId;
            user.UserFullName = result.UserFullName;
            user.UserRoles = roles;

            return new ApiSuccessResponse("Đăng nhập thành công", user);
        }
        public async Task<ApiResponse> SignUp(SignUpRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserPhone)) return new ApiFailResponse("Số điện thoại không được để trống");
                if (request.UserPhone.Contains("+84"))
                {
                    request.UserPhone = request.UserPhone.Replace("+84", "");
                    if (!request.UserPhone.StartsWith("0"))
                    {
                        request.UserPhone = "0" + request.UserPhone;
                    }
                }

                if (string.IsNullOrEmpty(request.UserFullName)) return new ApiFailResponse("Vui lòng nhập họ tên");
                if (string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.RePassword)) return new ApiFailResponse("Vui lòng nhập mật khẩu");
                if (request.Password != request.RePassword) return new ApiFailResponse("Mật khẩu không trùng");

                var checkMail = await _DbContext.Users.Where(i => i.UserMail == request.UserMail).FirstOrDefaultAsync();
                if (checkMail != null) return new ApiFailResponse("Mail đã tồn tại");

                var checkPhone = await _DbContext.Users.Where(i => i.UserPhone == request.UserPhone).FirstOrDefaultAsync();
                if (checkPhone != null) return new ApiFailResponse("Số điện thoại đã tồn tại");

                Data.Models.User user = new Data.Models.User();
                user.UserMail = !string.IsNullOrEmpty(request.UserMail) ? request.UserMail.Trim() : null;
                user.UserJoinDate = DateTime.Now;
                user.UserFullName = !string.IsNullOrEmpty(request.UserFullName) ? request.UserFullName.Trim() : null;
                user.UserPhone = !string.IsNullOrEmpty(request.UserPhone) ? request.UserPhone.Trim() : null;
                user.UserAddress = !string.IsNullOrEmpty(request.UserAddress) ? request.UserAddress.Trim() : null;
                user.UserDistrictCode = !string.IsNullOrEmpty(request.UserDistrictCode) ? request.UserDistrictCode.Trim() : null;
                user.UserCityCode = !string.IsNullOrEmpty(request.UserCityCode) ? request.UserCityCode.Trim() : null;
                user.UserWardCode = !string.IsNullOrEmpty(request.UserWardCode) ? request.UserWardCode.Trim() : null;
                user.Password = !string.IsNullOrEmpty(request.RePassword) ? request.RePassword.Trim() : null;
                user.Status = true;
                user.IsSystemAccount = request.isSystemAccount;
                await _DbContext.Users.AddAsync(user);
                await _DbContext.SaveChangesAsync();

                Data.Models.UserRole role = new Data.Models.UserRole();
                role.RoleId = request.RoleId != 0 ? request.RoleId : (int)enumRole.Buyer;
                role.UserId = user.UserId;
                await _DbContext.UserRoles.AddAsync(role);
                await _DbContext.SaveChangesAsync();

                return new ApiSuccessResponse("Tạo tài khoản thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Tạo tài khoản không thành công, lỗi: " + error.Message);
            }
        }
        public async Task<ApiResponse> AddSeller(SignUpRequest request)
        {
            try
            {
                string phonenumber = null;
                // Phone validate
                if (!string.IsNullOrEmpty(request.UserPhone))
                {
                    phonenumber = request.UserPhone.Trim();
                    if (phonenumber.Contains("+84"))
                    {
                        phonenumber = phonenumber.Replace("+84", "");
                        if (!phonenumber.StartsWith("0"))
                            phonenumber = "0" + phonenumber;
                    }
                }

                if (string.IsNullOrEmpty(request.UserFullName)) 
                    return new ApiFailResponse("Vui lòng nhập họ tên");
                if (string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.RePassword)) 
                    return new ApiFailResponse("Vui lòng nhập mật khẩu");
                if (request.Password != request.RePassword) 
                    return new ApiFailResponse("Mật khẩu không trùng");

                var checkMail = await _DbContext.Users
                    .Where(i => request.UserMail != null && i.UserMail == request.UserMail)
                    .FirstOrDefaultAsync();
                if (checkMail != null) 
                    return new ApiFailResponse("Mail đã tồn tại");

                var checkPhone = await _DbContext.Users.Where(i => i.UserPhone == request.UserPhone).FirstOrDefaultAsync();
                if (checkPhone != null) 
                    return new ApiFailResponse("Số điện thoại đã tồn tại");

                Data.Models.User user = new Data.Models.User();
                user.UserMail = request.UserMail != null ? request.UserMail.Trim() : null;
                user.UserJoinDate = DateTime.Now;
                user.UserFullName = request.UserFullName != null ? request.UserFullName.Trim() : null;
                user.UserPhone = phonenumber;

                user.UserPhone = request.UserPhone != null ? request.UserPhone.Trim() : null;
                user.UserAddress = request.UserAddress != null ? request.UserAddress.Trim() : null;
                user.UserDistrictCode = request.UserDistrictCode;
                user.UserCityCode = request.UserCityCode;
                user.UserWardCode = request.UserWardCode;
                user.Password = request.RePassword != null ? request.RePassword.Trim() : null;
                user.Status = true;
                user.IsSystemAccount = request.isSystemAccount;
                await _DbContext.Users.AddAsync(user);
                await _DbContext.SaveChangesAsync();

                // Role
                Data.Models.UserRole role = new Data.Models.UserRole();
                role.RoleId = (int)enumRole.Seller;
                role.UserId = user.UserId;
                await _DbContext.UserRoles.AddAsync(role);
                await _DbContext.SaveChangesAsync();

                // Shop
                if (request.shopIds != null)
                {
                    foreach (var id in request.shopIds)
                    {
                        var shop = await _DbContext.Shops.Where(i => i.ShopId == id).FirstOrDefaultAsync();
                        shop.UserId = user.UserId;
                        await _DbContext.SaveChangesAsync();
                    }
                }

                return new ApiSuccessResponse("Tạo tài khoản thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Tạo tài khoản không thành công, lỗi: " + error.Message);
            }
        }
        public async Task<UserGetModel> UserProfile(int id)
        {
            try
            {
                var user = await _DbContext.Users
                                .Where(i => i.UserId == id)
                                .Select(i => new UserGetModel
                                {
                                    UserId = i.UserId,
                                    UserFullName = i.UserFullName,
                                    UserJoinDate = (DateTime)i.UserJoinDate,
                                    UserMail = i.UserMail,
                                    UserAddress = i.UserAddress,
                                    UserWardCode = i.UserWardCode,
                                    UserDistrictCode = i.UserDistrictCode,
                                    UserCityCode = i.UserCityCode,
                                    UserPhone = i.UserPhone,
                                    UserRoles = i.UserRoles
                                        .Select(ur => ur.Role.RoleName)
                                        .ToList(),
                                    Status = (bool)i.Status,
                                    IsOnline = (bool)i.IsOnline,
                                }).FirstOrDefaultAsync();

                return user;
            }
            catch
            {
                return null;
            }
        }
        public async Task<ApiResponse> UpdateUserProfile(UserUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.UserFullName)) return new ApiFailResponse("Họ tên không thể để trống");

            var user = await _DbContext.Users
                                .Where(i => i.UserId == request.UserId)
                                .FirstOrDefaultAsync();
            if (user != null)
            {
                user.UserFullName = request.UserFullName;
                user.UserMail = request.UserMail;
                user.UserAddress = request.UserAddress;
                user.UserWardCode = request.UserWardCode;
                user.UserDistrictCode = request.UserDistrictCode;
                user.UserCityCode = request.UserCityCode;
                await _DbContext.SaveChangesAsync();

                var roles = (
                    from role in _DbContext.Roles
                    from userrole in _DbContext.UserRoles
                    where role.RoleId == userrole.RoleId && userrole.UserId == request.UserId
                    select role.RoleName
                ).Distinct().ToList();

                UserGetModel result = new UserGetModel();
                result.UserId = user.UserId;
                result.UserFullName = user.UserFullName;
                result.UserRoles = roles;

                return new ApiSuccessResponse("Cập nhật thành công", result);
            }

            return new ApiFailResponse("Cập nhật không thành công");
        }
        public async Task<ApiResponse> UpdateManageUserProfile(UserUpdateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserFullName)) return new ApiFailResponse("Họ tên không thể để trống");

                var user = await _DbContext.Users.Where(i => i.UserId == request.UserId).FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(request.UserFullName)) user.UserFullName = request.UserFullName.Trim();

                user.UserMail = null;
                if (!string.IsNullOrEmpty(request.UserMail))
                {
                    var checkMail = await _DbContext.Users.Where(i => i.UserMail == request.UserMail.Trim() && i.UserId != request.UserId).FirstOrDefaultAsync();
                    if (checkMail != null) return new ApiFailResponse("Mail đã tồn tại");
                    user.UserMail = request.UserMail.Trim();
                }

                user.UserPhone = null;
                if (!string.IsNullOrEmpty(request.UserPhone))
                {
                    var checkPhone = await _DbContext.Users.Where(i => i.UserPhone == request.UserPhone.Trim() && i.UserId != request.UserId).FirstOrDefaultAsync();
                    if (checkPhone != null) return new ApiFailResponse("Số điện thoại đã tồn tại");
                    // Phone validate
                    string phonenumber = request.UserPhone.Trim();
                    if (phonenumber.Contains("+84"))
                    {
                        phonenumber = phonenumber.Replace("+84", "");
                        if (!phonenumber.StartsWith("0"))
                        {
                            phonenumber = "0" + phonenumber;
                        }
                    }
                    user.UserPhone = phonenumber;
                }

                user.UserAddress = null;
                if (!string.IsNullOrEmpty(request.UserAddress)) user.UserAddress = request.UserAddress.Trim();
                user.UserWardCode = null;
                if (!string.IsNullOrEmpty(request.UserWardCode)) user.UserWardCode = request.UserWardCode.Trim();
                user.UserDistrictCode = null;
                if (!string.IsNullOrEmpty(request.UserDistrictCode)) user.UserDistrictCode = request.UserDistrictCode.Trim();
                user.UserCityCode = null;
                if (!string.IsNullOrEmpty(request.UserCityCode)) user.UserCityCode = request.UserCityCode.Trim();

                var shops = await _DbContext.Shops.Where(i => i.UserId == request.UserId).ToListAsync();
                if (shops.Count > 0)
                {
                    foreach (var shop in shops)
                    {
                        shop.UserId = null;
                    }
                }
                if (request.ShopIds != null)
                {
                    foreach (var id in request.ShopIds)
                    {
                        var shop = await _DbContext.Shops.Where(i => i.ShopId == id).FirstOrDefaultAsync();
                        shop.UserId = request.UserId;
                    }
                }

                await _DbContext.SaveChangesAsync();

                return new ApiSuccessResponse("Cập nhật thành công");
            }
            catch (Exception error)
            {
                return new ApiFailResponse("Có lỗi xảy ra, thử lại sau: " + error.Message);
            }
        }
        public async Task<ApiResponse> UpdateUserPhoneNumber(int UserId, string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber)) return new ApiFailResponse("Số điện thoại không được để trống");

            var phonenumber = PhoneNumber;
            if (phonenumber.Contains("+84"))
            {
                phonenumber = phonenumber.Replace("+84", "");
                if (!phonenumber.StartsWith("0"))
                {
                    phonenumber = "0" + phonenumber;
                }
            }
            var user = await _DbContext.Users
                                .Where(i => i.UserId == UserId)
                                .FirstOrDefaultAsync();
            if (user != null)
            {
                user.UserPhone = phonenumber;
                await _DbContext.SaveChangesAsync();
                return new ApiSuccessResponse("Cập nhật số điện thoại thành công");
            }

            return new ApiFailResponse("Cập nhật số điện thoại không thành công");
        }
        public async Task<ApiResponse> UpdateUserPassword(UpdatePasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.CurrentPassword)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.NewPassword)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.RePassword)) return new ApiFailResponse("Thông tin không được để trống");

            var user = await _DbContext.Users
                                .Where(i => i.UserId == request.UserId && i.Password == request.CurrentPassword)
                                .FirstOrDefaultAsync();

            if (user == null) return new ApiFailResponse("Mật khẩu không chính xác");
            if (request.NewPassword != request.RePassword) return new ApiFailResponse("Mật khẩu không trùng");

            user.Password = request.RePassword;
            await _DbContext.SaveChangesAsync();

            return new ApiSuccessResponse("Cập nhật mật khẩu thành công");
        }
        public async Task<ApiResponse> UpdateUserStatus(UserUpdateRequest request)
        {
            var user = await _DbContext.Users
                                .Where(i => i.UserId == request.UserId)
                                .FirstOrDefaultAsync();
            if (user != null)
            {
                user.Status = request.Status;
                _DbContext.SaveChangesAsync().Wait();
                return new ApiSuccessResponse("Cập nhật thành công");
            }

            return new ApiFailResponse("Cập nhật thất bại");
        }
        public async Task<ApiResponse> ResetPassword(UpdatePasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber)) return new ApiFailResponse("Thông tin không được để trống");
            if (request.PhoneNumber.Contains("+84"))
            {
                request.PhoneNumber = request.PhoneNumber.Replace("+84", "");
                if (!request.PhoneNumber.StartsWith("0"))
                {
                    request.PhoneNumber = "0" + request.PhoneNumber;
                }
            }
            if (string.IsNullOrEmpty(request.NewPassword)) return new ApiFailResponse("Thông tin không được để trống");
            if (string.IsNullOrEmpty(request.RePassword)) return new ApiFailResponse("Thông tin không được để trống");
            if (request.NewPassword != request.RePassword) return new ApiFailResponse("Mật khẩu không trùng");

            var user = await _DbContext.Users
                                .Where(i => i.UserPhone == request.PhoneNumber)
                                .FirstOrDefaultAsync();
            if (user != null) {
                user.Password = request.RePassword;
                await _DbContext.SaveChangesAsync();

                return new ApiSuccessResponse("Cập nhật mật khẩu thành công");
            } 
            
            return new ApiFailResponse("Mật khẩu không chính xác");
        }
        public async Task<string> getUserRole(int userId)
        {
            var role = await _DbContext.UserRoles
                .Where(i => i.UserId == userId)
                .Select(i => i.Role.RoleName)
                .FirstOrDefaultAsync();
            return role;
        }
        public async Task<UserGetModel> getUserByShop(int shopId)
        {
            // Get user id by shop id
            var userId = await _DbContext.Shops
                .Where(s => s.ShopId == shopId)
                .Select(i => i.UserId)
                .FirstOrDefaultAsync();

            if (userId == null)
                return null;
            
            // get user id by user id
            var user = await _DbContext.Users
                .Where(u => u.UserId == userId)
                .Select(i => new UserGetModel
                {
                    UserId = i.UserId,
                    UserFullName = i.UserFullName,
                    UserJoinDate = (DateTime)i.UserJoinDate,
                    UserMail = i.UserMail,
                    UserAddress = i.UserAddress,
                    UserWardCode = i.UserWardCode,
                    UserDistrictCode = i.UserDistrictCode,
                    UserCityCode = i.UserCityCode,
                    UserPhone = i.UserPhone,
                    Status = i.Status == null ? false : (bool)i.Status,
                    IsOnline = i.IsOnline == null ? false : (bool)i.IsOnline,
                })
                .FirstOrDefaultAsync();
            return user;
        }
        public async Task<ApiResponse> DeleteUser(int id)
        {
            try
            {   
                if (id == 0) return new ApiFailResponse("Vui lòng chọn User");
                var userRole = await getUserRole(id);
                if (userRole == RoleName.Admin) return new ApiFailResponse("Không thể xóa tài khoản này");

                // User's shops
                var shops = await _DbContext.Shops.Where(i => i.UserId == id).ToListAsync();
                foreach (var shop in shops)
                {
                    shop.UserId = null;
                }

                // User's roles
                var roles = await _DbContext.UserRoles.Where(i => i.UserId == id).ToListAsync();
                _DbContext.UserRoles.RemoveRange(roles);

                // User's comment
                var rates = await _DbContext.Rates.Where(i => i.UserId == id).FirstOrDefaultAsync();
                if (rates != null)
                {
                    // Images
                    var rateImages = await _DbContext.RatingImages.Where(i => i.RateId == rates.RateId).ToListAsync();
                    if (rateImages != null)
                        _DbContext.RatingImages.RemoveRange(rateImages);

                    // Like/Dislike
                    var interests = await _DbContext.Interests.Where(i => i.UserId == id).ToListAsync();
                    if (interests != null)
                        _DbContext.Interests.RemoveRange(interests);
                   
                    _DbContext.Rates.Remove(rates);
                }

                // User
                var user = await _DbContext.Users.Where(i => i.UserId == id).FirstOrDefaultAsync();
                _DbContext.Users.Remove(user);
                _DbContext.SaveChangesAsync().Wait();

                return new ApiSuccessResponse("Đã xóa tài khoản");
            }
            catch(Exception error)
            {
                return new ApiFailResponse(error.ToString());
            }
        }
        public bool IsAdmin(int id)
        {
            try
            {
                var userRoles = _DbContext.UserRoles
                    .Where(i => i.UserId == id)
                    .Select(i => i.Role.RoleName)
                    .ToList();
                if (userRoles != null)
                    if (userRoles.Contains(RoleName.Admin))
                        return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
