using ECommerce.Application.Services.Configurations.Dtos.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public interface IHeaderService
    {
        Task<int> Update(HeaderUpdateRequest request);
        Task<List<HeaderModel>> getAll();
    }
}
