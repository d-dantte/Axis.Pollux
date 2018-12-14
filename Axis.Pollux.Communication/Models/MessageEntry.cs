using System;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Communication.Models
{
    public class MessageEntry: BaseModel<Guid>
    {
        public IMessagePayload Payload { get; set; }

        public int Status { get; set; }

        public string Channel { get; set; }
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }

        public Guid? TargetUserId { get; set; }

        public string RenderedMessage { get; set; }
        public string RenderedTitle { get; set; }
    }

    public enum NotificationStatus
    {
        Pending,
        Sent,
        Canceled,
        Delivered,
        Peeked,
        Seen
    }
}
