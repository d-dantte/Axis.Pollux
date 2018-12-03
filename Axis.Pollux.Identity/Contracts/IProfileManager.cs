using System;
using System.Collections.Generic;
using System.Text;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IProfileManager: IAddressDataManager, IContactDataManager, IBioDataManager, INameDataManager, IUserDataManager, IUserManager
    {
    }
}
