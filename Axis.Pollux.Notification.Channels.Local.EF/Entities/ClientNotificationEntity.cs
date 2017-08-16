using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Notification.Models;
using System;

namespace Axis.Pollux.Notification.Client.EF.Entities
{
    public class ClientNotificationEntity: Common.PolluxEntity<long>
    {
        public string TargetId { get; set; }
        public UserEntity Target { get; set; }

        public Guid UUId { get; set; }

        public string Origin { get; set; }

        public string Channels { get; set; }

        public string Data { get; set; }

        public string Title { get; set; }

        public NotificationStatus Status { get; set; }
    }
}
