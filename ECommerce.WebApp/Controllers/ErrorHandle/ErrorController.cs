using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.ErrorHandle
{
    public class ErrorController : Controller
    {
        [Route("error/{statusCode}")]
        [AllowAnonymous]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404: 
                    return View("NotFound");
                case 403: 
                    return View("Forbidden");
                default: 
                    return View("NotFound");
            }
        }
        [Route("error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
