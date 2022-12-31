using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Chat.Dtos
{
    public class OfflineMessage
    {
        public string? Message { get; set; }
        public string? PhoneNumber { get; set; }     
        public string? UserName { get; set; }    
    }
}
