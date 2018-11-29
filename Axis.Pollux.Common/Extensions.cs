using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Operation;

namespace Axis.Pollux.Common
{
    public static class Extensions
    {
        public static void Throw(this OperationError operationError) => throw new OperationException(operationError);
    }
}
