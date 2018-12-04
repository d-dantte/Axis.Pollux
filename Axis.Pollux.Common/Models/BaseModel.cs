using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Exceptions;

namespace Axis.Pollux.Common.Models
{
    public abstract class BaseModel<Key>: IValidatable
    {

        public Key Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? ModifiedOn { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }


        public virtual Operation Validate() => Operation.Try(() =>
        {
            var context = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(this, context, results);

            if (isValid)
                return;

            else
                throw new CommonException(ErrorCodes.ModelValidationError, results);
        });
    }
}
