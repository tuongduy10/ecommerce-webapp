using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Brand.Dtos
{
    public class FileChangedResponse
    {
        public string previousFileName { get; set; }
        public string newFileName { get; set; }
    }
}
