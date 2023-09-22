using ECommerce.Application.BaseServices.Brand;
using ECommerce.Application.BaseServices.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs.Admin
{
    //[Authorize(AuthenticationSchemes = "AdminAuth")]
    //[Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ManageProductAPI : ControllerBase
    {
        private IProductBaseService _productService;
        private IBrandService _brandService;
        public ManageProductAPI(IProductBaseService productService, IBrandService brandService)
        {
            _productService = productService;
            _brandService = brandService;
        }
    }
}
