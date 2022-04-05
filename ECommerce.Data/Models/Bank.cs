using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Bank
    {
        public int BankId { get; set; }
        public decimal? BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string BankImage { get; set; }
        public string BankName { get; set; }
        public byte? Status { get; set; }
    }
}
