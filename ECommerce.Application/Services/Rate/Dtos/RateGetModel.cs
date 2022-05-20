﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Rate.Dtos
{
    public class RateGetModel
    {
        public int Value { get; set; }
        public string Comment { get; set; }
        public string UserName {get;set;}
        public DateTime CreateDate { get; set; }
        public List<string> Images { get; set; }
    }
}