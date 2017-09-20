using Axis.Luna;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Notification.Models;
using Axis.Pollux.Notification.Services;
using System;

namespace Axis.Pollux.Notification.Client.Services
{
    public interface IClientNotifier: INotifierChannel
    {
        IOperation<Models.Notification> UpdateStatus(Guid notificationId, NotificationStatus status);

        IOperation<SequencePage<Models.Notification>> GetNotifications(NotificationStatus status, PageParams @params = null);

        IOperation<long> GetCount(NotificationStatus status);
    }
}
