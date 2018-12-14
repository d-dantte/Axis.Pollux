using Axis.Luna.Operation;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IProfileManager
    {
        Operation<UserProfile> GetUserProfile(Params.UserProfileRequestInfo param);
    }
}
