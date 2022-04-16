using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.RoleFunction.Dtos
{
    public class RoleFunctionModel
    {
        public int RoleFunctionId { get; set; }
        public string RoleFunctionName { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}
