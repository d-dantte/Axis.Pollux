using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Operation;

namespace Axis.Pollux.Common
{
    public static class Extensions
    {
        public static void Throw(this OperationError operationError) => throw new OperationException(operationError);

        public static Enm ParseEnum<Enm>(this string @string)
        where Enm: struct
        {
            if (!Enum.TryParse<Enm>(@string, out var enm))
                throw new Exception("Invalid Enum Conversion");

            else
                return enm;
        }

        public static bool IsNull(this object value) => value == null;
        public static bool IsNotNull(this object value) => value != null;
    }
}
