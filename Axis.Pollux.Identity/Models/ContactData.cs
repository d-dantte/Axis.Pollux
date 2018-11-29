using System;
using System.Collections.Generic;
using System.Text;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Identity.Models
{
    public class ContactData: BaseModel<Guid>
    {
        public string Data { get; set; }
        public string Channel { get; set; }

        public bool IsPrimary { get; set; }
        public bool IsVerified { get; set; }

        public int Status { get; set; }

        public User Owner { get; set; }
    }

    /// <summary>
    /// Presents a template for which Contact Channels can follow. Ultimately, this is left to be used and interpreted
    /// by the implementation
    /// </summary>
    public enum ContactChannel
    {
        Phone,
        Email,
        Fax,
        Address,
        Post
    }


    /// <summary>
    /// Possible status values for the user
    /// </summary>
    public enum ContactDataStatus
    {
        Archived = 0,
        Active = 1
    }
}
