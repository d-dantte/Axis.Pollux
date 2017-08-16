using System.Collections.Generic;

namespace Axis.Pollux.Common.Models.Email
{
    public class Payload
    {
        private List<string> _recipients = new List<string>();
        private List<string> _ccRecipients = new List<string>();

        public Message Message { get; set; }
        public string Sender { get; set; }

        public List<string> Recipients
        {
            get { return _recipients; }
            set
            {
                _recipients.Clear();
                if (value != null) _recipients.AddRange(value);
            }
        }

        public List<string> CcRecipients
        {
            get { return _ccRecipients; }
            set
            {
                _ccRecipients.Clear();
                if (value != null) _ccRecipients.AddRange(value);
            }
        }
    }
}
