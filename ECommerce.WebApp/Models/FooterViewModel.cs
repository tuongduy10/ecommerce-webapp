using ECommerce.Application.Services.Configurations.Dtos;
using ECommerce.Application.Services.Configurations.Dtos.Footer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models
{
    public class FooterViewModel
    {
        public List<BlogModel> listBlog { get; set; }

        public List<SocialModel> listSocial { get; set; }

        public ConfigurationModel config { get; set; }
    }
}
