using ECommerce.Application.Common;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Application.Services.User_v2.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.User_v2
{
    public interface IUserService_v2
    {
        int GetCurrentUserId();
        Task<Response<List<UserGetModel>>> GetUsers(UserGetRequest request = null);
        Task<Response<UserGetModel>> GetUser(int userId);
        Task<ApiResponse> SetOnline(int userId = 0, bool isOnline = true);
        Task<Response<UserGetModel>> UpdateOnlineStatus(int _userId, bool _isOnline);
        Task<Response<UserGetModel>> UpdateOnlineHistory(int _userId);
    }
}
