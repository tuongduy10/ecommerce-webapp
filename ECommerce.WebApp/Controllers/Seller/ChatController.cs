using ECommerce.Application.Constants;
using ECommerce.Application.Services.Chat;
using ECommerce.Application.Services.User;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Seller
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Seller")]
    public class ChatController : Controller
    {
        private IChatService _chatService;
        private HttpContextHelper _contextHelper;

        private const string DATETIME_LABEL = ConfigConstant.DATE_FORMAT;
        public ChatController(
            IChatService chatService
        ) {
            _chatService = chatService;
            _contextHelper = new HttpContextHelper();
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ChatBox()
        {
            var userMessageList = await _chatService.GetUserList();

            ViewBag.UserMessages = userMessageList.Data;

            return View();
        }
        public async Task<IActionResult> GetMessages(int userId = 0)
        {
            var list = await _chatService.GetMessages(userId);
            return Ok(list);
        }
    }
}
