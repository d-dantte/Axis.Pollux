using System;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Exceptions;
using Axis.Pollux.Common.Models;
using Axis.Sigma.Policy;

namespace Axis.Pollux.Authorization.Abac.Models
{

    public class PolluxPolicy: Policy, IBaseModel<Guid>
    {
        public Operation Validate()
        => Operation.Try(() =>
        {
            if(string.IsNullOrWhiteSpace(Code))
                throw new AuthorizationException(Common.Exceptions.ErrorCodes.ModelValidationError);

            if (GovernedResources.Length == 0 || GovernedResources.Any(string.IsNullOrWhiteSpace))
                throw new AuthorizationException(Common.Exceptions.ErrorCodes.ModelValidationError);
        });

        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTime.Now;
        public DateTimeOffset? ModifiedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
