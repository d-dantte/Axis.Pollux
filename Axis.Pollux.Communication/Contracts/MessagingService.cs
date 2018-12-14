using System;
using System.Collections.Generic;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Communication.Exceptions;
using Axis.Pollux.Communication.Models;
using Axis.Pollux.Identity.Models;

using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Communication.Contracts
{
    public class MessagingService
    {
        private readonly ICommsChannel[] _channels;


        public MessagingService(IEnumerable<ICommsChannel> channels)
        {
            ThrowNullArguments(() => channels);

            _channels = channels
                .ToArray()
                .ThrowIf(
                    arr => arr.Length == 0,
                    new CommunicationException(Common.Exceptions.ErrorCodes.InvalidArgument));
        }

        public Operation<MessageRef> PushToContact(string sourceAddress, ContactData target, IMessagePayload payload)
        => Operation.Try(async () =>
        {
            var channel = _channels
                .FirstOrDefault(c => string.Equals(c.ContactChannel, target.Channel, StringComparison.InvariantCulture))
                .ThrowIfNull(new CommunicationException(ErrorCodes.NoSuitableChannelFound));

            return await channel.PushToContact(sourceAddress, target, payload);
        });

        public Operation<MessageRef> PushToContact(
            string sourceAddress, 
            ContactData target, 
            string transportType, 
            IMessagePayload payload)
        => Operation.Try(async () =>
        {
            var channel = _channels
                .Where(c => string.Equals(c.ContactChannel, target.Channel, StringComparison.InvariantCulture))
                .FirstOrDefault(c => string.Equals(c.TransportType, transportType, StringComparison.InvariantCulture))
                .ThrowIfNull(new CommunicationException(ErrorCodes.NoSuitableChannelFound));

            return await channel.PushToContact(sourceAddress, target, payload);
        });

        public Operation<MessageEntry> GetNotification(MessageRef messageRef)
        => Operation.Try(async () =>
        {
            var channel = _channels
                .Where(c => string.Equals(c.TransportType, messageRef.TransportType, StringComparison.InvariantCulture))
                .FirstOrDefault(c => string.Equals(c.ContactChannel, messageRef.ContactChannel, StringComparison.InvariantCulture))
                .ThrowIfNull(new CommunicationException(ErrorCodes.NoSuitableChannelFound));

            return await channel.GetNotification(messageRef);
        });

        public Operation<int> QueryStatus(MessageRef messageRef)
        => Operation.Try(async () =>
        {
            var channel = _channels
                .Where(c => string.Equals(c.TransportType, messageRef.TransportType, StringComparison.InvariantCulture))
                .FirstOrDefault(c => string.Equals(c.ContactChannel, messageRef.ContactChannel, StringComparison.InvariantCulture))
                .ThrowIfNull(new CommunicationException(ErrorCodes.NoSuitableChannelFound));

            return await channel.QueryStatus(messageRef);
        });

        public Operation<ArrayPage<MessageRef>> GetAllNotificationRefs(
            string contactChannel, 
            string transportType, 
            int? status, 
            ArrayPageRequest pageRequest = null)
        => Operation.Try(async () =>
        {
            return await _channels
                .Where(c => string.Equals(c.TransportType, transportType, StringComparison.InvariantCulture))
                .FirstOrDefault(c => string.Equals(c.ContactChannel, contactChannel, StringComparison.InvariantCulture))
                .ThrowIfNull(new CommunicationException(ErrorCodes.NoSuitableChannelFound))
                .GetAllNotificationRefs(status, pageRequest);
        });

        public Operation<ArrayPage<MessageRef>> GetUserNotificationRefs(
            Guid userId, 
            string contactChannel, 
            string transportType,
            int? status, 
            ArrayPageRequest pageRequest = null)
        => Operation.Try(async () =>
        {
            return await _channels
                .Where(c => string.Equals(c.TransportType, transportType, StringComparison.InvariantCulture))
                .FirstOrDefault(c => string.Equals(c.ContactChannel, contactChannel, StringComparison.InvariantCulture))
                .ThrowIfNull(new CommunicationException(ErrorCodes.NoSuitableChannelFound))
                .GetUserNotificationRefs(userId, status, pageRequest);
        });
    }
}
