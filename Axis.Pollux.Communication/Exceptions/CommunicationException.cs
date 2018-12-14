using System;
using Axis.Pollux.Common.Exceptions;

namespace Axis.Pollux.Communication.Exceptions
{
    public class CommunicationException: Exception, IExceptionContract
    {
        public string Code { get; }
        public object Info { get; }

        public CommunicationException(string code, object data = null, Exception innerException = null)
        : base(null, innerException)
        {
            Code = code;
            Info = data;
        }
    }
}
