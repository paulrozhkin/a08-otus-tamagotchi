using System;

namespace Infrastructure.Core.Messages.OrderQueueMessages
{
    public class NewKitchenStatusMessage
    {
        public Guid OrderId { get; set; }

        public string NewStatus { get; set; }
    }
}
