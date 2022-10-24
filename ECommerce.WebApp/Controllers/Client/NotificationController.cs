using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Application.Services.Notification;

namespace ECommerce.WebApp.Controllers.Client
{
    [Authorize(AuthenticationSchemes = "ClientAuth")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationController(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Read(int id)
        {
            var result = await _notificationService.ReadAsync(id);
            if (result.isSucceed)
                return Ok(result);
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _notificationService.DeleteAsync(id);
            if (result.isSucceed)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }
    }
}
