using System;
using Axis.Luna.Common;
using Axis.Luna.Extensions;
using Axis.Pollux.Authorization.Exceptions;
using Axis.Sigma;

namespace Axis.Pollux.Authorization.Abac.Models
{
    public class Attribute: IAttribute
    {
        public Attribute()
        {
            Category = AttributeCategory.Subject;
        }

        public Attribute(AttributeCategory category)
        {
            Category = category;
        }

        public object Clone() => Copy();

        public IAttribute Copy() => new Attribute(Category)
        {
            Name = Name,
            Data = Data,
            Type = Type
        };

        /// <summary>
        /// should be removed!
        /// </summary>
        /// <returns></returns>
        public string DisplayData() => Data;

        public void Initialize(string[] tuple)
        {
            if (tuple == null || tuple.Length != 4)
                throw new AuthorizationException(Common.Exceptions.ErrorCodes.InvalidArgument);

            Name = tuple[0];

            Enum.TryParse(tuple[1], out AttributeCategory category)
                .ThrowIf(false, new AuthorizationException(Common.Exceptions.ErrorCodes.InvalidArgument));
            Category = category;

            Enum.TryParse(tuple[2], out CommonDataType commonDataType)
                .ThrowIf(false, new AuthorizationException(Common.Exceptions.ErrorCodes.InvalidArgument));
            Type = commonDataType;

            Data = tuple[3];
        }

        public string[] Tupulize() => new[]
        {
            Name,
            Category.ToString(),
            Type.ToString(),
            Data
        };

        public string Name { get; set; }
        public string Data { get; set; }
        public CommonDataType Type { get; set; }

        public AttributeCategory Category { get; private set; }
    }
}
