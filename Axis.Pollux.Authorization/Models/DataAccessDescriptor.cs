using static Axis.Luna.Extensions.ExceptionExtension;

using System;

namespace Axis.Pollux.Authorization.Models
{
    [Flags]
    public enum DataAccessIntent
    {
        Read = 1,
        Write = 2,
        Delete = 4
    }

    public class DataAccessDescriptor
    {
        public string DataType { get; }
        public string DataId { get; }
        public DataAccessIntent Intent { get; }

        public DataAccessDescriptor(string dataType, string dataId, DataAccessIntent intent)
        {
            DataType = dataType.ThrowIf(
                string.IsNullOrWhiteSpace,
                new ArgumentException(nameof(dataType)));

            DataId = dataId; //optional

            Intent = intent;
        }

        public DataAccessDescriptor(string dataType)
        : this(dataType, null, DataAccessIntent.Read)
        {
        }

        public DataAccessDescriptor(string dataType, DataAccessIntent intent)
        : this(dataType, null, intent)
        {
        }

        public DataAccessDescriptor(string dataType, string dataId)
        : this(dataType, dataId, DataAccessIntent.Read)
        {
        }
    }
}
