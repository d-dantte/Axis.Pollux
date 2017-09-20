using Axis.Luna.Operation;
using System;
using System.Collections.Generic;

namespace Axis.Pollux.Notification.Services
{
    public interface INotifier
    {
        IOperation<IEnumerable<KeyValuePair<string, Guid>>> NotifyTarget<Data>(string origin, string targetUser, string title, Data data, params string[] channels);
    }
}
