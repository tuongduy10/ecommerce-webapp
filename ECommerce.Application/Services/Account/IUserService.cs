using ECommerce.Application.Common;
using ECommerce.Application.Services.Account.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Account
{
    public interface IUserService
    {
        Task<ApiResponse> SignIn(SignInRequest request);
        Task<ApiResponse> SignUp(SignUpRequest request);
        Task<UserGetModel> UserProfile(int id);
        Task<ApiResponse> CheckUserPhoneNumber(string PhoneNumber);
        Task<ApiResponse> UpdateUserProfile(UserUpdateRequest request);
        Task<ApiResponse> UpdateUserPhoneNumber(int UserId, string PhoneNumber);
    }
}
