using ECommerce.Application.BaseServices.Role.Dtos;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Role
{

    public class RoleService : IRoleService
    {
        private ECommerceContext _DbContext;
        public RoleService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<int> Create(RoleCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(int AttributeId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RoleModel>> getAll()
        {
            var result = await _DbContext.Roles
                .Select(i => new RoleModel { 
                    RoleId = i.RoleId,
                    RoleName = i.RoleName
                })
                .ToListAsync();
            return result;
        }

        public async Task<int> Update(RoleModel request)
        {
            throw new NotImplementedException();
        }
    }
}
