using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Contracts
{
    public interface INameDataManager
    {

        #region NameData

        Operation<NameData> AddNameData(Guid userId, NameData nameData);
        Operation<NameData> DeleteNameData(Guid nameDataId);
        Operation<NameData> UpdateNameData(NameData nameData);
        Operation UpdateNameDataStatus(Guid nameDataId, int status);

        Operation<NameData> GetNameData(Guid nameDataId);
        Operation<ArrayPage<NameData>> GetUserName(Guid userId, ArrayPageRequest request = null);

        #endregion
    }
}
