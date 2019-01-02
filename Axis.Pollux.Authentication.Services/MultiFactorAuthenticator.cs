using System;
using Axis.Jupiter;
using Axis.Luna.Common.Utils;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Contracts;
using Axis.Pollux.Authentication.Contracts.Params;
using Axis.Pollux.Authentication.Exceptions;
using Axis.Pollux.Authentication.Models;
using Axis.Pollux.Authentication.Services.CommsMessages;
using Axis.Pollux.Authentication.Services.Queries;
using Axis.Pollux.Communication;
using Axis.Pollux.Communication.Contracts;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Models;
using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Authentication.Services
{
    public class MultiFactorAuthenticator: IMultiFactorAuthenticator
    {
        private readonly IMultiFactorQueries _queries;
        private readonly IMultiFactorConfigurationQueries _configQueries;
        private readonly StoreProvider _storeProvider;
        //private readonly IDataAccessAuthorizer _authorizer;
        private readonly IContactDataManager _contactManager;
        private readonly MessagingService _commsService;
        private readonly ISystemChannelSourceAddressProvider _channelSourceAddressProvider;

        public MultiFactorAuthenticator(
            StoreProvider storeProvider, //IDataAccessAuthorizer authorizer,
            IMultiFactorQueries queries, ICredentialHasher hasher,
            IContactDataManager contactDataManager, MessagingService commsService,
            ISystemChannelSourceAddressProvider channelSourceAddressProvider,
            IMultiFactorConfigurationQueries configurationQueries)
        {
            ThrowNullArguments(
                () => storeProvider,
                //() => authorizer,
                () => queries,
                () => hasher,
                () => contactDataManager,
                () => commsService,
                () => channelSourceAddressProvider,
                () => configurationQueries);

            _queries = queries;
            _storeProvider = storeProvider;
            //_authorizer = authorizer;
            _configQueries = configurationQueries;
            _contactManager = contactDataManager;
            _commsService = commsService;
            _channelSourceAddressProvider = channelSourceAddressProvider;
        }

        public Operation Authenticate(MultiFactorAuthenticationInfo info)
        {
            if (info == null)
                return Operation.Fail(new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidArgument));

            return info
                .Validate()
                .Then(async () =>
                {
                    switch (info.Step)
                    {
                        case MultiFactorStep.Request:
                            var token = await RequestMultiFactorAuthenticationToken(info);
                            throw new AuthenticationException(ErrorCodes.MultiFactorAuthenticationRequest, token);

                        case MultiFactorStep.Response:
                            return AuthenticateMultiFactorCredential(info);

                        default:
                            throw new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidContractParamState);
                    }
                });
        }

        public Operation<MultiFactorAuthenticationToken> RequestMultiFactorToken(Guid userId, string eventLabel, Guid contactId)
        => RequestMultiFactorAuthenticationToken(new MultiFactorAuthenticationInfo
        {
            UserId = userId,
            EventLabel = eventLabel,
            ContactDataId = contactId
        });

        public Operation<MultiFactorAuthenticationToken> RequestMultiFactorToken(Guid userId, string eventLabel, string contactChannel)
        => RequestMultiFactorAuthenticationToken(new MultiFactorAuthenticationInfo
        {
            UserId = userId,
            EventLabel = eventLabel,
            Channel = contactChannel
        });

        public Operation ValidateMultiFactorToken(Guid userId, string eventLabel, string tokenKey, string tokenValue)
        => AuthenticateMultiFactorCredential(new MultiFactorAuthenticationInfo(MultiFactorStep.Response)
        {
            UserId = userId,
            EventLabel = eventLabel,
            CredentialKey = tokenKey,
            CredentialToken = tokenValue
        });

        private Operation<MultiFactorAuthenticationToken> RequestMultiFactorAuthenticationToken(MultiFactorAuthenticationInfo info)
        => Operation.Try(async () =>
        {
            await info
                .ThrowIfNull(new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidContractParamState))
                .Validate();

            var credential = await _queries.GetActiveCredential(info.UserId, info.EventLabel);

            var config = await _configQueries
                .GetMultiFactorConfiguration(info.EventLabel)
                .ThrowIfNull(new AuthenticationException(ErrorCodes.UnmappedMultiFactorEventLabelConfiguration));

            await config.Validate(); //necessary?

            if (credential == null)
            {
                //create the credential
                credential = new MultiFactorCredential
                {
                    TargetUser = new User { Id = info.UserId },
                    CredentialKey = GenerateCredentialKey(),
                    CredentialToken = GenerateCredentialToken(),
                    ExpiresOn = DateTimeOffset.Now + config.ValidityDuration,
                    IsAuthenticated = false,
                    EventLabel = info.EventLabel
                };

                var storeCommand = _storeProvider
                    .CommandFor(typeof(MultiFactorCredential).FullName)
                    .ThrowIfNull(ErrorCodes.UnmappedStoreCommandModelType);

                await storeCommand
                    .Add(credential)
                    .ThrowIfNull(new AuthenticationException(ErrorCodes.InvalidStoreCommandResult));
            }

            #region  send the email/sms/etc message to the target user

            var contactData = info.ContactDataId != null
                ? (await _contactManager
                    .GetContactData(info.ContactDataId.Value))
                    .ThrowIfNull(new AuthenticationException(ErrorCodes.InvalidStoreCommandResult))
                : (await _contactManager
                    .GetPrimaryUserContact(info.UserId, info.Channel))
                    .ThrowIfNull(new AuthenticationException(ErrorCodes.InvalidStoreCommandResult));

            //get the system-mapped source address for this type of messages.
            var sourceAddress = (await _channelSourceAddressProvider
                .GetChannelSourceAddress(info.Channel, $"{Constants.MessagePayloadPrefix}{info.EventLabel}"))
                .ThrowIf(string.IsNullOrWhiteSpace, new AuthenticationException(Communication.Exceptions.ErrorCodes.InvalidChannelSourceAddress));

            //push the message to the user
            var @ref = await _commsService
                .PushToContact(sourceAddress, contactData, new MultiFactorAuthenticationRequestMessage
                {
                    PayloadType = $"{Constants.MessagePayloadPrefix}{info.EventLabel}",
                    Key =  credential.CredentialKey,
                    Token = credential.CredentialToken,
                    ExpiresOn = credential.ExpiresOn
                });
            #endregion

            //throw the necessary exception
            return new MultiFactorAuthenticationToken
            {
                UserId = credential.TargetUser.Id,
                CredentialKey = credential.CredentialKey,
                EventLabel = credential.EventLabel,
                AllowedCommunicationChannels = config.CommunicationChannels
            };
        });

        private Operation AuthenticateMultiFactorCredential(MultiFactorAuthenticationInfo info)
        => Operation.Try(async () =>
        {
            var credential = await _queries.GetActiveCredential(info.UserId, info.EventLabel);
            if (credential == null)
                throw new AuthenticationException(ErrorCodes.MultiFactorAuthenticationFailure);

            if(credential.ExpiresOn <= DateTimeOffset.Now)
                throw new AuthenticationException(ErrorCodes.MultiFactorAuthenticationFailure);

            if (!string.Equals(credential.CredentialKey, info.CredentialKey, StringComparison.InvariantCulture)
                || !string.Equals(credential.CredentialToken, info.CredentialToken, StringComparison.InvariantCulture))
                throw new AuthenticationException(ErrorCodes.MultiFactorAuthenticationFailure);
        });

        private static string GenerateCredentialKey()
        => new SecureRandom().Using(random => random.NextAlphaNumericString(50).ToUpper());

        private static string GenerateCredentialToken()
        => new SecureRandom().Using(random => random.NextAlphaNumericString(7).ToUpper());
    }
}
