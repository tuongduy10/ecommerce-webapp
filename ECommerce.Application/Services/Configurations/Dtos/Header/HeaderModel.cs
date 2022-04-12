using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations.Dtos.Header
{
    public class HeaderModel
    {
        public int HeaderId { get; set; }
        public byte? HeaderPosition { get; set; }
        public string HeaderName { get; set; }
        public string HeaderUrl { get; set; }
        public byte? Status { get; set; }
    }
}
