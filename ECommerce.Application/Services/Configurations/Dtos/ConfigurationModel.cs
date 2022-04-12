using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations.Dtos
{
    public class ConfigurationModel
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
    }
}
