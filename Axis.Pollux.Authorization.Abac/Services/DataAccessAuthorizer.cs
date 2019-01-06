﻿using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Axis.Luna.Common;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Abac.Contracts;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Authorization.Contracts.Params;
using Axis.Pollux.Authorization.Exceptions;
using Axis.Pollux.Common.Contracts;
using Axis.Pollux.Identity.Contracts;
using Axis.Sigma;
using Axis.Sigma.Authority;

using static Axis.Luna.Extensions.ExceptionExtension;
using Attribute = Axis.Pollux.Authorization.Abac.Models.Attribute;

namespace Axis.Pollux.Authorization.Abac.Services
{
    /// <summary>
    /// Caters for data authorization by appending "resource" attributes through the injected AuthorizationContextProvider,
    /// to its returned AuthorizationContext. It is important that the policy rule providers/configurations be aware of
    /// these resources because they identify the resource (data) whose access is being challenged.
    /// The appended resources include:
    /// 1. [name = Pollux.DataAccess.DataType; Type = string; Data = {FullName of c# type}] //represents the data type being accessed
    /// 2. [name = Pollux.DataAccess.OwnerId; Type = guid; Data = {guid of the principal who owns data}] //represents the owner of the data
    /// 3. [name = Pollux.DataAccess.UniqueId; Type = string; Data = {a string identifying the resource}] //sting identifying the resource uniquely
    /// 4. [name = Pollux.DataAccess.CustomDataType; Type = string; Data = {a string value}] //sting indicating a unique name for the custom data involved in custom access evaluation
    /// 5. [name = Pollux.DataAccess.CustomData; Type = object; Data = {an object}] //an object encapsulating data that needs to be present for policy evaluation to occur
    ///
    /// Note that 4 & 5 above is the only resource attribute added for instances where the <c>AuthorizeCustomAccess</c> function is called.
    /// </summary>
    public class DataAccessAuthorizer: IDataAccessAuthorizer
    {
        private readonly PolicyAuthority _authority;
        private readonly IAuthorizationContextProvider _authorizationContextProvider;
        private readonly IUserContext _userContext;
        private readonly IDataSerializer _serializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="userContext"></param>
        /// <param name="serializer"></param>
        /// <param name="authority"></param>
        public DataAccessAuthorizer(
            IAuthorizationContextProvider provider, 
            IUserContext userContext,
            IDataSerializer serializer,
            PolicyAuthority authority)
        {
            ThrowNullArguments(
                () => provider,
                () => userContext,
                () => serializer,
                () => authority);

            _authority = authority;
            _userContext = userContext;
            _authorizationContextProvider = provider;
            _serializer = serializer;
        }
        

        public Operation AuthorizeAccess(string dataType)
        => _AuthorizeAccess(dataType, null, null);

        public Operation AuthorizeAccess(string dataType, string uniqueId)
        => _AuthorizeAccess(dataType, null, uniqueId);

        public Operation AuthorizeAccess(string dataType, Guid ownerId)
        => _AuthorizeAccess(dataType, ownerId, null);

        public Operation AuthorizeAccess(string dataType, Guid ownerId, string uniqueId)
        => _AuthorizeAccess(dataType, ownerId, uniqueId);

        public Operation AuthorizeCustomAccess<TData>(TData data)
        where TData: class, ICustomAccessDataRoot => Operation.Try(async () =>
        {
            //validate the data
            await data
                .ThrowIfNull(new AuthorizationException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            //create the resource attributes
            var resourceAttributes = new List<IAttribute>
            {
                new Attribute(AttributeCategory.Resource)
                {
                    Name = Models.ResourceAttributes.DataAccessCustomDataType,
                    Type = CommonDataType.String,
                    Data = data.CustomDataType.ThrowIf(
                        string.IsNullOrWhiteSpace,
                        new AuthorizationException(Common.Exceptions.ErrorCodes.InvalidArgument))
                },
                new Attribute(AttributeCategory.Resource)
                {
                    Name = Models.ResourceAttributes.DataAccessCustomData,
                    Type = CommonDataType.JsonObject,
                    Data = await _serializer.SerializeData( //potentially slow 
                        typeof(TData), 
                        data.CompressObjectGraph()) 
                }
            };

            var context = await _authorizationContextProvider
                .ExecutionAuthorizationContext(resourceAttributes.ToArray());

            await _authority
                .Authorize(context)
                .Catch(exception =>
                {
                    if (exception.GetType() == typeof(Sigma.Exceptions.SigmaAccessDeniedException))
                        throw new AuthorizationException(ErrorCodes.AccessDeniedError);

                    //else propagate the original error
                    ExceptionDispatchInfo
                        .Capture(exception)
                        .Throw();
                });
        });

        private Operation _AuthorizeAccess(string dataType, Guid? ownerId, string uniqueId)
        => Operation.Try(async () =>
        {
            //create the resource attributes
            var resourceAttributes = new List<IAttribute>
            {
                new Attribute(AttributeCategory.Resource)
                {
                    Name = Models.ResourceAttributes.DataAccessDataType,
                    Type = CommonDataType.String,
                    Data = dataType.ThrowIf(
                        string.IsNullOrWhiteSpace,
                        new AuthorizationException(Common.Exceptions.ErrorCodes.InvalidArgument))
                }
            };
            if(ownerId != null) resourceAttributes.Add(new Attribute(AttributeCategory.Resource)
            {
                Name = Models.ResourceAttributes.DataAccessOwnerId,
                Type = CommonDataType.Guid,
                Data = ownerId.ToString()
            });
            if(!string.IsNullOrWhiteSpace(uniqueId)) resourceAttributes.Add(new Attribute(AttributeCategory.Resource)
            {
                Name = Models.ResourceAttributes.DataAccessUniqueId,
                Type = CommonDataType.Guid,
                Data = uniqueId
            });

            //first and foremost, the easiest test is if the current user is the owner of the data
            var currentUserId = await _userContext.CurrentUserId();
            if (currentUserId == ownerId)
                return; //<-- access granted

            var context = await _authorizationContextProvider
                .ExecutionAuthorizationContext(resourceAttributes.ToArray());

            await _authority
                .Authorize(context)
                .Catch(exception =>
                {
                    if (exception.GetType() == typeof(Sigma.Exceptions.SigmaAccessDeniedException))
                        throw new AuthorizationException(ErrorCodes.AccessDeniedError);

                    //else propagate the original error
                    ExceptionDispatchInfo
                        .Capture(exception)
                        .Throw();
                });
        });
    }
}
