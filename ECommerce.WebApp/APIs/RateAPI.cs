using ECommerce.Application.Services.Rate;
using ECommerce.Application.Services.Rate.Dtos;
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
        private IWebHostEnvironment _webHostEnvironment;
        public RateAPI(IRateService rateService, IWebHostEnvironment webHostEnvironment)
        {
            _rateService = rateService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("PostComment")]
        public async Task<IActionResult> PostComment([FromForm] PostCommentRequest request)
        {
            var listName = new List<string>();
            if (request.files == null)
            {
                return BadRequest("Chọn ít nhất một ảnh");
            }
            for (int i = 0; i < request.files.Count; i++)
            {
                var fileName = Guid.NewGuid().ToString() + new FileInfo(request.files[i].FileName).Extension;
                listName.Add(fileName);
            }
            request.fileNames = listName;
            request.userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);

            var result = await _rateService.postComment(request);
            if (result.isSucceed)
            {
                // save images to folder
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/rates");
                for (int i = 0; i < request.files.Count; i++)
                {
                    string filePath = Path.Combine(uploadsFolder, listName[i]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        request.files[i].CopyTo(fileStream);
                    }
                }
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
