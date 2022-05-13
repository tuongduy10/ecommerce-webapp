using ECommerce.Application.Services.Configurations;
using ECommerce.WebApp.Models.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ECommerce.WebApp.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IHeaderService _headerService;
        private readonly IConfigurationService _configurationService;
        private IConfiguration _config;
        public HeaderViewComponent(IConfiguration config, IConfigurationService configurationService, IHeaderService headerService)
        {
            _config = config;
            _configurationService = configurationService;
            _headerService = headerService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var _listHeader = await _headerService.getAll();
            var _config = await _configurationService.getConfiguration();

            var model = new HeaderViewModel()
            {
                listHeader = _listHeader,
                config = _config
            };

            return View(model);
        }
    }
}
