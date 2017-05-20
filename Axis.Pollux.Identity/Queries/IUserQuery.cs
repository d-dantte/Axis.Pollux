using Axis.Luna;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Identity.Queries
{
    public interface IUserQuery
    {
        BioData GetBioData(string userId);

        ContactData GetContactData(long id);
        SequencePage<ContactData> GetContactData(string userId, int pageSize = 500, int pageIndex = 0, bool includeCount = true);

        UserData GetUserData(string userId, string dataName);
        SequencePage<UserData> GetUserData(string userId, int pageSize = 500, int pageIndex = 0, bool includeCount = true);

        long GetUserCount();
        bool UserExists(string userId);
    }
}
