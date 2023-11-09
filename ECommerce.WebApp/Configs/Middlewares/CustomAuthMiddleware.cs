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
            if (context.Response.StatusCode == 401)
            {
                _logger.LogWarning("Unauthorized request detected.");

                // Customize the response here, e.g., return a JSON error message
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
                return;
            }

            await _next(context);
        }
    }
}
