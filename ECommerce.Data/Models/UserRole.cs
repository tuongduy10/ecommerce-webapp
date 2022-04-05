using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int RoleFunctionId { get; set; }

        public virtual Role Role { get; set; }
        public virtual RoleFunction RoleFunction { get; set; }
        public virtual User User { get; set; }
    }
}
