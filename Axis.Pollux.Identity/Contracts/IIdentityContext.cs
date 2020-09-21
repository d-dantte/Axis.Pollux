using Axis.Luna.Operation;
using System;

namespace Axis.Pollux.Identity.Contracts
{
    /// <summary>
    /// Enables impersonation to execute a given action as another user. Useful for elevating privileges, 
    /// also potentially dangerous
    /// </summary>
    public interface IIdentityContext
    {
        Operation ExecuteAs(Guid userId, Action<Guid> action);
        Operation ExecuteAs(Guid userId, Func<Guid, Operation> function);

        Operation<R> ExecuteAs<R>(Guid userId, Func<Guid, R> function);
        Operation<R> ExecuteAs<R>(Guid userId, Func<Guid, Operation<R>> function);
    }
}
