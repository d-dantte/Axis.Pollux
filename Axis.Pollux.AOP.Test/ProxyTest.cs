using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.AOP.Test
{
    [TestClass]
    public class ProxyTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var gen = new ProxyGenerator();
            var proxy = gen.CreateInterfaceProxyWithoutTarget<IXtacy>(new Interceptor());

            var r1 = proxy.Method1();
            proxy.Method2(r1);
        }
    }

    public class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
        }
    }

    public interface IXtacy
    {
        int Method1();
        void Method2(int x);
    }

    public class Xtacy : IXtacy
    {
        public int Method1() => new Random(Guid.NewGuid().GetHashCode()).Next();

        public void Method2(int x)
        {
        }
    }
}
