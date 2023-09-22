using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Configurations.Dtos.Footer
{
    public class TimeUpdateRequest
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
