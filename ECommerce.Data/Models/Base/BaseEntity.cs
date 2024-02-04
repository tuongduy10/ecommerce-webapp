using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Data.Models.Base
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
