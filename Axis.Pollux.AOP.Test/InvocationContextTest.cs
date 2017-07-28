using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Axis.Luna.Extensions.EnumerableExtensions;
using Axis.Luna.Operation;
using System.Linq;
using Axis.Nix;

namespace Axis.Pollux.AOP.Test
{
    [TestClass]
    public class InvocationContextTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var interceptors = new[] { new FakeInterceptor(), new FakeInterceptor(), new FakeInterceptor(), new FakeInterceptor() };
            Action x = X;
            var cxt = new InvocationContext(x.Method, null, null, null, interceptors);
            var g = cxt.Next();
            g.Resolve();
        }

        public void X()
        { }
    }

    public class SomeType
    {

    }

    public class FakeInterceptor : IProxyInterceptor
    {
        public IOperation<object> Intercept(InvocationContext context)
        => LazyOp.Try(() =>
        {
            //pre-invocation advice
            {
            }

            return context.Next?
                .Invoke()

                //post-invocation advice
                .Then(r => 
                {
                    Console.WriteLine($"succeeded: {r}");
                    return (object)"bleh";
                },

                //faulted-invocation advice                           
                err => 
                {
                    Console.WriteLine("failed");
                })

                //This is the last interceptor, so handle it here
                ?? LazyOp.Try(() =>
                {
                    return (object)6;
                });
        });
    }
}
