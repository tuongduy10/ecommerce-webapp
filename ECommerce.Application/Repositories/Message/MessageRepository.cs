using ECommerce.Application.Repositories.Message.Dtos;
using ECommerce.Application.Services.Chat.Dtos;
using ECommerce.Data.Context;
using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Message
{
    public class MessageRepository : RepositoryBase<Data.Entities.MessageHistory>, IMessageRepository
    {
        public MessageRepository(ECommerceContext DbContext) : base(DbContext) { }
    }
}
