using ECommerce.Application.BaseServices.Configurations.Dtos;
using ECommerce.Application.BaseServices.Configurations.Dtos.Footer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.Configurations.Footer
{
    public class ManageFooterViewModel
    {
        public List<BlogModel> listBlog { get; set; }
        public List<SocialModel> listSocial { get; set; }
        public ConfigurationModel config { get; set; }
    }
}
