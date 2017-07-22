using Axis.Pollux.Authentication.Models;
using Axis.Pollux.Common;
using Axis.Pollux.Identity.OAModule.Entities;
using System;

namespace Axis.Pollux.Authentication.OAModule.Entities
{
    public class CredentialEntity: PolluxEntity<long>
    {
        public CredentialMetadataEntity Metadata { get; set; } = new CredentialMetadataEntity(CredentialMetadata.Password);

        public byte[] Value { get; set; }

        public string SecuredHash { get; set; } //hash of data if required is kept here

        public DateTime? ExpiresOn { get; set; }

        /// <summary>
        /// inline css formatted name/value pairs
        /// </summary>
        public string Tags { get; set; }


        public UserEntity Owner { get; set; }
        public string OwnerId { get; set; }

        public CredentialEntity()
        { }
    }

    public class CredentialMetadataEntity
    {
        public CredentialMetadataEntity(CredentialMetadata metadata)
        {
            Name = metadata.Name;
            Access = metadata.Access;
        }
        public CredentialMetadataEntity()
        {
        }

        #region properties
        public string Name { get; set; }
        public Access Access { get; set; }
        #endregion
    }
}
