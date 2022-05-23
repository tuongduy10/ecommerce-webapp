using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Product;
using ECommerce.Application.Services.SubCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public SaleProductAPI(
            IProductService productService, 
            ISubCategoryService subCategoryService,
            IBrandService brandService)
        {
            _productService = productService;
            _subCategoryService = subCategoryService;
            _brandService = brandService;
        }

    }
}
