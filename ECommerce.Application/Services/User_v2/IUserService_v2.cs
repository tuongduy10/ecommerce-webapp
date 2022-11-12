using ECommerce.Application.Common;
using ECommerce.Application.Services.User.Dtos;
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
        Task<ApiResponse> SetOnline(int userId = 0, bool isOnline = true);
    }
}
