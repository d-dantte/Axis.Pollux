using System;
using Axis.Pollux.Common.Exceptions;

namespace Axis.Pollux.Identity.Exceptions
{
    public class IdentityException: Exception, IExceptionContract
    {
        public string Code { get; }
        public object Info { get; }

        public IdentityException(string code, object data = null, Exception innerException = null)
        : base(null, innerException)
        {
            Code = code;
            Info = data;
        }
    }
}
