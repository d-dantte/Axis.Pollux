using Axis.Luna.Operation;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Exceptions;
using System.Text.RegularExpressions;

namespace Axis.Pollux.Identity.Services
{
    public class SimpleUserNameValidator : IUserNameValidator
    {
        private static readonly Regex SIMPLE_USERNAME = new Regex("^\\w+$");

        public Operation<string> ValidateUsername(string userName)
        => Operation.Try(() =>
        {
            if (!SIMPLE_USERNAME.IsMatch(userName))
                throw new IdentityException(Common.Exceptions.ErrorCodes.GeneralError);

            else return userName;
        });
    }
}
