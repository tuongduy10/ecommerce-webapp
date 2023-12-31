using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class BrandModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imagePath { get; set; }
        public DateTime createdDate { get; set; }
        public bool isActive { get; set; }
        public bool? isHighlights { get; set; }
        public bool? isNew { get; set; }
        public string? description { get; set; }
        public string? descriptionTitle { get; set; }
        public List<string> categoryNames { get; set; }
    }
}
