using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Dtos.Common
{
    public class RemoveFilesRequest
    {
        public List<string> fileNames { get; set; }
        public string uploadType { get; set; }
    }
}
