using System;
using System.Threading.Tasks;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Services.Queries
{
    public interface IUserQueries
    {
        Task<User> GetUserById(Guid userId);
        Task<long> UserCount();
        Task<ArrayPage<AddressData>> GetUserAddressData(Guid userId, ArrayPageRequest request = null);
        Task<ArrayPage<ContactData>> GetUserContactData(Guid userId, ArrayPageRequest request = null);
        Task<ArrayPage<NameData>> GetUserNameData(Guid userId, ArrayPageRequest request = null);
        Task<ArrayPage<UserData>> GetUserData(Guid userId, ArrayPageRequest request = null);
        Task<BioData> GetUserBioData(Guid userId);
        Task<BioData> GetBioDataById(Guid bioDataId);
        Task<NameData> GetNameDataById(Guid nameData);
        Task<AddressData> GetAddressDataById(Guid addressDataId);
        Task<ContactData> GetContactDataById(Guid contactDataId);
        Task<UserData> GetUserDataById(Guid userDataId);
        Task<ContactData> GetPrimaryUserContactData(Guid userId, string communicationChannel);
        Task<ArrayPage<ContactData>> GetUserContactData(
            Guid userId,
            string[] communicationChannels,
            string[] tags,
            ArrayPageRequest arrayPageRequest);
    }
}
