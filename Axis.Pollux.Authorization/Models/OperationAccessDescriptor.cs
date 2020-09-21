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
        private readonly CommonDataItem[] _parameterContext;

        public string OperationName { get; }

        public CommonDataItem[] ParameterContext => _parameterContext.ToArray(); //so the array cannot be altered externally

        public OperationAccessDescriptor(string name, params CommonDataItem[] parameters)
        {
            OperationName = name.ThrowIf(
                string.IsNullOrWhiteSpace, 
                new ArgumentException(nameof(name)));

            _parameterContext = parameters
                ?.ToArray()                 //duplicate the array
                ?? new CommonDataItem[0];   //or an empty array

            _parameterContext
                .Any(p => p == null)
                .ThrowIf(true, new ArgumentException("Invalid Parameter"));
        }
    }
}
