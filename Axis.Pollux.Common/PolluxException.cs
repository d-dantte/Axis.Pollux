using Axis.Luna.Extensions;
using System;

namespace Axis.Pollux.Common
{
    public class PolluxException: Exception
    {
        public PolluxException(string code, string message)
        {
            _code = code.ThrowIfNull();
            _message = message;
        }


        private string _message;
        private string _code;

        public override string Message => _message;
        public string Code => _code;
    }
}
