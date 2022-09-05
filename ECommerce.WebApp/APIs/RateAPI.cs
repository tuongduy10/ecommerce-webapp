using ECommerce.Application.Constants;
using ECommerce.Application.Services.Rate;
using ECommerce.Application.Services.Rate.Dtos;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RateAPI : ControllerBase
    {
        private IRateService _rateService;
        private ManageFiles _manageFiles;
        private string FILE_PATH = FilePathConstant.RATE_FILEPATH;
        private string FILE_PREFIX = FilePathConstant.RATE_FILEPREFIX;

        public RateAPI(
            IRateService rateService, 
            IWebHostEnvironment webHostEnvironment
        ) {
            _rateService = rateService;
            _manageFiles = new ManageFiles(webHostEnvironment);
        }

        [HttpPost("PostComment")]
        public async Task<IActionResult> PostComment([FromForm] PostCommentRequest request)
        {
            var listName = new List<string>();
            if (request.files != null)
            {
                request.fileNames = _manageFiles.GetFilesName(request.files, FILE_PREFIX);
            }   
            request.userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);

            var result = await _rateService.postComment(request);
            if (result.isSucceed)
            {
                // save images to folder
                if (request.files != null)
                {
                    _manageFiles.AddFiles(request.files, request.fileNames, FILE_PATH);
                }
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
