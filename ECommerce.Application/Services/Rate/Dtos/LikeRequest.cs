using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Rate.Dtos
{
    public class LikeRequest
    {
        public int userId { get; set; }
        public int rateId { get; set; }
        public bool liked { get; set; }
    }
}
