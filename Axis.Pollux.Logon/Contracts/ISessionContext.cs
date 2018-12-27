using Axis.Luna.Operation;
using System;

namespace Axis.Pollux.Logon.Contracts
{
    public interface ISessionContext
    {
        Operation<Guid> CurrentSessionId();
    }
}
