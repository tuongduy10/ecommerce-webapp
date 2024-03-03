using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Entities
{
    public partial class RoleFunction
    {
        public RoleFunction()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int RoleFunctionId { get; set; }
        public string RoleFunctionName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
