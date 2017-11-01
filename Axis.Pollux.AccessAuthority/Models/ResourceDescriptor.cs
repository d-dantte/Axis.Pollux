using System;
using Axis.Luna.Operation;

namespace Axis.Pollux.AccessAuthority.Models
{
    public class ResourceDescriptor
    {
        public string Resource { get; set; }
        public string Intent { get; set; }
        public string Context { get; set; }

        public override string ToString()
        {
            var @string = $"{Resource}/{Intent}";
            if (!string.IsNullOrWhiteSpace(Context)) @string += $"/{Context}";

            return @string;
        }

        public IOperation Validate()
        => LazyOp.Try(() =>
        {
            if (string.IsNullOrWhiteSpace(Resource)) throw new Exception("Invalid Resource");
            else if (string.IsNullOrWhiteSpace(Intent)) throw new Exception("Invalid Intent");
        });
    }
}
