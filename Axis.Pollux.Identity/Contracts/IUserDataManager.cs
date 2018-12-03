using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IUserDataManager
    {

        #region UserData

        Operation<UserData> AddUserData(Guid userId, UserData userData);
        Operation<UserData> DeleteUserData(Guid userDataId);
        Operation<UserData> UpdateUserData(UserData userData);
        Operation UpdateUserDataStatus(Guid userDataId, int status);

        Operation<UserData> GetUserData(Guid userDataId);
        Operation<ArrayPage<UserData>> GetUserUser(Guid userId, ArrayPageRequest request = null);

        #endregion
    }
}
