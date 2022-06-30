using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    public class ManageSizeGuide : Controller
    {
        [Authorize(AuthenticationSchemes = "AdminAuth")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
