using ECommerce.Application.Common;
using ECommerce.Application.BaseServices.User.Dtos;
using ECommerce.Application.Services.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ECommerce.Application.Services.User
{
    public interface IUserService
    {
        int GetCurrentUserId();
        Task<Response<List<UserGetModel>>> GetUsers(UserGetRequest request = null);
        Task<Response<UserGetModel>> GetUser(int userId);
        Task<ApiResponse> SetOnline(int userId = 0, bool isOnline = true);
        Task<Response<UserGetModel>> UpdateOnlineStatus(int _userId, bool _isOnline);
        Task<Response<UserGetModel>> UpdateOnlineHistory(int _userId);
        Task<Response<UserModel>> ValidateUser(SignInRequest request);
        Task<Response<List<ShopModel>>> GetShops();
    }
}
