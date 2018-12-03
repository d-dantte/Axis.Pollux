using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Operation;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IBioDataManager
    {

        #region BioData

        Operation<BioData> CreateBioData(Guid userId, BioData bioData);
        Operation<BioData> DeleteBioData(Guid bioDataId);
        Operation<BioData> UpdateBioData(BioData bioData);
        Operation UpdateBioDataStatus(Guid bioDataId, int status);

        Operation<BioData> GetUserBioData(Guid bioDataId);

        #endregion
    }
}
