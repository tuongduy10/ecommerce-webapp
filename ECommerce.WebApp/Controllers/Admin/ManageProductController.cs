using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [Authorize(Policy = "Admin")]
    public class ManageProductController : Controller
    {
        private IProductService _productService;
        private IBrandService _brandService;
        public ManageProductController(IProductService productService, IBrandService brandService)
        {
            _productService = productService;
            _brandService = brandService;
        }
        public async Task<IActionResult> Index()
        {
            var brand = await _brandService.getAllManage();
            return View(brand);
        }
    }
}
