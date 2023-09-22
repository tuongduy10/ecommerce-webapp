using ECommerce.Application.Constants;
using ECommerce.Application.BaseServices.Bank;
using ECommerce.Application.BaseServices.Configurations;
using ECommerce.Application.BaseServices.Configurations.Dtos.Footer;
using ECommerce.Application.BaseServices.Configurations.Dtos.Header;
using ECommerce.Data.Models;
using ECommerce.WebApp.Models.Configurations.Footer;
using ECommerce.WebApp.Models.Configurations.Header;
using ECommerce.WebApp.Utils;
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
    //[Authorize(AuthenticationSchemes = "AdminAuth")]
    //[Authorize(Policy = "Admin")]
    public class ManageConfigurationController : Controller
    {
        private IFooterService _footerService;
        private IHeaderService _headerService;
        private IConfigurationService _configurationService;
        private IBankService _bankService;
        private ManageFiles _manageFiles;
        private const string BANNER_FILEPATH = FilePathConstant.BANNER_FILEPATH;
        private const string BANNER_FILEPREFIX = FilePathConstant.BANNER_FILEPREFIX;
        private const string LOGO_FILEPATH = FilePathConstant.LOGO_FILEPATH;
        private const string LOGO_FILEPREFIX = FilePathConstant.LOGO_FILEPREFIX;
        private const string FAVICON_FILEPATH = FilePathConstant.FAVICON_FILEPATH;
        private const string FAVICON_FILEPREFIX = FilePathConstant.FAVICON_FILEPREFIX;
        public ManageConfigurationController(
            IFooterService footerService, 
            IConfigurationService configurationService, 
            IBankService bankService, 
            IHeaderService headerService,
            IWebHostEnvironment webHostEnvironment
        ) {
            _footerService = footerService;
            _configurationService = configurationService;
            _bankService = bankService;
            _headerService = headerService;
            _manageFiles = new ManageFiles(webHostEnvironment);
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
        [HttpPost]
        public async Task<IActionResult> AddBlog(BlogModel request)
        {
            var result = await _footerService.AddBlog(request);
            if (!result.isSucceed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        public async Task<IActionResult> UpdateBlog(BlogUpdateRequest request)
        {
            var result = await _footerService.UpdateBlog(request);
            if (!result.isSucceed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        public async Task<IActionResult> DeleteBlog(int BlogId)
        {
            var result = await _footerService.DeleteBlog(BlogId);
            if (!result.isSucceed)
            {
                return BadRequest(result);
            }
            return Ok(result);
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
            string fileName = _manageFiles.GetFileName(image, LOGO_FILEPREFIX);
            var result = await _headerService.updateLogo(fileName);
            if (result.isSucceed)
            {
                _manageFiles.DeleteAllFiles(LOGO_FILEPATH);
                _manageFiles.AddFile(image, fileName, LOGO_FILEPATH);
            }

            return RedirectToAction("ManageHeader", "ManageConfiguration");
        }
        public async Task<IActionResult> UpdateFavicon(IFormFile image)
        {
            if (image == null)
            {
                ViewBag.Message = "Vui lòng chọn thêm ảnh";
                return RedirectToAction("ManageHeader", "ManageConfiguration");
            }
            string fileName = _manageFiles.GetFileName(image, FAVICON_FILEPREFIX);
            var result = await _headerService.updateFavicon(fileName);
            if (result.isSucceed)
            {
                _manageFiles.DeleteAllFiles(FAVICON_FILEPATH);
                _manageFiles.AddFile(image, fileName, FAVICON_FILEPATH);
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

            var listName = _manageFiles.GetFilesName(images, BANNER_FILEPREFIX);
            var result = await _headerService.addBanner(listName);
            if (result.isSucceed)
            {
                // Save images to folder
                _manageFiles.AddFiles(images, listName, BANNER_FILEPATH);
            }
            ViewBag.Message = result.Message;
            return RedirectToAction("ManageHeader", "ManageConfiguration");
        }
        public async Task<IActionResult> UpdateHeaderMenu(HeaderUpdateRequest request)
        {
            var result = await _headerService.updateHeaderMenu(request);
            if (result.isSucceed)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    } 
}
