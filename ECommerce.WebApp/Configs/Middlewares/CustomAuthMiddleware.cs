using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.BaseServices.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ECommerce.Application.Common;

namespace ECommerce.WebApp.Middlewares
{
    public class CustomAuthMiddleware
    {
        public readonly RequestDelegate _next;
        private readonly ILogger<CustomAuthMiddleware> _logger;
        public CustomAuthMiddleware(RequestDelegate next, ILogger<CustomAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 401)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 401;

                // Customize the response format
                var response = new FailResponse<bool>();

                await context.Response.WriteAsync("{\"status\": \"fail\", \"message\": \"Unauthorized\", \"data\": null }");
            }
        }
    }
}
