using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageUserController : Controller
    {
        public ManageUserController()
        {

        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
