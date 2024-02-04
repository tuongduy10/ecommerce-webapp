using ECommerce.Application.Common;
using ECommerce.Application.Constants;
using ECommerce.Application.Services.Comment;
using ECommerce.Application.Services.Common;
using ECommerce.Application.Services.Common.DTOs.Requests;
using ECommerce.Application.Services.Product;
using ECommerce.WebApp.Dtos.Common;
using ECommerce.WebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/common")]
    public class CommonController : ControllerBase
    {
        private ManageFiles _manageFiles;
        private string BRAND_FILE_PATH = FilePathConstant.BRAND_FILEPATH;
        private string BRAND_FILE_PREFIX = FilePathConstant.BRAND_FILEPREFIX;
        private string RATING_FILE_PATH = FilePathConstant.RATE_FILEPATH;
        private string RATING_FILE_PREFIX = FilePathConstant.RATE_FILEPREFIX;
        private string PRODUCT_FILE_PATH = FilePathConstant.PRODUCT_FILEPATH;
        private string PRODUCT_FILE_PREFIX = FilePathConstant.PRODUCT_FILEPREFIX;
        private IProductService _productService;
        private ICommonService _commonService;
        public CommonController(IWebHostEnvironment webHostEnvironment,
            IProductService productService,
            ICommonService commonService)
        {
            _manageFiles = new ManageFiles(webHostEnvironment);
            _productService = productService;
            _commonService = commonService;
        }
        [AllowAnonymous]
        [HttpPost("upload")]
        public IActionResult upload([FromForm]UploadRequest request)
        {
            try
            {
                if (request.files == null)
                {
                    return BadRequest(new FailResponse<string>("Vui lòng chọn ảnh"));
                }
                string prefix = "";
                string path = "";
                switch (request.uploadType)
                {
                    case UploadTypeConstant.PRODUCT:
                    {
                        prefix = PRODUCT_FILE_PREFIX;
                        path = PRODUCT_FILE_PATH;
                        break;
                    }
                    case UploadTypeConstant.BRAND:
                    {
                        prefix = BRAND_FILE_PREFIX;
                        path = BRAND_FILE_PATH;
                        break;
                    }
                    case UploadTypeConstant.RATING:
                    {
                        prefix = RATING_FILE_PREFIX;
                        path = RATING_FILE_PATH;
                        break;
                    }
                }
                if (!String.IsNullOrEmpty(path))
                {
                    var fileNames = _manageFiles.GetFilesName(request.files, prefix);
                    _manageFiles.AddFiles(request.files, fileNames, path);
                    return Ok(new SuccessResponse<List<string>>(fileNames));
                }
                return BadRequest(new FailResponse<string>("Lưu không thành công"));
            }
            catch (Exception error)
            {
                return BadRequest(new FailResponse<string>(error.Message));
            }
        }
        [AllowAnonymous]
        [HttpPost("remove-files")]
        public async Task<IActionResult> removeFiles(RemoveUploadRequest request)
        {
            try
            {
                string path = "";
                switch (request.uploadType)
                {
                    case UploadTypeConstant.PRODUCT:
                    {
                        path = PRODUCT_FILE_PATH;
                        await _productService.removeUserImages(request.fileNames);
                        await _productService.removeSystemImages(request.fileNames);
                            break;
                    }
                    case UploadTypeConstant.BRAND:
                    {
                        path = BRAND_FILE_PATH;
                        break;
                    }
                    case UploadTypeConstant.RATING:
                    {
                        path = RATING_FILE_PATH;
                        break;
                    }
                }
                if (!String.IsNullOrEmpty(path) && request.fileNames.Count > 0)
                {
                    _manageFiles.DeleteFiles(request.fileNames, path);
                    return Ok(new SuccessResponse<string>());
                }
                return BadRequest(new FailResponse<string>("Xóa không thành công"));
            }
            catch (Exception error)
            {
                return BadRequest(new FailResponse<string>(error.Message));
            }
        }
        [AllowAnonymous]
        [HttpPost("remove-product-files")]
        public async Task<IActionResult> removeProductFiles(List<int?> ids)
        {
            try
            {
                string path = PRODUCT_FILE_PATH;
                var result = await _productService.removeSystemImages(ids);
                if (!String.IsNullOrEmpty(path) && result.Data.Count > 0)
                {
                    _manageFiles.DeleteFiles(result.Data, path);
                    return Ok(new SuccessResponse<string>());
                }
                return BadRequest(new FailResponse<string>("Xóa không thành công"));
            }
            catch (Exception error)
            {
                return BadRequest(new FailResponse<string>(error.Message));
            }
        }
        [AllowAnonymous]
        [HttpPost("remove-user-product-files")]
        public async Task<IActionResult> removeUserProductFiles(List<int?> ids)
        {
            try
            {
                string path = PRODUCT_FILE_PATH;
                var result = await _productService.removeUserImages(ids);
                if (!String.IsNullOrEmpty(path) && result.Data.Count > 0)
                {
                    _manageFiles.DeleteFiles(result.Data, path);
                    return Ok(new SuccessResponse<string>());
                }
                return BadRequest(new FailResponse<string>("Xóa không thành công"));
            }
            catch (Exception error)
            {
                return BadRequest(new FailResponse<string>(error.Message));
            }
        }
        [AllowAnonymous]
        [HttpGet("provinces")]
        public async Task<IActionResult> getProvinces()
        {
            var result = await _commonService.getProvinces();
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("districts")]
        public async Task<IActionResult> getDistricts(DistrictGetRequest request)
        {
            var result = await _commonService.getDistricts(request);
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("wards")]
        public async Task<IActionResult> getWards(WardGetRequest request)
        {
            var result = await _commonService.getWards(request);
            if (!result.isSucceed)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
