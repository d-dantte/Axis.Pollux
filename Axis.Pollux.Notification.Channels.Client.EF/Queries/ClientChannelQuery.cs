using System;
using Axis.Jupiter.Europa;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Pollux.Notification.Client.EF.Entities;
using System.Linq;
using Axis.Pollux.Notification.Client.Queries;
using Axis.Luna;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Notification.Models;
using Axis.Luna.Extensions;

namespace Axis.Pollux.Notification.Client.EF.Queries
{
    public class ClientChannelQuery : IChannelQuery
    {
        private DataStore _store;

        public ClientChannelQuery(DataStore store)
        {
            ThrowNullArguments(() => store);

            _store = store;
        }

        public Models.Notification GetNotification(Guid notificationId)
        => _store
            .Query<ClientNotificationEntity>(_n => _n.Target)
            .Where(_n => _n.UUId == notificationId)
            .FirstOrDefault()
            .Transform<ClientNotificationEntity, Models.Notification>(_store);

        public long GetCount(string target, NotificationStatus status)
        => _store
            .Query<ClientNotificationEntity>()
            .LongCount(_n => _n.Status == status);

        public SequencePage<Models.Notification> GetNotifications(string target, NotificationStatus status, PageParams @params = null)
        => _store
            .Query<ClientNotificationEntity>(_n => _n.Target)
            .Where(_n => _n.TargetId == target)
            .Where(_n => _n.Status == status)
            .OrderByDescending(_n => _n.CreatedOn)
            .Pipe(_q =>
            {
                @params = @params ?? PageParams.EntireSequence();
                return @params.Paginate(_q, __q => __q.Transform<ClientNotificationEntity, Models.Notification>(_store));
            });
    }
}
