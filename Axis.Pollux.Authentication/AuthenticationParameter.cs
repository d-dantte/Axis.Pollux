using System;
using System.Collections.Generic;

namespace Axis.Pollux.Authentication
{
    public class AuthenticationParameter
    {
        public Guid TransactionId { get; set; } = Guid.NewGuid();
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public List<KeyValuePair<string, string>> Parameters { get; } = new List<KeyValuePair<string, string>>();
    }
}
