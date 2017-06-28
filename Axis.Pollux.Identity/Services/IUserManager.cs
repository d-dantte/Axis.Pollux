using Axis.Luna;
using Axis.Luna.Operation;
using Axis.Pollux.Identity.Principal;
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
        IOperation<ContactData> AddContactData(ContactData data);
        IOperation<ContactData> UpdateContactData(ContactData data);

        IOperation<ContactData> ArchiveContactData(long id);

        IOperation<SequencePage<ContactData>> GetAllContactData(int pageSize = 500, int pageIndex = 0, bool includeCount = true);
        #endregion

        #region User data
        IOperation<UserData> AddData(UserData data);
        
        IOperation<UserData> UpdateData(UserData data);
        
        IOperation<IEnumerable<UserData>> RemoveData(string[] names);
        
        IOperation<SequencePage<UserData>> GetUserData(int pageSize=500, int pageIndex=0, bool includeCount = true);
        
        IOperation<UserData> GetUserData(string name);
        #endregion

        #region User
        IOperation<long> UserCount();
        IOperation<User> CreateUser(string userId, int status);
        #endregion
    }
}
