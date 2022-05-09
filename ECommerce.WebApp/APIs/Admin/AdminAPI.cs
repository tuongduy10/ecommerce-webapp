using ECommerce.Application.Services.User;
using ECommerce.Application.Services.User.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAPI : ControllerBase
    {
        private IUserService _userService;
        public AdminAPI(IUserService userService)
        {
            _userService = userService;
        }
    }
}
