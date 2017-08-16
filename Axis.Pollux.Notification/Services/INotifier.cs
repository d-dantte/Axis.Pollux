using Axis.Luna.Operation;
using System;
using System.Collections.Generic;

namespace Axis.Pollux.Notification.Services
{
    public interface INotifier
    {
        IOperation<IEnumerable<KeyValuePair<string, Guid>>> NotifyTarget(string targetUser, Models.Notification notification);
    }
}
