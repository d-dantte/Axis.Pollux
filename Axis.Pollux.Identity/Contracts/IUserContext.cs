using System;
using Axis.Luna.Operation;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IUserContext
    {
        Operation<Guid> CurrentUserId();
    }
}
