using Axis.Luna.Operation;
using Microsoft.Owin;
using Owin;
using System.Runtime.Remoting.Messaging;

namespace Axis.Pollux.Owin.Services.Impl
{
    public class CallContextOwinProvider: IOwinContextProvider
    {
        public IOperation<IOwinContext> Owin()
        => LazyOp.Try(() => CallContext.LogicalGetData(OwinContextProviderExtension.CallContextOwinKey) as IOwinContext);
    }

    public static class OwinContextProviderExtension
    {
        public static readonly string CallContextOwinKey = "Axis.Pollux.Owin.Services.CallContext.OwinContext";

        public static IAppBuilder UseCallContextOwinProvider(this IAppBuilder app)
        => app.Use(async (context, next) =>
        {
            try
            {
                CallContext.LogicalSetData(CallContextOwinKey, context);
                await next();
            }
            finally
            {
                CallContext.FreeNamedDataSlot(CallContextOwinKey);
            }
        });
    }
}
