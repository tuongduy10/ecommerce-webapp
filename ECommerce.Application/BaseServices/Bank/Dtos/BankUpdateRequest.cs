using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Bank.Dtos
{
    public class BankUpdateRequest
    {
        public int BankId { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string BankImage { get; set; }
        public string BankName { get; set; }
    }
}
