using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.RBAC.Exceptions
{
    public class AuthorizationException: Exception
    {
        public AuthorizationException()
        {
        }
        public AuthorizationException(string message)
        :this(message, null)
        {
        }
        public AuthorizationException(string message, Exception inner)
        :base(message, inner)
        {
        }
    }
}
