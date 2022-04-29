using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Client
{
    public class AccountController : Controller
    {
        public async Task<IActionResult> SignIn()
        {
            return View();
        }
    }
}
