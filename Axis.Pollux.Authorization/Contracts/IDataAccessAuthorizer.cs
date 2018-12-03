using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Operation;

namespace Axis.Pollux.Authorization.Contracts
{
    public interface IDataAccessAuthorizer
    {
        /// <summary>
        /// Attempt to ascertain if the user found in IUserContext.CurrentUserId() has the permission to access the specified data.
        /// </summary>
        /// <param name="dataType">A unique string label signifying the root data-type being accessed. Typically, this will be Type.FullName, but can be anything at all.</param>
        /// <param name="uniqueId">When present, a unique value signifying that access to a specific object is requested. If absent, means access to any object of the specified data type is requested</param>
        /// <param name="ownerId">When present, compared with the IUserContext.CurrentUserId() to determine automatic access to the data, else is ignored</param>
        /// <returns></returns>
        Operation AuthorizeAccess(string dataType, string uniqueId, Guid ownerId = default(Guid));
    }
}
