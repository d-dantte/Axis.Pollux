using Axis.Luna.Extensions;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Axis.Pollux.ABAC.DAS
{
    public static class Extensions
    {
        public static string UniqueSignature(this MethodInfo m)
        {
            return new StringBuilder()
                .Append(m.DeclaringType.MinimalAQName()).Append(".")
                .Append(!m.IsGenericMethod ? "" :
                        "<" + m.GetGenericArguments().Aggregate("", (@params, param) => @params += (@params == "" ? "" : ", ") + "[" + param.MinimalAQName() + "]") + ">")
                .Append("(")
                .Append(m.GetParameters()
                         .Aggregate("", (@params, param) => @params += (@params == "" ? "" : ", ") + "[" + param.ParameterType.MinimalAQName() + "] " + param.Name))
                .Append(")")
                .Append("::")
                .Append("[").Append(m.ReturnType.MinimalAQName()).Append("]")
                .ToString();
        }
    }
}
