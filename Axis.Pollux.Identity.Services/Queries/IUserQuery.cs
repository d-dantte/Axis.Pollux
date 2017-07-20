using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.UserCommon.Models;

namespace Axis.Pollux.Identity.Services.Queries
{
    public interface IUserQuery
    {
        BioData GetBioData(string userId);

        UserData GetContactData(long id);
        SequencePage<UserData> GetContactData(string userId, int? status = null, PageParams pageParams = null);

        UserData GetUserData(string userId, string dataName);
        SequencePage<UserData> GetUserData(string userId, PageParams pageParams = null);

        long GetUserCount();
        bool UserExists(string userId);

        AddressData GetAddressById(long id);
        SequencePage<AddressData> GetAddresses(string userId, AddressStatus? status, PageParams pageParams = null);
    }
}
