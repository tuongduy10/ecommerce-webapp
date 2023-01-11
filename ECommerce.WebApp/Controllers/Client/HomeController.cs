using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Category;
using ECommerce.Application.Services.Chat;
using ECommerce.Application.Services.Chat.Dtos;
using ECommerce.Application.Services.Configurations;
using ECommerce.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IFooterService _footerService;
        private readonly IChatService _chatService;

        public HomeController(
            ILogger<HomeController> logger, 
            ICategoryService categoryService, 
            IBrandService brandService, 
            IFooterService footerService,
            IChatService chatService
        ) {
            _logger = logger;
            _categoryService = categoryService;
            _brandService = brandService;
            _footerService = footerService;
            _chatService = chatService;
        }

        public async Task<IActionResult> Index()
        {
            var _listBrand = await _brandService.GetAllAvailable();
            var _listCategory = await _categoryService.getAll();

            var model = new HomeViewModel()
            {
                listBrand = _listBrand,
                listCategory = _listCategory
            };

            ViewBag.hasHighlightItems = _listBrand.Any(item => item.Highlights == true);
            ViewBag.highlightItemsCount = _listBrand.Where(item => item.Highlights == true).Count();

            return View(model);
        }
        public async Task<IActionResult> Payment()
        {
            var result = await _footerService.getBlogDetail(7);
            return View(result);
        }
        [AllowAnonymous]
        public async Task<IActionResult> SendUnAuthMessage(MessageModel request)
        {
            var result = await _chatService.SendUnAuthMessage(request);
            if (result.isSucceed) return Ok();
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> BlogDetail(int BlogId)
        {
            var result = await _footerService.getBlogDetail(BlogId);
            return View(result);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
