using Axis.Luna;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Notification.Models;
using System;

namespace Axis.Pollux.Notification.Client.Queries
{
    public interface IChannelQuery
    {
        Models.Notification GetNotification(Guid notificationId);
        SequencePage<Models.Notification> GetNotifications(string target, NotificationStatus status,  PageParams @params = null);
        long GetCount(string target, NotificationStatus status);
    }
}
