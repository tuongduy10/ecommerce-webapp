using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Rate.Models
{
    public class LikeAndDislike
    {
        public int RateId { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
    }
}
