using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Data.Entities
{
    public class MessageHistory
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? Attachment { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? ToPhoneNumber { get; set; }
        public string? FromName { get; set; }
        public string? FromPhoneNumber { get; set; } 
        public string? Type { get; set; }
        public string? Status { get; set; }
    }
}
