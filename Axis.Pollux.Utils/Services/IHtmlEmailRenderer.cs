using Axis.Luna.Operation;
using System.Collections.Generic;

namespace Axis.Pollux.Utils.Services
{
    public interface IHtmlEmailRenderer
    {
        IOperation<string> RenderHtml(IDictionary<string, object> emailModel);
    }
}
