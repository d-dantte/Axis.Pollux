﻿using System;
using System.Linq;
using Axis.Luna.Common;
using Axis.Luna.Extensions;
using Axis.Pollux.Authorization.Exceptions;
using Axis.Sigma;

using static Axis.Luna.Extensions.Common;

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

        public override string ToString() => Tupulize().Select(Encode).JoinUsing(" | ");
        public override bool Equals(object obj)
        {
            return obj is Attribute att
                && att.Data == Data
                && att.Name == Name
                && att.Type == Type
                && att.Category == Category;
        }
        public override int GetHashCode() => ValueHash(Category, Type, Name, Data);


        public static Attribute Parse(string attribute)
        {
            var parts = attribute
                .Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(Decode)
                .ToArray();

            var result = new Attribute();
            result.Initialize(parts);
            return result;
        }
        public static bool TryParse(string @string, out Attribute attribute )
        {
            try
            {
                attribute = Parse(@string);
                return true;
            }
            catch
            {
                attribute = null;
                return false;
            }
        }

        private static string Encode(string value) => value?.Replace("@", "@at;").Replace("|", "@bar;");
        private static string Decode(string value) => value?.Replace("@bar;", "|").Replace("@at;", "@");

        public static bool operator== (Attribute first, Attribute second)
        {
            return (first == null && second == null)
                || (first?.Equals(second) ?? false);
        }
        public static bool operator!= (Attribute first, Attribute second) => !(first == second);
    }
}
