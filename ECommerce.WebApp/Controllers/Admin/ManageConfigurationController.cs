using ECommerce.Application.Services.Bank;
using ECommerce.Application.Services.Configurations;
using ECommerce.WebApp.Models.Configurations.Footer;
using ECommerce.WebApp.Models.Configurations.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ManageConfigurationController(IFooterService footerService, 
                                            IConfigurationService configurationService, 
                                            IBankService bankService, 
                                            IHeaderService headerService,
                                            IWebHostEnvironment webHostEnvironment)
        {
            _footerService = footerService;
            _configurationService = configurationService;
            _bankService = bankService;
            _headerService = headerService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> ManageHeader()
        {
            var headers = await _headerService.getAllManage();
            var banners = await _configurationService.getBanner();
            var config = await _configurationService.getConfiguration();
            var model = new ManageHeaderViewModel()
            {
                headers = headers,
                banners = banners,
                config = config,
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
        public async Task<IActionResult> AddLogo(IFormFile image)
        {
            if (image == null)
            {
                ViewBag.Message = "Vui lòng chọn thêm ảnh";
                return RedirectToAction("ManageHeader", "ManageConfiguration");
            }
            string fileName = Guid.NewGuid().ToString() + new FileInfo(image.FileName).Extension;
            var result = await _headerService.updateLogo(fileName);
            if (result.isSucceed)
            {
                string logoFolderStorage = Path.Combine(_webHostEnvironment.WebRootPath, "images/logo");
                string faviconFolderStorage = Path.Combine(_webHostEnvironment.WebRootPath, "images/favicon");

                DirectoryInfo logoDirectory = new DirectoryInfo(logoFolderStorage);
                DirectoryInfo faviconDirectory = new DirectoryInfo(faviconFolderStorage);

                // Delete all previous logo and favicon
                foreach (FileInfo file in logoDirectory.GetFiles()) file.Delete();
                foreach (FileInfo file in faviconDirectory.GetFiles()) file.Delete();

                // Add new logo
                string logoPath = Path.Combine(logoFolderStorage, fileName);
                string faviconPath = Path.Combine(faviconFolderStorage, fileName);

                using (var fileStream = new FileStream(logoPath, FileMode.Create)) image.CopyTo(fileStream);
                using (var fileStream = new FileStream(faviconPath, FileMode.Create)) image.CopyTo(fileStream);
            }

            return RedirectToAction("ManageHeader", "ManageConfiguration");
        }
        public async Task<IActionResult> AddBanner(List<IFormFile> images)
        {
            if (images.Count == 0)
            {
                ViewBag.Message = "Vui lòng chọn thêm ảnh";
                return RedirectToAction("ManageHeader", "ManageConfiguration");
            }
            var listName = new List<string>();
            for (int i = 0; i < images.Count; i++)
            {
                listName.Add(Guid.NewGuid().ToString() + new FileInfo(images[i].FileName).Extension);
            }
            var result = await _headerService.addBanner(listName);
            if (result.isSucceed)
            {
                // Save images to folder
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/banner");
                for (int i = 0; i < images.Count; i++)
                {
                    string filePath = Path.Combine(uploadsFolder, listName[i]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        images[i].CopyTo(fileStream);
                    }
                }
            }
            ViewBag.Message = result.Message;
            return RedirectToAction("ManageHeader", "ManageConfiguration");
        }
    } 
}
