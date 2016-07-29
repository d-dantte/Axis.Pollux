
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sigma = Axis.Sigma.Core;

namespace Axis.Pollux.ABAC.PVGExpressions
{
    public static class Extensions
    {
        public static AttributeExpressionContext<E> CreateContext<E>()
        where E : sigma.IAttributeContainer
        {
            var et = typeof(E);
            if (et == typeof(sigma.Action)) return (dynamic) new ActionContext();
            else if (et == typeof(sigma.Environment)) return (dynamic) new EnvironmentContext();
            else if (et == typeof(sigma.Resource)) return (dynamic)new ResourceContext();
            else if (et == typeof(sigma.Subject)) return (dynamic)new SubjectContext();
            else throw new Exception($"Unrecognzed AttributeContainerType: {et.FullName}");
        }
    }
}
