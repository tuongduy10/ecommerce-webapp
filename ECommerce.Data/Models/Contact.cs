using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Contact
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public string WardCode { get; set; }
        public string DistrictCode { get; set; }
        public string CityCode { get; set; }
    }
}
