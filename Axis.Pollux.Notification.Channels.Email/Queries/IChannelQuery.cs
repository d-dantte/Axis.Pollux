using System;

namespace Axis.Pollux.Notification.Email.Queries
{
    public interface IChannelQuery
    {
        Models.Notification GetNotification(Guid notificationId);
    }
}
