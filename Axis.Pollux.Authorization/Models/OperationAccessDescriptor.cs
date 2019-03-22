using Axis.Luna.Extensions;
using Axis.Pollux.Common.Models;
using System;
using System.Linq;

namespace Axis.Pollux.Authorization.Models
{
    /// <summary>
    /// Operation access authorization uses this as a basis to identify operation for whom access is being verified
    /// </summary>
    public class OperationAccessDescriptor
    {
        public string OperationName { get; }

        public CommonDataItem[] ParameterContext { get; }

        public OperationAccessDescriptor(string name, params CommonDataItem[] parameters)
        {
            OperationName = name.ThrowIf(
                string.IsNullOrWhiteSpace, 
                new ArgumentException(nameof(name)));

            ParameterContext = parameters
                ?.ToArray()                 //duplicate the array
                ?? new CommonDataItem[0];   //or an empty array
        }
    }
}
