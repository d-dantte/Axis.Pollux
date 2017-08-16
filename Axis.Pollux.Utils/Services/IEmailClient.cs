using Axis.Luna.Operation;
using Axis.Pollux.Common.Models.Email;
using System;

namespace Axis.Pollux.Common.Services
{
    public interface IEmailClient
    {
        IOperation<Guid?> Send(Payload payload);

        IOperation<EmailStatus> QueryStatus(Guid emailId);
    }

    public enum EmailStatus
    {
        Unknown,
        Pending,
        Sent,
        Delivered,
        Read
    }
}