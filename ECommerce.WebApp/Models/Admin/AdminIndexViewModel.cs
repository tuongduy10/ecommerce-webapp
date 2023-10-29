using ECommerce.Application.BaseServices.Rate.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.Admin
{
    public class AdminIndexViewModel
    {
        public List<RateGetModel> comments { get; set; }
    }
}
