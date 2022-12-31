using System;
using System.Collections.Generic;

namespace ECommerce.Data.Models
{
    public partial class Bank
    {
        public int BankId { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string BankImage { get; set; }
        public string BankName { get; set; }
        public byte? Status { get; set; }
    }
}
