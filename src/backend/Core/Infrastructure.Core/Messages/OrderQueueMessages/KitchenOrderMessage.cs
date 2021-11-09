using System;

namespace Infrastructure.Core.Messages.OrderQueueMessages
{
    public class KitchenOrderMessage
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public DateTime CreateTime { get; set; }
    }
}