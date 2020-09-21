using static Axis.Luna.Extensions.ExceptionExtension;
using Attribute = Axis.Pollux.Authorization.Abac.Models.Attribute;

using Axis.Luna.Common;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Abac.Contracts;
using Axis.Pollux.Authorization.Abac.Models;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Authorization.Exceptions;
using Axis.Pollux.Authorization.Models;
using Axis.Pollux.Common.Contracts;
using Axis.Sigma;
using Axis.Sigma.Authority;
using System.Linq;

namespace Axis.Pollux.Authorization.Abac.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationAccessAuthorizer: IOperationAccessAuthorizer
    {
        private readonly PolicyAuthority _authority;
        private readonly IAuthorizationContextProvider _authorizationContextProvider;
        private readonly IDataSerializer _serializer;

        /// <summary>
        /// Create a new <c>OperationAccessAuthorizer</c>
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="serializer"></param>
        /// <param name="authority"></param>
        public OperationAccessAuthorizer(
            IAuthorizationContextProvider provider, 
            IDataSerializer serializer,
            PolicyAuthority authority)
        {
            ThrowNullArguments(
                nameof(provider).ObjectPair(provider),
                nameof(serializer).ObjectPair(serializer),
                nameof(authority).ObjectPair(authority));

            _authority = authority;
            _authorizationContextProvider = provider;
            _serializer = serializer;
        }

        public Operation AuthorizeAccess(OperationAccessDescriptor descriptor)
        => Operation.Try(async () =>
        {
            descriptor
                .ThrowIfNull(new AuthorizationException(Common.Exceptions.ErrorCodes.InvalidArgument));

            //convert descriptor to attributes
            var attributes = descriptor.ParameterContext
                .Select(item => new Attribute(AttributeCategory.Resource)
                {
                    Type = item.Type,
                    Name = $"{ResourceAttributeNames.OperationAccessParameterPrefix}.{item.Name}",
                    Data = item.Data
                })
                .AppendAt(0, new Attribute(AttributeCategory.Resource)
                {
                    Name = ResourceAttributeNames.OperationAccessName,
                    Type = CommonDataType.String,
                    Data = descriptor.OperationName,
                })
                .AppendAt(0, DefaultIntentAttributes.OperationIntent);
                        
            await _authorizationContextProvider
                .CaptureAuthorizationContext(attributes.ToArray()) //acquire authorization context
                .Then(r => _authority.Authorize(r))                //authorize
                .Catch(exception =>                                //convert exception if present
                {
                    if (exception is Sigma.Exceptions.SigmaAccessDeniedException)
                        throw new AuthorizationException(ErrorCodes.AccessDeniedError);
                });
        });
    }
}
