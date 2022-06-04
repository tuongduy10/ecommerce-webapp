using ECommerce.Application.Services.Brand;
using ECommerce.Application.Services.Brand.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Controllers.Client
{
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public async Task<IActionResult> HighlightsBrands()
        {
            var list = await _brandService.getAll();
            return View(list);
        }
        public async Task<IActionResult> NewBrands()
        {
            var list = await _brandService.getAll();
            return View(list);
        }
        public async Task<IActionResult> AllBrands()
        {
            var list = await _brandService.getAll();
            return View(list);
        }
    }
}
