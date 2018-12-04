using System;

namespace Axis.Pollux.Common.Exceptions
{
    public class CommonException: Exception, IExceptionContract
    {
        public string Code { get; }
        public object Info { get; }

        public CommonException(string code, object data = null, Exception innerException = null)
        : base(null, innerException)
        {
            Code = code;
            Info = data;
        }
    }
}
