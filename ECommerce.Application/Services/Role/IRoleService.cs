using ECommerce.Application.Services.Role.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Role
{
    public interface IRoleService
    {
        Task<int> Create(RoleCreateRequest request);
        Task<int> Update(RoleModel request);
        Task<int> Delete(int RoleId);
        Task<List<RoleModel>> getAll();
    }
}
