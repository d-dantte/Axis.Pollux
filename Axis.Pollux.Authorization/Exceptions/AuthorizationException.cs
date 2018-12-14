using System;
using Axis.Pollux.Common.Exceptions;

namespace Axis.Pollux.Authorization.Exceptions
{
    public class AuthorizationException: Exception, IExceptionContract
    {
        public string Code { get; }
        public object Info { get; }

        public AuthorizationException(string code, object data = null, Exception innerException = null)
        : base(null, innerException)
        {
            Code = code;
            Info = data;
        }
    }
}
