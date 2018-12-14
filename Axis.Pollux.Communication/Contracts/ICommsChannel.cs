using System;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Communication.Models;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Communication.Contracts
{
    public interface ICommsChannel
    {
        /// <summary>
        /// The contact channel that this service can communicate with (mobile, email, etc)
        /// </summary>
        string ContactChannel { get; }

        /// <summary>
        /// The transport type used in communicating with the contact (voice call, sms, mms, email, IM, etc).
        /// This exists because some channels can be communicated with in different ways.
        /// </summary>
        string TransportType { get; }

        Operation<MessageRef> PushToContact(string sourceAddress, ContactData target, IMessagePayload payload);

        Operation<MessageEntry> GetNotification(MessageRef messageRef);

        Operation<int> QueryStatus(MessageRef messageRef);

        Operation<ArrayPage<MessageRef>> GetAllNotificationRefs(int? status, ArrayPageRequest pageRequest = null);

        Operation<ArrayPage<MessageRef>> GetUserNotificationRefs(
            Guid targetId, 
            int? status,
            ArrayPageRequest pageRequest = null);

    }
}
