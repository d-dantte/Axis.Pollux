using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Contracts.Params;

namespace Axis.Pollux.Authentication.Contracts
{
    public interface IMultiFactorAuthenticator
    {
        /// <summary>
        /// With a "Request-Step" info, a brand new MultiFactorCredential is created, and an AuthenticationException is thrown with the credential key,
        /// while the credential data is communicated to the Target User via one of the communications channel set up for such.
        /// Front end is then responsible for providing endpoints to collate both data (credential key and data), and present back to this service
        /// along with the original endpoint parameters, which will eventually have the necessary data to call this method and the authentication
        /// can happen.
        /// If a "Request-Step" is used and a MultiFactorCredential is already created, the message is resent to the user (provided some time has elapsed
        /// since the last request).
        /// If an expired Credential is expired, a new one is created and the message sent to the user.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Operation Authenticate(MultiFactorAuthenticationInfo info);
    }
}
