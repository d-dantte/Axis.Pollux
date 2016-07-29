using Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sigma = Axis.Sigma.Core;
using static Axis.Pollux.ABAC.PVGExpressions.Extensions;

namespace Axis.Pollux.ABAC.PVGExpressions
{
    public class PolicyExpressionProvider : IPolicyExpressionProvider
    {
        public Func<V, bool> ToAttributeExpression<V>(string expression) where V : sigma.IAttributeContainer
        {
            var cxt = CreateContext<V>();
            var bexp = new DynamicExpression(expression, ExpressionLanguage.Csharp).Bind(cxt);
            return v =>
            {
                cxt.SetValue(v);
                return (bool)bexp.Invoke(cxt);
            };
        }

        public Func<sigma.Subject, sigma.Action, sigma.Resource, bool> ToTargetExpression(string expression)
        {
            var cxt = new TargetContext();
            var bexp = new DynamicExpression(expression, ExpressionLanguage.Csharp).Bind(cxt);
            return (s, a, r) =>
            {
                cxt.action = a;
                cxt.resource = r;
                cxt.subject = s;
                return (bool)bexp.Invoke(cxt);
            };
        }
    }
}
