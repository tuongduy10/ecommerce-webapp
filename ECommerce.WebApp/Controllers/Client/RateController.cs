using ECommerce.Application.Constants;
using ECommerce.Application.Repositories.Notification.Dtos;
using ECommerce.Application.Services.Comment;
using ECommerce.Application.Services.Comment.Request;
using ECommerce.Application.Services.Notification;
using ECommerce.Application.BaseServices.Rate;
using ECommerce.Application.BaseServices.Rate.Dtos;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Client
{
    [Authorize(Policy = "Buyer")]
    [Authorize(AuthenticationSchemes = "ClientAuth")]
    public class RateController : Controller
    {
        private IRateService _rateService;
        private ICommentService _commentService;
        private ManageFiles _manageFiles;
        private string FILE_PATH = FilePathConstant.RATE_FILEPATH;
        private string FILE_PREFIX = FilePathConstant.RATE_FILEPREFIX;
        
        public RateController(
            IRateService rateService,
            ICommentService comment,
            IWebHostEnvironment webHostEnvironment
        ) {
            _rateService = rateService;
            _commentService = comment;
            _manageFiles = new ManageFiles(webHostEnvironment);
        }
        public async Task<IActionResult> PostComment(PostCommentBaseRequest request)
        {
            if (request.files != null)
                request.fileNames = _manageFiles.GetFilesName(request.files, FILE_PREFIX);
            request.userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);

            var result = await _rateService.PostComment(request);
            if (result.isSucceed)
            {
                // save images to folder
                if (request.files != null)
                    _manageFiles.AddFiles(request.files, request.fileNames, FILE_PATH);

                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        public async Task<IActionResult> ReplyCommentBakup(ReplyCommentRequest request)
        {
            if (request.files != null)
                request.fileNames = _manageFiles.GetFilesName(request.files, FILE_PREFIX);
            request.userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);

            var result = await _rateService.ReplyComment(request);
            if (result.isSucceed)
            {
                // save images to folder
                if (request.files != null)
                    _manageFiles.AddFiles(request.files, request.fileNames, FILE_PATH);
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> ReplyComment(ReplyCommentRequest request)
        {
            if (request.files != null)
                request.fileNames = _manageFiles.GetFilesName(request.files, FILE_PREFIX);
            request.userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);

            var result = await _commentService.ReplyCommentAsync(request);
            if (result.isSucceed)
            {
                // save images to folder
                if (request.files != null)
                    _manageFiles.AddFiles(request.files, request.fileNames, FILE_PATH);
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> LikeCommentBakup(LikeAndDislikeCount request)
        {
            var _id = User.Claims.FirstOrDefault(i => i.Type == "UserId") != null ?
                Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value) : 0;
            request.userId = _id;
            var result = await _rateService.LikeComment(request);
            if (result.isSucceed)
                return Ok(result.Data);
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> LikeComment(LikeAndDislikeCount request)
        {
            var _id = User.Claims.FirstOrDefault(i => i.Type == "UserId") != null ?
                Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value) : 0;
            request.userId = _id;
            var result = await _commentService.LikeComment(request);
            if (result.isSucceed)
                return Ok(result.Data);
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> UpdateComment(UpdateCommentRequest request)
        {
            if (request.files != null)
                request.fileNames = _manageFiles.GetFilesName(request.files, FILE_PREFIX);

            var result = await _commentService.UpdateComment(request);
            if (result.isSucceed)
            {
                // save images to folder
                if (request.files != null)
                    _manageFiles.AddFiles(request.files, request.fileNames, FILE_PATH);

                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _rateService.DeleteComment(id);
            if (result.isSucceed)
            {
                _manageFiles.DeleteFiles(result.Data, FILE_PATH);
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        public async Task<IActionResult> DeleteImages(List<int> id) 
        {
            var result = await _rateService.DeleteImages(id);
            if (result.isSucceed)
            {
                _manageFiles.DeleteFiles(result.Data, FILE_PATH);
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetUsersFavor(UserFavorRequest request)
        {
            var result = await _commentService.GetUsersFavor(request);
            if(result.isSucceed)
                return Ok(result);
            return BadRequest(result.Message);
        }
    }
}
