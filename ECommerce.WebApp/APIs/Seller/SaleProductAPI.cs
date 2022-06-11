using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.SubCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs.Seller
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Seller")]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleProductAPI : ControllerBase
    {
        //AddProductViewModel
        private IProductService _productService;
        private ISubCategoryService _subCategoryService;
        private IBrandService _brandService;
        private IWebHostEnvironment _webHostEnvironment;
        public SaleProductAPI(
            IWebHostEnvironment webHostEnvironment,
            IProductService productService, 
            ISubCategoryService subCategoryService,
            IBrandService brandService)
        {
            _webHostEnvironment = webHostEnvironment;
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody]ProductAddRequest request)
        {
            // Image null check
            if (request.systemImage == null || request.userImage == null)
            {
                return BadRequest("Vui lòng chọn ảnh");
            }

            // Get files name
            var listSysFileName = new List<string>();
            for (int i = 0; i < request.systemImage.Count; i++)
            {
                request.systemFileName.Add(Guid.NewGuid().ToString() + new FileInfo(request.systemImage[i].FileName).Extension);
            }
            for (int i = 0; i < request.userImage.Count; i++)
            {
                request.userFileName.Add(Guid.NewGuid().ToString() + new FileInfo(request.userImage[i].FileName).Extension);
            }

            request.userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "UserId").Value);
            var result = await _productService.AddProduct(request);
            // Result 
            if (result.isSucceed)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/product");
                for (int i = 0; i < request.systemImage.Count; i++)
                {
                    string filePath = Path.Combine(uploadsFolder, request.systemFileName[i]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        request.systemImage[i].CopyTo(fileStream);
                    }
                }
                for (int i = 0; i < request.userImage.Count; i++)
                {
                    string filePath = Path.Combine(uploadsFolder, request.userFileName[i]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        request.userImage[i].CopyTo(fileStream);
                    }
                }

                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
