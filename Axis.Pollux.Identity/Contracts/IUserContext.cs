using System;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IUserContext
    {
        Guid CurrentUserId();
    }
}
