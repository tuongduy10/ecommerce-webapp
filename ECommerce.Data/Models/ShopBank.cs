using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class ShopBank
    {
        public int ShopBankId { get; set; }
        public string ShopBankName { get; set; }
        public string ShopAccountNumber { get; set; }
        public string ShopAccountName { get; set; }
        public int ShopId { get; set; }

        public virtual Shop Shop { get; set; }
    }
}
