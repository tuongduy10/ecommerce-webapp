using ECommerce.Application.Common;
using ECommerce.Application.Services.User.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.User
{
    public interface IUserService
    {
        Task<List<UserGetModel>> getAll();
        Task<ApiResponse> SignIn(SignInRequest request);
        Task<ApiResponse> SignUp(SignUpRequest request);
        Task<UserGetModel> UserProfile(int id);
        Task<ApiResponse> CheckUserPhoneNumber(string PhoneNumber);
        Task<ApiResponse> UpdateUserProfile(UserUpdateRequest request);
        Task<ApiResponse> UpdateUserPhoneNumber(int UserId, string PhoneNumber);
        Task<ApiResponse> UpdateUserPassword(UpdatePasswordRequest request);
        Task<ApiResponse> UpdateUserStatus(UserUpdateRequest request);
    }
}
