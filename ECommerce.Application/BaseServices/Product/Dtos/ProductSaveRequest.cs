using ECommerce.Application.Services.Inventory.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductSaveRequest : ProductModel
    {

        public List<int>? currentOptions { get; set; }
        public List<OptionModel>? newOptions { get; set; }
        private List<IFormFile>? systemImage { get; set; }
        private List<IFormFile>? userImage { get; set; }
        public ProductSaveRequest() : base()
        {

        }
    }
}
