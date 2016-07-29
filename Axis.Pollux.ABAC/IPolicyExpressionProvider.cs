using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sigma = Axis.Sigma.Core;

namespace Axis.Pollux.ABAC
{
    public interface IPolicyExpressionProvider
    {
        Func<sigma.Subject, sigma.Action, sigma.Resource, bool> ToTargetExpression(string expression);
        Func<V, bool> ToAttributeExpression<V>(string expression) where V : sigma.IAttributeContainer;
    }
}
