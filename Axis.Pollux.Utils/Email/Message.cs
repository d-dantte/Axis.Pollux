using Axis.Luna.Utils;
using System.Collections.Generic;

namespace Axis.Pollux.Common.Models
{
    public class Message
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
        public List<EncodedBinaryData> Attachments { get; private set; } = new List<EncodedBinaryData>();
    }
}
