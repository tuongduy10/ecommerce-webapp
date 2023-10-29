using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Configurations.Dtos.Footer
{
    public class AddressUpdateRequest
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string AddressUrl { get; set; }
    }
}
