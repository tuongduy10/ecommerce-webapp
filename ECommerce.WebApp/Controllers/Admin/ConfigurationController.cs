using ECommerce.Application.Services.Configurations;
using ECommerce.WebApp.Models.Configurations.Footer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    public class ConfigurationController : Controller
    {
        private IFooterService _footerService;
        public ConfigurationController(IFooterService footerService)
        {
            _footerService = footerService;
        }
        public async Task<IActionResult> ManageFooter()
        {
            var blogs = await _footerService.getAllBlog();
            var social = await _footerService.getAllSocial();

            var model = new ManageFooterViewModel()
            {
                listBlog = blogs,
                listSocial = social,
            };
            return View(model);
        }
    }
}
