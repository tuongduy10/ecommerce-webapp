using ECommerce.Application.Common;
using ECommerce.Application.Services.Account.Dtos;
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

namespace ECommerce.Application.Services.Account
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

        public async Task<bool> checkUserPhoneNumber(string PhoneNumber)
        {
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
                return false;
            }

            return true;
        }

        public async Task<ApiResponse> SignIn(SignInRequest request)
        {
            var phonenumber = request.UserPhone;
            if (phonenumber.Contains("+84"))
            {
                phonenumber = phonenumber.Replace("+84", "");
                if (!phonenumber.StartsWith("0"))
                {
                    phonenumber = "0" + phonenumber;
                }
            }

            var user = await _DbContext.Users.Where(i => i.UserPhone == phonenumber).FirstOrDefaultAsync();
            if (user == null) return new ApiFailResponse("Tài khoản không tồn tại");

            var result = await _DbContext.Users
                .Where(i => i.UserPhone == phonenumber && i.Password == request.Password)
                .FirstOrDefaultAsync();
            if (result == null) return new ApiFailResponse("Đăng nhập không thành công");

            return new ApiSuccessResponse("Đăng nhập thành công", result);

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

        public async Task<ApiResponse> SignUp(SignUpRequest request)
        {
            var fullname = request.UserFullName;
            var mail = request.UserMail;
            var phone = request.UserPhone;
            var address = request.UserAddress;
            var district = request.UserDistrictCode;
            var ward = request.UserWardCode;
            var city = request.UserCityCode;
            var password = request.Password;
            var repassword = request.RePassword;
            if (phone.Contains("+84"))
            {
                phone = phone.Replace("+84", "");
                if (!phone.StartsWith("0"))
                {
                    phone = "0" + phone;
                }
            }

            if (fullname == "") return new ApiFailResponse("Vui lòng nhập họ tên");
            if (password == "" || repassword == "") return new ApiFailResponse("Vui lòng nhập mật khẩu");
            if (password != repassword) return new ApiFailResponse("Mật khẩu không trùng");

            var checkMail = await _DbContext.Users.Where(i => i.UserMail == mail).FirstOrDefaultAsync();
            if(checkMail != null) return new ApiFailResponse("Mail đã tồn tại");

            User user = new User();
            user.UserMail = mail;
            user.UserJoinDate = DateTime.Now;
            user.UserFullName = fullname;
            user.UserPhone = phone;
            user.UserAddress = address;
            user.UserDistrictCode = district;
            user.UserCityCode = city;
            user.UserWardCode = ward;
            user.Password = repassword;
            user.Status = true;

            await _DbContext.Users.AddAsync(user);
            await _DbContext.SaveChangesAsync();
            
            return new ApiSuccessResponse("Tạo tài khoản thành công", user);
        }
    }
}
