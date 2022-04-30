using ECommerce.Application.Common;
using ECommerce.Application.Services.User.Dtos;
using ECommerce.Data.Models;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly ECommerceContext _DbContext;

        public UserService(IConfiguration config, ECommerceContext DbContext)
        {
            _config = config;
            _DbContext = DbContext;
        }
        public async Task<ApiResponse> SignIn(SignInRequest request)
        {
            var user = await _DbContext.Users.Where(i => i.UserPhone == request.UserPhone).FirstAsync();
            if (user == null) return new ApiFailResponse("Tài khoản không tồn tại");

            var result = await _DbContext.Users
                .Where(i => i.UserPhone == request.UserPhone && i.Password == request.Password)
                .FirstOrDefaultAsync();
            if (result == null) return new ApiFailResponse("Đăng nhập không thành công");

            return new ApiSucceedResponse("Đăng nhập thành công", result);

            // Jwt Security
            //var jwtTokenHandler = new JwtSecurityTokenHandler();
            //var secretKey = Encoding.UTF8.GetBytes(_config["SecretKey:Key"]);
            //var tokenDescription = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[] {
            //        new Claim(ClaimTypes.Name, result.UserFullName),
            //        new Claim("UserId", result.UserId.ToString()),
            //        new Claim("UserPhone", result.UserPhone),
            //        new Claim("TokenId", Guid.NewGuid().ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddSeconds(10),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha512Signature)
            //};
            //var token = jwtTokenHandler.CreateToken(tokenDescription);

            //return new ApiSuccessResult<string>("Đăng nhập thành công", jwtTokenHandler.WriteToken(token));
        }
    }
}
