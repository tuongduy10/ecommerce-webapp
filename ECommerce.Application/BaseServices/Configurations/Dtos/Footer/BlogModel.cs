using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Configurations.Dtos.Footer
{
    public class BlogModel
    {
        public int BlogId { get; set; }
        public byte? BlogPosition { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public byte? Status { get; set; }
        public string? Type { get; set; }
    }
}
