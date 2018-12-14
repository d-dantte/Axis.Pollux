using System;
using System.Collections.Generic;
using System.Text;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Authentication.Models
{
    public enum CredentialVisibility
    {
        Secret,
        Public
    }

    /// <summary>
    /// A value used to determine if the credential enjoys uniqueness in various contexts.
    /// Uniqueness applies ONLY to credentials with "Secret" visibility.
    /// </summary>
    public enum Uniqueness
    {
        None,
        UserUnique,
        SystemUnique
    }

    public class Credential: BaseModel<Guid>
    {
        public CredentialVisibility Visibility { get; set; }
        public int Status { get; set; }

        /// <summary>
        /// Indicates that this credential's "data" should be unique across the system, given the "name" value
        /// </summary>
        public Uniqueness Uniqueness { get; set; }

        /// <summary>
        /// Credential Name (password, username, etc)
        /// </summary>
        public string Name { get; set; }
        public string Data { get; set; }
        public DateTimeOffset? ExpiresOn { get; set; }

        public User Owner { get; set; }

    }

    public enum CredentialStatus
    {
        Archived,
        Active,
    }
}
