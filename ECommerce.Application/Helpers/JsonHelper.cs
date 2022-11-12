using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Helpers
{
    public class JsonHelper<T> where T : class
    {
        public string Stringify(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public T Parse(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
