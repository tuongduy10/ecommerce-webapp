using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductSaveRequest : ProductModel
    {
        private List<IFormFile>? systemImage { get; set; }
        private List<IFormFile>? userImage { get; set; }
        public ProductSaveRequest() : base()
        {

        }
    }
}
