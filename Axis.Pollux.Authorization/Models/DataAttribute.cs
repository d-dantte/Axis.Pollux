using static Axis.Luna.Extensions.ExceptionExtension;

using Axis.Luna.Common;
using Axis.Luna.Common.Contracts;
using Axis.Luna.Operation;
using Axis.Pollux.Common;
using Axis.Pollux.Common.Models;
using System;

namespace Axis.Pollux.Authorization.Models
{
    public class DataAttribute : IDataItem, IValidatable
    {
        public string Name { get; set; }
        public string Data { get; set; }
        public CommonDataType Type { get; set; }

        public void Initialize(string[] tuple)
        {
            //assign temporary
            var temp = new DataAttribute
            {
                Type = tuple[0].ParseEnum<CommonDataType>(),
                Name = tuple[1],
                Data = tuple[2]
            };

            //validate
            temp.Validate()
                .Resolve();

            //assign permanent
            Name = temp.Name;
            Data = temp.Data;
            Type = temp.Type;
        }

        public string[] Tupulize() => new[]
        {
            Type.ToString(),
            Name,
            Data
        };

        public Operation Validate()
        => Operation.Try(() =>
        {
            Name.ThrowIf(
                string.IsNullOrWhiteSpace,
                new Exception("Invalid DataAttributeName"));

            Type.ThrowIf(
                IsNotDefined,
                new Exception($"Invalid DataType: {Type}"));
        });

        private static bool IsNotDefined(CommonDataType type) => !Enum.IsDefined(typeof(CommonDataType), type);
    }
}
