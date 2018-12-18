using System;
using System.Collections.Generic;
using System.Text;
using Axis.Pollux.Common.Exceptions;

namespace Axis.Pollux.Logon.Exceptions
{
    public class LogonException : Exception, IExceptionContract
    {
        public string Code { get; }
        public object Info { get; }

        public LogonException(string code, object data = null, Exception innerException = null)
        : base(null, innerException)
        {
            Code = code;
            Info = data;
        }
    }
}
