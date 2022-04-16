using ECommerce.Application.Services.RoleFunction.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.RoleFunction
{
    interface IRoleFunctionService
    {
        Task<int> Create(RoleFunctionCreateRequest request);
        Task<int> Update(RoleFunctionModel request);
        Task<int> Delete(int RoleFunctionId);
        Task<List<RoleFunctionModel>> getAll();

    }
}
