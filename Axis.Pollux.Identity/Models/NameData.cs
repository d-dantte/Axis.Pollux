using System;
using System.Collections.Generic;
using System.Text;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Identity.Models
{
    public class NameData: BaseModel<Guid>, IUserOwned
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// Context-based String labels
        /// </summary>
        public string[] Tags { get; set; }

        public User Owner { get; set; }
    }

    public enum NameInfoStatus
    {
        Archived = 0,
        Active = 1
    }
}
