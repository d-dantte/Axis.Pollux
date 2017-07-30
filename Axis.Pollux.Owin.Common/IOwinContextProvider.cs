using Axis.Luna.Operation;
using Microsoft.Owin;

namespace Axis.Pollux.Owin.Services
{
    public interface IOwinContextProvider
    {
        IOperation<IOwinContext> Owin();
    }

}
