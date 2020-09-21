using Axis.Luna.Operation;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IUserNameValidator
    {
        Operation<string> ValidateUsername(string userName);
    }
}
