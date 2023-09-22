using ECommerce.Application.BaseServices.Brand;
using ECommerce.Application.BaseServices.Product;
using ECommerce.Application.BaseServices.Product.Dtos;
using ECommerce.Application.BaseServices.SubCategory;
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
        private IProductBaseService _productService;
        private ISubCategoryService _subCategoryService;
        private IBrandService _brandService;
        private IWebHostEnvironment _webHostEnvironment;
        public SaleProductAPI(
            IWebHostEnvironment webHostEnvironment,
            IProductBaseService productService,
            ISubCategoryService subCategoryService,
            IBrandService brandService)
        {
            _webHostEnvironment = webHostEnvironment;
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
        }

    }
}
