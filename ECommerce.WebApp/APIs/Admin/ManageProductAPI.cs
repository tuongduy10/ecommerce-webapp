using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ManageProductAPI : ControllerBase
    {
        private IProductService _productService;
        private IBrandService _brandService;
        public ManageProductAPI(IProductService productService, IBrandService brandService)
        {
            _productService = productService;
            _brandService = brandService;
        }
    }
}
