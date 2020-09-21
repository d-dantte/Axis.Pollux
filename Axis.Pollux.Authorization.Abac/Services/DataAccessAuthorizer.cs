using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Abac.Contracts;
using Axis.Pollux.Authorization.Abac.Models;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Authorization.Exceptions;
using Axis.Pollux.Authorization.Models;
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
    /// 1. [name = Pollux.DataAccess.DataType; Type = string; Data = {a string value}] //sting indicating a unique name for the custom data involved in access evaluation
    /// 2. [name = Pollux.DataAccess.Data; Type = object; Data = {an object}] //an object encapsulating data that needs to be present for policy evaluation to occur
    /// </summary>
    public class DataAccessAuthorizer: IDataAccessAuthorizer
    {
        private readonly PolicyAuthority _authority;
        private readonly IAuthorizationContextProvider _authorizationContextProvider;
        private readonly IUserContext _userContext;

        /// <summary>
        /// create a new instance
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="userContext"></param>
        /// <param name="serializer"></param>
        /// <param name="authority"></param>
        public DataAccessAuthorizer(
            IAuthorizationContextProvider provider, 
            IUserContext userContext,
            PolicyAuthority authority)
        {
            ThrowNullArguments(
                nameof(provider).ObjectPair(provider),
                nameof(userContext).ObjectPair(userContext),
                nameof(authority).ObjectPair(authority));

            _authority = authority;
            _userContext = userContext;
            _authorizationContextProvider = provider;
        }

        public Operation AuthorizeAccess(DataAccessDescriptor descriptor)
        => Operation.Try(async () =>
        {
            //validate the data
            descriptor.ThrowIfNull(new AuthorizationException(Common.Exceptions.ErrorCodes.InvalidArgument));

            //create the resource attributes
            var resourceAttributes = this
                .Intents(descriptor.Intent)
                .Select(intent => new Attribute(AttributeCategory.Intent)
                {
                    Type = Luna.Common.CommonDataType.String,
                    Name = IntentAttributeNames.DataAccessIntent,
                    Data = intent.ToString()
                })
                .AppendAt(0, new Attribute(AttributeCategory.Resource)
                {
                    Type = Luna.Common.CommonDataType.String,
                    Name = ResourceAttributeNames.DataAccessType,
                    Data = descriptor.DataType
                })
                .ToList();

            if (!string.IsNullOrWhiteSpace(descriptor.DataId))
                resourceAttributes.Add(new Attribute(AttributeCategory.Resource)
                {
                    Type = Luna.Common.CommonDataType.String,
                    Name = ResourceAttributeNames.DataAccessId,
                    Data = descriptor.DataId
                });

            var context = await _authorizationContextProvider
                .CaptureAuthorizationContext(resourceAttributes.ToArray());

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

        private DataAccessIntent[] Intents(DataAccessIntent flags)
        {
            return Enum
                .GetValues(typeof(DataAccessIntent))
                .Cast<DataAccessIntent>()
                .Where(flag => flags.HasFlag(flag))
                .ToArray();
        }
    }
}
