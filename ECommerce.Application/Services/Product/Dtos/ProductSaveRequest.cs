using ECommerce.Application.Services.Inventory.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductSaveRequest : ProductModel
    {
        public List<string> systemFileNames { get; set; }
        public List<string> userFileNames { get; set; }
        public ProductSaveRequest() : base()
        {

        }
    }
}
