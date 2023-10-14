using ECommerce.Application.Constants;
using ECommerce.Application.Services.Rate;
using ECommerce.Application.Services.User;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    //[Authorize(AuthenticationSchemes = "AdminAuth")]
    //[Authorize(Policy = "Seller")]
    public class ManageCommentController : Controller
    {
        private string RATE_FILE_PATH = FilePathConstant.RATE_FILEPATH;
        private string RATE_FILE_PREFIX = FilePathConstant.RATE_FILEPREFIX;

        private IUserService _userService;
        private IRateService _rateService;
        private ManageFiles _manageFiles;

        public ManageCommentController(
            IUserService userService,
            IRateService rateService,
            IWebHostEnvironment webHostEnvironment
        ) {
            _userService = userService;
            _rateService = rateService;
            _manageFiles = new ManageFiles(webHostEnvironment);
        }
        public async Task<IActionResult> Index()
        {
            var list = await _rateService.GetAll();
            return View(list);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var list = await _rateService.GetAllByParentId(id);
            return View(list);
        }
    }
}
