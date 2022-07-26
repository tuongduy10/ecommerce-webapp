using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Brand.Dtos
{
    public class BrandUpdateRequest
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandImagePath { get; set; }
        public bool Status { get; set; }
        //public int CategoryId { get; set; }
        public List<int> CategoryIds { get; set; }
        public bool? Highlights { get; set; }
        public bool? New { get; set; }
        public IFormFile ImagePath { get; set; }
        public bool imageChanged { get; set; }
    }
}
