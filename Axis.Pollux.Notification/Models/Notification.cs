using Axis.Pollux.Identity;
using Axis.Pollux.Identity.Principal;
using System;
using System.Collections.Generic;

namespace Axis.Pollux.Notification.Models
{
    public class Notification: Common.PolluxModel<long>, IUserTargeted
    {
        public Guid UUId { get; set; } = Guid.NewGuid();

        public User Target { get; set; }

        public string Origin { get; set; }

        public string[] Channels { get; set; }

        public IDictionary<string, object> Data { get; set; }

        public string Title { get; set; }

        public NotificationStatus Status { get; set; }
    }
}
