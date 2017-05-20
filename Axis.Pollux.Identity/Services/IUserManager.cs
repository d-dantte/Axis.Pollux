using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using System.Collections.Generic;

namespace Axis.Pollux.Identity.Services
{
    public interface IUserManager
    {
        #region Biodata
        Operation<BioData> UpdateBioData(BioData data);
        
        Operation<BioData> GetBioData();
        #endregion

        #region Contact data
        Operation<ContactData> AddContactData(ContactData data);
        Operation<ContactData> UpdateContactData(ContactData data);

        Operation<ContactData> ArchiveContactData(long id);

        Operation<SequencePage<ContactData>> GetAllContactData(int pageSize = 500, int pageIndex = 0, bool includeCount = true);
        #endregion

        #region User data
        Operation<UserData> AddData(UserData data);
        
        Operation<UserData> UpdateData(UserData data);
        
        Operation<IEnumerable<UserData>> RemoveData(string[] names);
        
        Operation<SequencePage<UserData>> GetUserData(int pageSize=500, int pageIndex=0, bool includeCount = true);
        
        Operation<UserData> GetUserData(string name);
        #endregion

        #region User
        Operation<long> UserCount();
        Operation<User> CreateUser(string userId, int status);
        #endregion
    }
}
