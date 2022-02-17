using System;

namespace Infrastructure.Core.Messages.OrderQueueMessages
{
    public class NewKitchenOrderMessage
    {
        public Guid OrderId { get; set; }
    }
}
