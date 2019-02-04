using Axis.Luna.Common;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Abac.Contracts;
using Axis.Pollux.Authorization.Abac.Models;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Authorization.Exceptions;
using Axis.Pollux.Common.Attributes;
using Axis.Sigma;
using Axis.Sigma.Authority;

using static Axis.Luna.Extensions.ExceptionExtension;
using Attribute = Axis.Pollux.Authorization.Abac.Models.Attribute;

namespace Axis.Pollux.Authorization.Abac.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationAccessAuthorizer: IOperationAccessAuthorizer
    {
        private readonly PolicyAuthority _authority;
        private readonly IAuthorizationContextProvider _authorizationContextProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="authority"></param>
        public OperationAccessAuthorizer(IAuthorizationContextProvider provider, PolicyAuthority authority)
        {
            ThrowNullArguments(
                () => provider,
                () => authority);

            _authority = authority;
            _authorizationContextProvider = provider;
        }

        public Operation AuthorizeAccess(ContractOperation descriptor)
        => Operation.Try(async () =>
        {
            await descriptor
                .ThrowIfNull(new AuthorizationException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            //create descriptor attribute
            var descriptorAttribute = new Attribute(AttributeCategory.Resource)
            {
                Name = ResourceAttributes.OperationAccessDescriptor,
                Data = descriptor.Name,
                Type = CommonDataType.String
            };

            //acquire authorization context
            var context = await _authorizationContextProvider
                .CaptureAuthorizationContext(descriptorAttribute);

            await _authority
                .Authorize(context)
                .Catch(exception =>
                {
                    if (exception.GetType() == typeof(Sigma.Exceptions.SigmaAccessDeniedException))
                        throw new AuthorizationException(ErrorCodes.AccessDeniedError);
                });
        });
    }
}
