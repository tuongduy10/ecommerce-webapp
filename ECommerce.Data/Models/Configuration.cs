using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Configuration
    {
        public int? Id { get; set; }
        public string WebsiteName { get; set; }
        public string FaviconPath { get; set; }
        public string LogoPath { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public string Facebook { get; set; }
        public string FacebookUrl { get; set; }
    }
}
