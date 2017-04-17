using Axis.Sigma.Core;
using System.Collections.Generic;
using System.Linq;
using Axis.Sigma.Core.Request;
using System.Reflection;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.AttributeAuthorization.Services
{
    public class OperationAuthorizationFactory
    {
        private IAttributeSource _subjects = null;
        private IAttributeSource _environment = null;

        private IntentMap _intentMap = null;


        public OperationAuthorizationFactory(IntentMap resourceMap, IAttributeSource subjectSource, IAttributeSource environmentSource)
        {
            ThrowNullArguments(() => resourceMap); //<-- for now, nulls are allowed for both attribute sources

            _intentMap = resourceMap;

            _subjects = subjectSource;
            _environment = environmentSource;
        }

        public IAuthorizationRequest OperationAuthorizationRequestFor(MethodInfo domainOperation)
        => new AuthRequest(_subjects, _environment, _intentMap.ResourceDescriptorsFor(domainOperation));



        public class AuthRequest : IAuthorizationRequest
        {
            private IAttributeSource _subjects = null;
            private AccessIntent[] _intents = null;
            private IAttributeSource _environment = null;

            internal AuthRequest(IAttributeSource subjectSource, IAttributeSource environmentSource, AccessIntent[] intents)
            {
                _subjects = subjectSource;
                _environment = environmentSource;
                _intents = intents;
            }

            public IEnumerable<IAttribute> SubjectAttributes() => _subjects.Attributes;

            public IEnumerable<IAttribute> EnvironmentAttributes() => _environment.Attributes;

            public IEnumerable<AccessIntent> Intents() => _intents.AsEnumerable();
        }
    }
}
