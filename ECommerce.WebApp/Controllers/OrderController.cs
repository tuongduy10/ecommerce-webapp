using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
    }
}
