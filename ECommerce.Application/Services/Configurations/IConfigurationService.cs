using ECommerce.Application.Services.Configurations.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public interface IConfigurationService
    {
        Task<ConfigurationModel> getConfiguration();
    }
}
