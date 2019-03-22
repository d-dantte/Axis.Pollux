using Axis.Luna.Common;
using Axis.Luna.Common.Contracts;
using Axis.Luna.Extensions;
using System;

using Ext = Axis.Pollux.Common.Extensions;

namespace Axis.Pollux.Common.Models
{

    public class CommonDataItem : IDataItem
    {
        public string Name { get; set; }
        public string Data { get; set; }
        public CommonDataType Type { get; set; }

        public void Initialize(string[] tuple)
        {
            var argumentException = new ArgumentException("Invalid Tuple");

            if (tuple?.Length != 3)
                throw argumentException;

            Name = tuple[0]
                .ThrowIfNull(argumentException)
                .ThrowIf(string.IsNullOrWhiteSpace, argumentException);

            Type = tuple[1]
                .ThrowIfNull(argumentException)
                .ThrowIf(string.IsNullOrWhiteSpace, argumentException)
                .Pipe(Ext.ParseEnum<CommonDataType>);

            Data = tuple[2];
        }

        public string[] Tupulize() => new[]
        {
            Name,
            Type.ToString(),
            Data
        };
    }
}
