using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IContactDataManager
    {

        #region ContactData

        Operation<ContactData> AddContactData(Guid userId, ContactData contactData);
        Operation<ContactData> DeleteContactData(Guid contactDataId);
        Operation<ContactData> UpdateContactData(ContactData contactData);
        Operation UpdateContactDataStatus(Guid contactDataId, int status);

        Operation<ContactData> GetContactData(Guid contactDataId);
        Operation<ArrayPage<ContactData>> GetUserContact(Guid userId, ArrayPageRequest request = null);

        #endregion
    }
}
