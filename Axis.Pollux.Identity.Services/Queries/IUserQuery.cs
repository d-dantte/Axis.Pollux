using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Common.Models;
using System;

namespace Axis.Pollux.Identity.Services.Queries
{
    public interface IUserQuery
    {
        User GetUserById(long userId);
        User GetUserUUID(Guid uuid);

        BioData GetBioData(long userId);

        ContactData GetContactData(long id);
        SequencePage<ContactData> GetContactData(long userId, ContactStatus? status = null, PageParams pageParams = null);

        UserData GetUserData(long userId, string dataName);
        SequencePage<UserData> GetUserData(long userId, PageParams pageParams = null);

        long GetUserCount();
        bool UserExists(string userName);

        AddressData GetAddressById(long id);
        SequencePage<AddressData> GetAddresses(long userId, AddressStatus? status, PageParams pageParams = null);
        bool UserIs(long userId, int status);
        SequencePage<ContactData> GetContactDataOfType(long userId, ContactChannel channel, ContactStatus? status, PageParams pageParams = null);
    }
}
