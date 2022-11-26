using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApp.Controllers.Others
{
    [AllowAnonymous]
    public class ElementsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
