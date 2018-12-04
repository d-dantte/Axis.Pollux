using Axis.Luna.Operation;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IProfileManager: IAddressDataManager, IContactDataManager, IBioDataManager, INameDataManager, IUserDataManager, IUserManager
    {
        Operation<UserProfile> GetUserProfile(Params.UserProfileRequest param);
    }
}
