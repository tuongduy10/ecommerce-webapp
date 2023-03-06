using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Brand.Dtos
{
    public class BrandCreateRequest
    {
        public string BrandName { get; set; }
        public string BrandImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<int> CategoryIds { get; set; }
        public bool? Highlights { get; set; }
        public bool? New { get; set; }
        public string? Description { get; set; }
        public string? DescriptionTitle { get; set; }
        public IFormFile ImagePath { get; set; }
    }
}
