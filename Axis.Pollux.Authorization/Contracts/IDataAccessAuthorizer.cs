using System;
using Axis.Luna.Operation;

namespace Axis.Pollux.Authorization.Contracts
{
    public interface IDataAccessAuthorizer
    {
        /// <summary>
        /// Attempt to ascertain if the user found in IUserContext.CurrentUserId() has the permission to access any data of the specified type.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        Operation AuthorizeAccess(string dataType);

        /// <summary>
        /// Attempt to ascertain if the user found in IUserContext.CurrentUserId() has the permission to access data identified by "uniqueId" of the specified type.
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        Operation AuthorizeAccess(string dataType, string uniqueId);

        /// <summary>
        /// Attempt to ascertain if the user found in IUserContext.CurrentUserId() has the permission to access any data of the specified type, owned by the specified owner.
        /// If OwnerId does not represent a valid user, the authorization is denied
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Operation AuthorizeAccess(string dataType, Guid ownerId);

        /// <summary>
        /// Attempt to ascertain if the user found in IUserContext.CurrentUserId() has the permission to access the specified data.
        /// If OwnerId does not represent a valid user, the authorization is denied
        /// </summary>
        /// <param name="dataType">A unique string label signifying the root data-type being accessed. Typically, this will be Type.FullName, but can be anything at all.</param>
        /// <param name="ownerId"> a unique value signifying that access to a specific object is requested.</param>
        /// <param name="uniqueId"> compared with the IUserContext.CurrentUserId() to determine automatic access to the data</param>
        /// <returns></returns>
        Operation AuthorizeAccess(string dataType, Guid ownerId, string uniqueId);
    }
}
