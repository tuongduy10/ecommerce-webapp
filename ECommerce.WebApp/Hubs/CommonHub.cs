using ECommerce.Application.Services.User;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Hubs
{
    public class CommonHub : Hub
    {
        private IUserService _userSerivceV2;
        private HttpContextHelper _contextHelper;
        private ILogger<ClientHub> _logger;
        public CommonHub(IUserService userServiceV2, ILogger<ClientHub> logger)
        {
            _contextHelper = new HttpContextHelper();
            _userSerivceV2 = userServiceV2;
            _logger = logger;
        }
        // receive from client-hub
        //public async Task SendOnlineUser(int userId)
        //{
        //    var result = await _userSerivceV2.GetUser(userId);
        //    await Clients.All.SendAsync("onUpdateUser", result);
        //}
    }
}
