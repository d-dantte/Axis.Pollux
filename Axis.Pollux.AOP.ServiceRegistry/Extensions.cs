using Axis.Nix;
using Axis.Proteus;
using Castle.DynamicProxy;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Axis.Pollux.AOP.ServiceRegistry
{
    public static class Extensions
    {
        private static ConcurrentDictionary<IServiceRegistrar, ProxyGenerator> GeneratorMap = new ConcurrentDictionary<IServiceRegistrar, ProxyGenerator>();

        public static IServiceRegistrar BindInterceptorFor<Service, Impl>(this IServiceRegistrar container, object registrarParam, params Func<IProxyInterceptor>[] proxyGenerators)
        where Service : class
        where Impl : class, Service
        {
            container.Register(() =>
            {
                //Each new service MUST have it's own InterceptorRoot, which subsequently has it's own interceptors created for it. What this means is that
                //each IProxyInterceptor instances, with respect to this AOP ServiceRegistry, should be treated as uniquely bound to the proxy that it is 
                //intercepting.
                var rootInterceptor = new InterceptorRoot(proxyGenerators.Select(_ => _.Invoke()).ToArray());
                return GeneratorMap
                    .GetOrAdd(container, _ => new ProxyGenerator())
                    .CreateInterfaceProxyWithoutTarget<Service>(rootInterceptor);
            }, 
            registrarParam);

            return container;
        }
    }
}
