using ECommerce.Application.BaseServices.Configurations.Dtos;
using ECommerce.Application.BaseServices.Configurations.Dtos.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.Configurations.Header
{
    public class ManageHeaderViewModel
    {
        public List<HeaderModel> headers { get; set; }
        public List<BannerModel> banners { get; set; }
        public ConfigurationModel config { get; set; }
    }
}
