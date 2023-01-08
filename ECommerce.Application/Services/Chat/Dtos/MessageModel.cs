using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Chat.Dtos
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? Attachment { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateDateLabel { get; set; }
        public int FromUserId { get; set; }
        public int? ToUserId { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
    }
}
