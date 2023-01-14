using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Helpers
{
    public class CommonHelper<T> where T : class
    {
        public string FormatPhoneNumber(T str)
        {
            var phoneNumber = str.ToString();
            if (phoneNumber.Contains("+84"))
            {
                phoneNumber = phoneNumber.Replace("+84", "");
                if (!phoneNumber.StartsWith("0"))
                {
                    return "0" + phoneNumber;
                }
            }
            return phoneNumber;
        }
    }
}
