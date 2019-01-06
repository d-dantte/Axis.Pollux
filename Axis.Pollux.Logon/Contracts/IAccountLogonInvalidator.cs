using System;
using Axis.Luna.Operation;
using Axis.Pollux.Logon.Models;

namespace Axis.Pollux.Logon.Contracts
{

    public interface IAccountLogonInvalidator
    {
        /// <summary>
        /// Invalidates a user logon. Note that the token should be unique for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Operation<UserLogon> InvalidateLogon(Guid userId, string token);

        /// <summary>
        /// Invalidates a user logon identified by the given id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="logonId"></param>
        /// <returns></returns>
        Operation<UserLogon> InvalidateLogon(Guid userId, Guid logonId);
    }
}
