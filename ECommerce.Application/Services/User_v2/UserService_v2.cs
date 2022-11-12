using ECommerce.Application.Common;
using ECommerce.Application.Repositories.User;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Services.User_v2
{
    public class UserService_v2 : IUserService_v2
    {
        private ECommerceContext _DbContext;
        private IUserRepository _userRepo;
        
        public UserService_v2(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
            if (_userRepo == null)
                _userRepo = new UserRepository(_DbContext);
        }
        public int GetCurrentUserId()
        {
            return 0;
        }
        public IUserRepository User { get => _userRepo; }
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
                    })
                    .ToListAsync();

                if (request != null)
                {
                    if (request.all) list = list.ToList();
                    if (request.isSystemAccount) list = list.Where(i => i.isSystemAccount == true).ToList();
                    if (request.isOnline) list = list.Where(i => i.IsOnline == true).ToList();
                }

                var result = list.OrderByDescending(i => i.UserJoinDate).ToList();
                return new SuccessResponse<List<UserGetModel>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<List<UserGetModel>>("Lỗi: \n\n" + error.ToString());
            }
        }
        public async Task<ApiResponse> SetOnline(int _userId = 0, bool _isOnline = true)
        {
            try
            {
                if(_userId != 0)
                {
                    var user = await _userRepo.FindAsyncWhere(item => item.UserId == _userId);
                    if (user != null)
                    {
                        user.IsOnline = _isOnline;
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
    }
}
