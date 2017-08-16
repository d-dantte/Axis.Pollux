using Axis.Luna.Operation;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Proteus;
using Axis.Nix;
using Axis.Rhea;

namespace Axis.Pollux.AOP.ServiceRegistry.Interceptors
{
    /// <summary>
    /// This interceptor SHOULD be at the end of the interceptor list because it goes ahead to pass on the call to a class it resolves from its resovler.
    /// Any other interceptors after it WILL NOT be called.
    /// </summary>
    /// <typeparam name="Impl"></typeparam>
    public class LazyServiceLoader<Impl> : IProxyInterceptor
    where Impl: class
    {
        private IServiceResolver _resolver;
        private Impl _impl;

        public LazyServiceLoader(IServiceResolver resolver)
        {
            ThrowNullArguments(() => resolver);

            _resolver = resolver;
        }

        public IOperation<object> Intercept(InvocationContext context)
        => LazyOp.Try(() =>
        {
            var target = _impl ?? (_impl = _resolver.Resolve<Impl>()); //load the implementation "on-demand"
            return target.Call(context.Method, context.Arguments);
        });
    }
}
