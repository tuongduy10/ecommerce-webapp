using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Client
{
    public class CartController : Controller
    {
        public CartController()
        {

        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
