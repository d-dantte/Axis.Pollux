using Axis.Pollux.Identity;
using Axis.Pollux.Identity.Principal;
using Newtonsoft.Json;
using System;

namespace Axis.Pollux.Notification.Models
{
    public class Notification: Common.PolluxModel<long>, IUserTargeted
    {
        public Guid UUId { get; set; } = Guid.NewGuid();

        public User Target { get; set; }

        public string Origin { get; set; }

        public string[] Channels { get; set; }

        public string Data { get; set; }

        public string Title { get; set; }

        public NotificationStatus Status { get; set; }
    }


    public class Notification<D> : Notification
    {
        private D _data;
        new public D Data
        {
            get { return _data; }
            set
            {
                _data = value;
                if (value == null) base.Data = null;
                else base.Data = JsonConvert.SerializeObject(value);
            }
        }
    }
}
