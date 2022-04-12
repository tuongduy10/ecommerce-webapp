using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations.Dtos.Footer
{
    public class SocialModel
    {
        public int SocialId { get; set; }
        public byte? Position { get; set; }
        public string Icon { get; set; }
        public string SocialName { get; set; }
        public string SocialUrl { get; set; }
        public byte? Status { get; set; }
    }
}
