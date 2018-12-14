using System;
using Axis.Pollux.Common.Exceptions;

namespace Axis.Pollux.Authentication.Exceptions
{
    public class AuthenticationException : Exception, IExceptionContract
    {
        public string Code { get; }
        public object Info { get; }

        public AuthenticationException(string code, object data = null, Exception innerException = null)
        : base(null, innerException)
        {
            Code = code;
            Info = data;
        }
    }
}
