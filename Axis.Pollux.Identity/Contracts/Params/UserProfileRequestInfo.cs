﻿using System;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Exceptions;
using ErrorCodes = Axis.Pollux.Common.Exceptions.ErrorCodes;

namespace Axis.Pollux.Identity.Contracts.Params
{

    public class UserProfileRequestInfo : IValidatable
    {
        public Guid UserId { get; set; }
        public ArrayPageRequest AddressDataRequest { get; set; }
        public ArrayPageRequest ContactDataRequest { get; set; }
        public ArrayPageRequest NameDataRequest { get; set; }
        public ArrayPageRequest UserDataRequest { get; set; }


        public virtual Operation Validate() => Operation.Try(() =>
        {
            if (UserId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidContractParamState);
        });
    }
}
