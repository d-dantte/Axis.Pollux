using Axis.Luna.Operation;
using Axis.Pollux.Notification.Models;
using System;

namespace Axis.Pollux.Notification.Services
{
    public interface INotifierChannel
    {
        ChannelInfo Info { get; }

        /// <summary>
        /// There is no guarantee
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        IOperation<Guid> PushNotification<D>(Models.Notification<D> notification);

        IOperation<Models.Notification> GetNotification(Guid notificationId);
    }
}