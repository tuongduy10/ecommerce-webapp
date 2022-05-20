﻿
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.Rate.Dtos;
using System.Collections.Generic;

namespace ECommerce.WebApp.Models.Products
{
    public class ProductDetailViewModel
    {
        public string phone { get; set; }
        public ProductDetailModel product { get; set; }
        public List<RateGetModel> rates { get; set; }
        public List<ProductModel> suggestion { get; set; }
        public List<Option> options { get; set; }
    }
}