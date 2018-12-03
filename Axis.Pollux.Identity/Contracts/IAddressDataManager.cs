using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IAddressDataManager
    {

        #region AddressData

        Operation<AddressData> AddAddressData(Guid userId, AddressData addressData);
        Operation<AddressData> DeleteAddressData(Guid addressDataId);
        Operation<AddressData> UpdateAddressData(AddressData addressData);
        Operation UpdateAddressDataStatus(Guid addressDataId, int status);

        Operation<AddressData> GetAddressData(Guid addressDataId);
        Operation<ArrayPage<AddressData>> GetUserAddresses(Guid userId, ArrayPageRequest request = null);

        #endregion
    }
}
