using System;
using Axis.Luna.Operation;
using Axis.Pollux.Notification.Models;

using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ValidatableExtensions;
using Axis.Jupiter.Commands;
using Axis.Pollux.Notification.Client.Queries;
using Axis.Luna;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Identity.Services;

namespace Axis.Pollux.Notification.Client.Services
{
    public class ChannelService : IClientNotifier
    {
        private IPersistenceCommands _pcommands;
        private IChannelQuery _query;
        private IUserContext _userContext;

        public ChannelInfo Info { get; private set; }


        public ChannelService(IChannelQuery query, IPersistenceCommands pcommands, IUserContext userContext)
        {
            ThrowNullArguments(() => pcommands,
                               () => query,
                               () => userContext);
            
            _pcommands = pcommands;
            _query = query;
            _userContext = userContext;
        }

        public IOperation<Guid> PushNotification<D>(Models.Notification<D> notification)
        => ValidateModels(notification)
        .Then(() =>
        {
            notification.Status = NotificationStatus.Delivered;
            return _pcommands.Add(notification as Models.Notification);
        })
        .Then(_ => _.UUId);

        public IOperation<Models.Notification> GetNotification(Guid notificationId)
        => LazyOp.Try(() =>
        {
            var notification = _query.GetNotification(notificationId);
            return notification;
        });

        public IOperation<Models.Notification> UpdateStatus(Guid notificationId, NotificationStatus status)
        => LazyOp.Try(() =>
        {
            var n = _query.GetNotification(notificationId);
            switch(n.Status)
            {
                case NotificationStatus.Delivered: return Deliver(n);
                case NotificationStatus.Seen: return See(n);
                default: throw new Exception("Invalid status");
            }
        });

        public IOperation<SequencePage<Models.Notification>> GetNotifications(NotificationStatus status, PageParams @params = null)
        => LazyOp.Try(() =>
        {
            return _query.GetNotifications(_userContext.User().UserId, status, @params);
        });

        public IOperation<long> GetCount(NotificationStatus status)
        => LazyOp.Try(() =>
        {
            return _query.GetCount(_userContext.User().UserId, status);
        });



        private Models.Notification Deliver(Models.Notification notification)
        {
            if(notification.Status == NotificationStatus.Seen)
            {
                notification.Status = NotificationStatus.Delivered;
                notification = _pcommands.Update(notification).Resolve();
            }

            return notification;
        }
        private Models.Notification See(Models.Notification notification)
        {
            if (notification.Status == NotificationStatus.Delivered)
            {
                notification.Status = NotificationStatus.Seen;
                notification = _pcommands.Update(notification).Resolve();
            }

            return notification;
        }
    }
}
