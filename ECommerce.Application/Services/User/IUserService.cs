using ECommerce.Application.Common;
using ECommerce.Application.Services.User.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.User
{
    public interface IUserService
    {
        Task<List<UserGetModel>> getAll();
        Task<List<UserGetModel>> getUsersByFiltered(UserGetRequest request);
        Task<ApiResponse> SignIn(SignInRequest request);
        Task<ApiResponse> SignUp(SignUpRequest request);
        Task<ApiResponse> AddSeller(SignUpRequest request);
        Task<UserGetModel> UserProfile(int id);
        Task<ApiResponse> UpdateManageUserProfile(UserUpdateRequest request);
        Task<ApiResponse> CheckUserPhoneNumber(string PhoneNumber);
        Task<ApiResponse> UpdateUserProfile(UserUpdateRequest request);
        Task<ApiResponse> UpdateUserPhoneNumber(int UserId, string PhoneNumber);
        Task<ApiResponse> UpdateUserPassword(UpdatePasswordRequest request);
        Task<ApiResponse> UpdateUserStatus(UserUpdateRequest request);
        Task<ApiResponse> ResetPassword(UpdatePasswordRequest request);
        Task<string> getUserRole(int userId);
        Task<UserGetModel> getUserByShop(int shopId);
        Task<ApiResponse> DeleteUser(int id);
        bool IsAdmin(int id);
    }
}
