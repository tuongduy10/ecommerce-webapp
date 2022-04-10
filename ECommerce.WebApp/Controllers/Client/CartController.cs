using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Client
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
