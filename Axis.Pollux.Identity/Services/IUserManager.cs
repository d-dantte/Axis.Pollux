using Axis.Luna;
using Axis.Luna.Operation;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Common.Models;
using System.Collections.Generic;

namespace Axis.Pollux.Identity.Services
{
    public interface IUserManager
    {
        #region Biodata
        IOperation<BioData> UpdateBioData(BioData data);
        
        IOperation<BioData> GetBioData();
        #endregion

        #region Contact data
        IOperation<ContactData> AddContactData(ContactChannel channel, string data);
        IOperation DeleteContactData(long id);

        IOperation VerifyContactData(long contactId);
        IOperation ArchiveContactData(long contactId);


        IOperation<SequencePage<ContactData>> GetContactData(ContactStatus? status = null, PageParams pageParams = null);
        IOperation<ContactData> GetContactData(long contactId);

        IOperation<SequencePage<ContactData>> GetContactDataOfType(ContactChannel channel, ContactStatus? status = null, PageParams pageParams = null);
        #endregion

        #region User data
        IOperation<UserData> AddData(UserData data);
        
        IOperation<UserData> UpdateData(UserData data);
        
        IOperation<IEnumerable<UserData>> RemoveData(string[] names);
        
        IOperation<SequencePage<UserData>> GetUserData(PageParams pageParams);
        
        IOperation<UserData> GetUserData(string name);
        #endregion

        #region Address data
        IOperation<AddressData> AddAddressData(AddressData address);
        IOperation<AddressData> ModifyAddressData(AddressData address);
        IOperation<AddressData> ArchiveAddress(long id);
        IOperation<AddressData> ActivateAddress(long id);

        IOperation<SequencePage<AddressData>> GetAddresses(AddressStatus? status = null, PageParams pageParams = null);
        IOperation<AddressData> GetAddress(long id);
        #endregion

        #region User
        IOperation<long> UserCount();
        IOperation<User> CreateUser(string userName, int status);
        IOperation<bool> UserIs(long userId, int status);
        #endregion
    }
}
