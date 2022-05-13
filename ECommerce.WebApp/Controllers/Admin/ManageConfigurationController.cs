using ECommerce.Application.Services.Bank;
using ECommerce.Application.Services.Configurations;
using ECommerce.WebApp.Models.Configurations.Footer;
using ECommerce.WebApp.Models.Configurations.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageConfigurationController : Controller
    {
        private IFooterService _footerService;
        private IHeaderService _headerService;
        private IConfigurationService _configurationService;
        private IBankService _bankService;
        public ManageConfigurationController(IFooterService footerService, IConfigurationService configurationService, IBankService bankService, IHeaderService headerService)
        {
            _footerService = footerService;
            _configurationService = configurationService;
            _bankService = bankService;
            _headerService = headerService;
        }
        public async Task<IActionResult> ManageHeader()
        {
            var headers = await _headerService.getAll();
            var banners = await _configurationService.getBanner();
            var logo = await _configurationService.getConfiguration();
            var model = new ManageHeaderViewModel()
            {
                headers = headers,
                banners = banners,
                LogoPath = logo.LogoPath,
            };
            return View(model);
        }
        public async Task<IActionResult> ManageFooter()
        {
            var blogs = await _footerService.getAllBlog();
            var socials = await _footerService.getAllSocial();
            var configs = await _configurationService.getConfiguration();

            var model = new ManageFooterViewModel()
            {
                listBlog = blogs,
                listSocial = socials,
                config = configs
            };
            return View(model);
        }
        public async Task<IActionResult> AddBlog()
        {
            return View();
        }
        public async Task<IActionResult> BlogDetail(int BlogId)
        {
            var blog = await _footerService.getBlogDetail(BlogId);
            return View(blog);
        }
        public async Task<IActionResult> ManageBank()
        {
            var listBank = await _bankService.getListBank();
            return View(listBank);
        }

        public async Task<IActionResult> UpdateBanner(List<IFormFile> images)
        {
            return View();
        }
    } 
}
