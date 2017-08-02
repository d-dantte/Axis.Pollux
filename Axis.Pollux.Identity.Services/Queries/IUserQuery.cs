using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Common.Models;
using System;

namespace Axis.Pollux.Identity.Services.Queries
{
    public interface IUserQuery
    {
        User GetUserById(string userId);
        User GetUserUUID(Guid uuid);

        BioData GetBioData(string userId);

        UserData GetContactData(long id);
        SequencePage<UserData> GetContactData(string userId, int? status = null, PageParams pageParams = null);

        UserData GetUserData(string userId, string dataName);
        SequencePage<UserData> GetUserData(string userId, PageParams pageParams = null);

        long GetUserCount();
        bool UserExists(string userId);

        AddressData GetAddressById(long id);
        SequencePage<AddressData> GetAddresses(string userId, AddressStatus? status, PageParams pageParams = null);
        bool UserIs(string userId, int status);
    }
}
