using ECommerce.Application.BaseServices.Role.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Role
{
    public interface IRoleService
    {
        Task<int> Create(RoleCreateRequest request);
        Task<int> Update(RoleModel request);
        Task<int> Delete(int RoleId);
        Task<List<RoleModel>> getAll();
    }
}
