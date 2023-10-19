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
        public List<IFormFile>? systemImage { get; set; }
        public List<string> systemFileName {get;set;}
        public List<IFormFile>? userImage { get; set; }
        public List<string> userFileName { get; set; }
        public ProductSaveRequest() : base()
        {

        }
    }
}
