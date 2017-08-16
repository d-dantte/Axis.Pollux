using Axis.Pollux.Notification.Services;
using System;
using Axis.Luna.Operation;
using Axis.Pollux.Notification.Models;
using Axis.Pollux.Common.Services;
using Axis.Pollux.Utils.Services;

using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ValidatableExtensions;
using static Axis.Luna.Extensions.EnumerableExtensions;
using Axis.Pollux.Common.Models.Email;
using System.Linq;
using Axis.Jupiter.Commands;
using Axis.Pollux.Notification.Email.Queries;

namespace Axis.Pollux.Notification.Email.Services
{
    public class ChannelService : INotifierChannel
    {
        private IEmailClient _emailClient;
        private IHtmlEmailRenderer _renderer;
        private IPersistenceCommands _pcommands;
        private IChannelQuery _query;

        public ChannelInfo Info { get; private set; }


        public ChannelService(IEmailClient emailService, IHtmlEmailRenderer renderer, IChannelQuery query, IPersistenceCommands pcommands)
        {
            ThrowNullArguments(() => emailService,
                               () => renderer,
                               () => pcommands,
                               () => query);

            _emailClient = emailService;
            _renderer = renderer;
            _pcommands = pcommands;
            _query = query;
        }

        public IOperation<Guid> PushNotification(Models.Notification notification)
        => ValidateModels(notification)
        .Then(() => _renderer.RenderHtml(notification.Data))
        .Then(_html =>
        {
            var payload = new Payload
            {
                Sender = notification.Origin,
                Recipients = Enumerate(notification.Target.UserId).ToList(),
                Message = new Common.Models.Message
                {
                    Subject = notification.Title,
                    Body = _html,
                    IsBodyHtml = true
                }
            };

            return _pcommands.Add(notification) //<-- persist the notification first
                .Then(_ => _emailClient.Send(payload))
                .Then(_ =>
                {
                    notification.UUId = _ ?? Guid.Empty; //<-- return empty guids for emails that have none
                    notification.Status = NotificationStatus.Delivered; //<-- should be changed to "Sent" after QueryStatus has been implemented fully!!!
                    return _pcommands.Update(notification);
                })
                .Then(_ => _.UUId);
        });

        public IOperation<Models.Notification> GetNotification(Guid operationId)
        => LazyOp.Try(() =>
        {
            var notification = _query.GetNotification(operationId);
            switch (notification.Status)
            {
                case NotificationStatus.Failed:
                case NotificationStatus.Delivered:
                case NotificationStatus.Seen: return notification;
                case NotificationStatus.Sent:
                    var status = _emailClient.QueryStatus(operationId).Resolve();
                    if(status == EmailStatus.Delivered)
                    {
                        notification.Status = NotificationStatus.Delivered;
                        _pcommands.Update(notification).Resolve();
                    }
                    return notification;
                default: throw new Exception("invalid status");
            }
        });
    }
}
