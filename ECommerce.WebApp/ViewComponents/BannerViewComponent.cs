using ECommerce.Application.Services.Configurations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {
        private readonly IConfigurationService _configurationService;
        public BannerViewComponent(IConfigurationService configurationService) 
        {
            _configurationService = configurationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listBanner = _configurationService.getBanner();
            return View(listBanner);
        }
    }
}
