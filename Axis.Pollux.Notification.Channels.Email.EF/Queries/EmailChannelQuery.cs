using Axis.Pollux.Notification.Email.Queries;
using System;
using Axis.Jupiter.Europa;
using System.Linq;
using Axis.Pollux.Notification.Email.EF.Entities;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.Notification.Email.EF.Queries
{
    public class EmailChannelQuery : IChannelQuery
    {
        private DataStore _store;

        public EmailChannelQuery(DataStore store)
        {
            ThrowNullArguments(() => store);

            _store = store;
        }

        public Models.Notification GetNotification(Guid notificationId)
        => _store
            .Query<EmailNotificationEntity>(_n => _n.Target)
            .Where(_n => _n.UUId == notificationId)
            .FirstOrDefault()
            .Transform<EmailNotificationEntity, Models.Notification>(_store);
    }
}
