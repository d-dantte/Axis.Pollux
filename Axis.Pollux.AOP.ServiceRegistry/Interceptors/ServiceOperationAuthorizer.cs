using System;
using Axis.Luna.Operation;
using Axis.Proteus;

using static Axis.Luna.Extensions.ExceptionExtensions;
using System.Reflection;
using Axis.Nix;

namespace Axis.Pollux.AOP.ServiceRegistry.Interceptors
{
    public class ServiceOperationAuthorizer : IProxyInterceptor
    {
        private IServiceResolver _resolver;
        private Func<IServiceResolver, MethodInfo, IOperation> _authorizer;

        public ServiceOperationAuthorizer(IServiceResolver resolver, Func<IServiceResolver, MethodInfo, IOperation> authorizer)
        {
            ThrowNullArguments(() => resolver, () => authorizer);
            
            _resolver = resolver;
            _authorizer = authorizer;
        }

        public IOperation<object> Intercept(InvocationContext context)
        {
            return _authorizer
                .Invoke(_resolver, context.Method)
                .Then(context.Next);
        }
    }
}
