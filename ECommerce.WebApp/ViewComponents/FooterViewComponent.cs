using ECommerce.Application.Services.Configurations;
using ECommerce.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly IFooterService _footerService;
        public FooterViewComponent(IFooterService footerService) 
        {
            _footerService = footerService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var _listBlog = await _footerService.getAllBlog();
            var _listSocial = await _footerService.getAllSocial();

            var model = new FooterViewModel()
            {
                listBlog = _listBlog,
                listSocial = _listSocial
            };

            return View(model);
        }
    }
}
