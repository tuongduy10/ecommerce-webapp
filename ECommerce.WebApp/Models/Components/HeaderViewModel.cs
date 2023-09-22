using ECommerce.Application.BaseServices.Configurations.Dtos;
using ECommerce.Application.BaseServices.Configurations.Dtos.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.Components
{
    public class HeaderViewModel
    {
        public List<HeaderModel> listHeader { get; set; }
        public ConfigurationModel config { get; set; }
    }
}
