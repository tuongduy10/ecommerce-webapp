using ECommerce.Application.Common;
using ECommerce.Application.Repositories.User;
using ECommerce.Application.BaseServices.User.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ECommerce.Application.Constants;
using ECommerce.Data.Models;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.OnlineHistory;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Application.Repositories.Notification;
using ECommerce.Application.BaseServices.Shop.Dtos;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Data.Models.Common;
using ECommerce.Application.Enums;

namespace ECommerce.Application.Services.User
{
    public class UserService : IUserService
    {
        private ECommerceContext _DbContext;
        private IUserRepository _userRepo;
        private IOnlineHistoryRepository _onlineHistoryRepo;
        private INotificationRepository _notiRepo;
        private IRepositoryBase<MessageHistory> _messageRepo;
        private IRepositoryBase<Shop> _shopRepo;
        private readonly IRepositoryBase<UserRole> _userRoleRepo;
        private readonly IRepositoryBase<Province> _provinceRepo;
        private readonly IRepositoryBase<District> _districtRepo;
        private readonly IRepositoryBase<Ward> _wardRepo;
        public UserService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
            if (_userRepo == null)
                _userRepo = new UserRepository(_DbContext);
            if(_onlineHistoryRepo == null)
                _onlineHistoryRepo = new OnlineHistoryRepository(_DbContext);
            if (_notiRepo == null)
                _notiRepo = new NotificationRepository(_DbContext);
            if (_messageRepo == null)
                _messageRepo = new RepositoryBase<MessageHistory>(_DbContext);
            if (_shopRepo == null)
                _shopRepo = new RepositoryBase<Shop>(_DbContext);
            if (_provinceRepo == null)
                _provinceRepo = new RepositoryBase<Province>(_DbContext);
            if (_districtRepo == null)
                _districtRepo = new RepositoryBase<District>(_DbContext);
            if (_wardRepo == null)
                _wardRepo = new RepositoryBase<Ward>(_DbContext);
            if (_userRoleRepo == null)
                _userRoleRepo = new RepositoryBase<UserRole>(_DbContext);
        }
        public int GetCurrentUserId()
        {
            return 0;
        }
        public IUserRepository User { get => _userRepo; }
        public async Task<Response<PageResult<UserGetModel>>> getUserPagingList(UserGetRequest request)
        {
            try
            {
                var query = _userRepo
                    .Query()
                    .Where(x => request.userId == -1 || x.UserId == request.userId)
                    .Select(i => (UserGetModel)i);

                var record = await query.CountAsync();
                var data = await PaginatedList<UserGetModel>.CreateAsync(query, request.PageIndex, request.PageSize);
                var result = new PageResult<UserGetModel>()
                {
                    Items = data,
                    CurrentRecord = (request.PageIndex * request.PageSize) <= record ? (request.PageIndex * request.PageSize) : record,
                    TotalRecord = record,
                    CurrentPage = request.PageIndex,
                    TotalPage = (int)Math.Ceiling(record / (double)request.PageSize)
                };
                return new SuccessResponse<PageResult<UserGetModel>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<PageResult<UserGetModel>>(error.Message);
            }
        }
        public async Task<Response<List<UserGetModel>>> GetUsers(UserGetRequest request = null)
        {
            try
            {
                var list = await _userRepo
                    .Query()
                    .Select(i => new UserGetModel()
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
                        isSystemAccount = i.IsSystemAccount == null ? false : (bool)i.IsSystemAccount,
                        Status = i.Status == null ? false : (bool)i.Status,
                        IsOnline = i.IsOnline == null ? false : (bool)i.IsOnline,
                        LastOnline = (DateTime)i.LastOnline
                    })
                    .ToListAsync();

                if (request != null)
                {
                    if (request.all) list = list.ToList();
                    if (request.userId != 0) list = list.Where(i => i.UserId == request.userId).ToList();
                    if (request.isSystemAccount) list = list.Where(i => i.isSystemAccount == true).ToList();
                    if (request.isOnline) list = list.Where(i => i.IsOnline == true).ToList();
                    if (request.isOffline) list = list.Where(i => i.IsOnline == false).ToList();
                }

                var result = list.OrderByDescending(i => i.UserJoinDate).ToList();
                return new SuccessResponse<List<UserGetModel>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<List<UserGetModel>>("Lỗi: \n\n" + error.ToString());
            }
        }
        public async Task<Response<UserGetModel>> GetUser(int userId)
        {
            try
            {
                var user = await _userRepo.GetAsyncWhere(i => i.UserId == userId, "Shops");
                if (user == null) 
                    return new FailResponse<UserGetModel>("Không tìm thấy người dùng");

                var result = (UserGetModel)user;
                return new SuccessResponse<UserGetModel>("", result);
            }
            catch (Exception error)
            {
                return new FailResponse<UserGetModel>(error.ToString());
            }
        }
        public async Task<ApiResponse> SetOnline(int _userId = 0, bool _isOnline = true)
        {
            try
            {
                if(_userId != 0)
                {
                    var user = await _userRepo.GetAsyncWhere(item => item.UserId == _userId);
                    if (user != null)
                    {
                        user.IsOnline = _isOnline;
                        user.LastOnline = DateTime.Now;
                        _userRepo.Update(user);
                        await _userRepo.SaveChangesAsync();
                        return new ApiSuccessResponse("");
                    }
                }
                return new ApiFailResponse("");
            }
            catch (Exception error)
            {
                return new ApiFailResponse(error.ToString());
            }
        }
        public async Task<Response<UserGetModel>> UpdateOnlineStatus(int _userId, bool _isOnline)
        {
            try
            {
                if (_userId == 0) return new FailResponse<UserGetModel>("");

                var user = await _userRepo.GetAsyncWhere(item => item.UserId == _userId);
                if(user == null) return new FailResponse<UserGetModel>("Không tìm thấy người dùng");

                user.IsOnline = _isOnline;
                if(_isOnline == false) user.LastOnline = DateTime.Now;
                _userRepo.Update(user);
                await _userRepo.SaveChangesAsync();

                var ressult = new UserGetModel()
                {
                    UserId = user.UserId,
                    IsOnline = user.IsOnline != null ? (bool)user.IsOnline : false,
                    LastOnlineLabel = ((DateTime)user.LastOnline).ToString(ConfigConstant.DATE_FORMAT) 
                };
                return new SuccessResponse<UserGetModel>("", ressult);
            }
            catch (Exception error)
            {
                return new FailResponse<UserGetModel>(error.ToString());
            }
        }
        public async Task<Response<UserGetModel>> UpdateOnlineHistory(int _userId)
        {
            try
            {
                if (_userId == 0) return new FailResponse<UserGetModel>("");

                var user = await _userRepo.GetAsyncWhere(item => item.UserId == _userId);
                if (user == null) return new FailResponse<UserGetModel>("Không tìm thấy người dùng");

                var history = await _onlineHistoryRepo.GetAsyncWhere(i => i.UserId == _userId && i.Duration == 0);
                if (history == null)
                {
                    // create new logic here
                    var newHistory = new OnlineHistory()
                    {
                        AccessDate = DateTime.Now,
                        UserId = _userId,
                        Duration = 0
                    };
                    await _onlineHistoryRepo.AddAsync(newHistory);
                    await _onlineHistoryRepo.SaveChangesAsync();
                }
                else
                {
                    var totalMin = (DateTime.Now - (DateTime)user.LastOnline).TotalMinutes;
                    // update when over 3 minutes
                    if ((DateTime.Now - (DateTime)user.LastOnline).TotalSeconds >= 3)
                    {
                        int duration = Int32.Parse((DateTime.Now - (DateTime)history.AccessDate).TotalSeconds.ToString());
                        history.Duration = duration;
                        await _onlineHistoryRepo.SaveChangesAsync();
                    }
                }

                return new SuccessResponse<UserGetModel>("");
            }
            catch (Exception error)
            {
                return new FailResponse<UserGetModel>(error.ToString());
            }
        }
        public async Task<Response<UserModel>> ValidateUser(SignInRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserPhone) || string.IsNullOrEmpty(request.Password)) 
                    return new FailResponse<UserModel>("Thông tin không được để trống");

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

                if (result == null) 
                    return new FailResponse<UserModel>("Mật khẩu hoặc tài khoản không đúng");
                if (result.Status == false) 
                    return new FailResponse<UserModel>("Tài khoản đã bị khóa");

                var roles = (
                    from role in _DbContext.Roles
                    from userrole in _DbContext.UserRoles
                    where role.RoleId == userrole.RoleId && userrole.UserId == result.UserId
                    select role.RoleName
                ).Distinct().ToList();

                var user = new UserModel();
                user.id = result.UserId;
                user.fullName = result.UserFullName;
                user.roles = roles;
                user.phone = result.UserPhone;

                return new SuccessResponse<UserModel>("Đăng nhập thành công", user);
            }
            catch(Exception error)
            {
                return new FailResponse<UserModel>("Đang xảy ra lỗi, vui lòng thử lại sau");
            }
        }
        public async Task<Response<List<ShopModel>>> GetShops()
        {
            try
            {
                var list = await _shopRepo
                    .Entity()
                    .Where(i => i.Status != 4 && i.Status != 2)
                    .Select(i => new ShopModel()
                    {
                        id = i.ShopId,
                        name = i.ShopName,
                        phoneNumber = i.ShopPhoneNumber,
                        mail = i.ShopMail,
                        address = i.ShopAddress,
                        cityCode = i.ShopCityCode,
                        districtCode = i.ShopDistrictCode,
                        joinDate = (DateTime)i.ShopJoinDate,
                        wardCode = i.ShopWardCode,
                        status = (byte)i.Status,
                        user = i.User != null ? new UserModel
                        {
                            id = i.User.UserId,
                            fullName = i.User.UserFullName
                        } : null,
                    })
                    .ToListAsync();
                var result = list.OrderByDescending(i => i.id).ToList();
                return new SuccessResponse<List<ShopModel>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<List<ShopModel>>(error.Message);
            }
        }
        public async Task<Response<UserShopModel>> SaveSeller(UserShopModel request)
        {
            try
            {
                string phonenumber = null;
                // Phone validate
                if (!string.IsNullOrEmpty(request.phone))
                {
                    phonenumber = request.phone.Trim();
                    if (phonenumber.Contains("+84"))
                    {
                        phonenumber = phonenumber.Replace("+84", "");
                        if (!phonenumber.StartsWith("0"))
                            phonenumber = "0" + phonenumber;
                    }
                }

                if (string.IsNullOrEmpty(request.fullName))
                    return new FailResponse<UserShopModel>("Vui lòng nhập họ tên");
                if ((string.IsNullOrEmpty(request.password) || string.IsNullOrEmpty(request.rePassword)) && request.id == -1)
                    return new FailResponse<UserShopModel>("Vui lòng nhập mật khẩu");
                if (request.password != request.rePassword)
                    return new FailResponse<UserShopModel>("Mật khẩu không trùng");

                var hasSeller = await _userRepo.AnyAsyncWhere(_ => 
                    _.UserPhone == phonenumber ||
                    _.UserMail == request.email.Trim());
                if (hasSeller && request.id == -1)
                    return new FailResponse<UserShopModel>("Số điện thoại hoặc Email đã tồn tại");

                var seller = await _userRepo.GetAsyncWhere(_ => _.UserId == request.id);
                if (seller == null)
                    seller = new Data.Models.User();

                seller.UserMail = request.email.Trim();
                seller.UserCityCode = request.cityCode;
                seller.UserCityName = (await _provinceRepo.GetAsyncWhere(_ => _.Code == request.cityCode))?.Name;
                seller.UserDistrictCode = request.districtCode;
                seller.UserDistrictName = (await _districtRepo.GetAsyncWhere(_ => _.Code == request.districtCode))?.Name;
                seller.UserWardCode = request.wardCode;
                seller.UserWardName = (await _wardRepo.GetAsyncWhere(_ => _.Code == request.wardCode))?.Name;
                seller.UserFullName = request.fullName.Trim();
                seller.UserAddress = request.address.Trim();
                seller.UserPhone = phonenumber;
                seller.Password = request.password.Trim();
                seller.IsOnline = false;
                seller.LastOnline = DateTime.Now;
                seller.IsSystemAccount = true;

                if (request.id == -1) // Add
                    _userRepo.AddAsync(seller).Wait();
                if (request.id > -1) // Update
                    _userRepo.Update(seller);
                _userRepo.SaveChangesAsync().Wait();

                /** Role
                 * Remove all user roles then
                 * add new role from user id in request
                 **/
                var userRoles = await _userRoleRepo.ToListAsyncWhere(_ => _.UserId == seller.UserId);
                if (userRoles.Count() > 0)
                {
                    _userRoleRepo.RemoveRange(userRoles);
                    _userRoleRepo.SaveChangesAsync().Wait();
                }
                var newRole = new UserRole
                {
                    RoleId = (int)RoleEnum.Seller,
                    UserId = seller.UserId,
                };
                await _userRoleRepo.AddAsync(newRole);
                await _userRoleRepo.SaveChangesAsync();

                /**
                 * Shop
                 * Set null all shops has user id then 
                 * update new user id from request
                 **/
                var shopsByUser = await _shopRepo.ToListAsyncWhere(_ => _.UserId == seller.UserId);
                if (shopsByUser.Count() > 0)
                {
                    foreach (var shop in shopsByUser)
                    {
                        shop.UserId = null;
                    }
                    _shopRepo.UpdateRange(shopsByUser);
                    _shopRepo.SaveChangesAsync().Wait();
                }
                if (request.shopIds.Count > 0)
                {
                    var shops = await _shopRepo.ToListAsyncWhere(_ => request.shopIds.Contains(_.ShopId));
                    if (shops.Count() > 0)
                    {
                        foreach (var shop in shops)
                        {
                            shop.UserId = seller.UserId;
                        }
                        _shopRepo.UpdateRange(shops);
                        _shopRepo.SaveChangesAsync().Wait();
                    }
                }

                return new SuccessResponse<UserShopModel>();
            }
            catch (Exception exc)
            {
                return new FailResponse<UserShopModel>(exc.Message);
            }
        }
    }
}
