using System.Collections.Generic;
using Axis.Sigma.Core;
using Axis.Sigma.Core.Request;
using System.Linq;

namespace Axis.Pollux.ABAC.Services
{
    public class AuthorizationRequest : IAuthorizationRequest
    {
        private List<IUserAttributeSource> _userAttributeSources = new List<IUserAttributeSource>();
        private List<IEnvironmentAttributeSource> _environmentAttributeSources = new List<IEnvironmentAttributeSource>();
        private List<IIntentAttributeSource> _intentAttributeSources = new List<IIntentAttributeSource>();


        public AuthorizationRequest(IEnumerable<IUserAttributeSource> userSources, 
                                    IEnumerable<IIntentAttributeSource> intentSources,
                                    IEnumerable<IEnvironmentAttributeSource> environmentSources)
        {
            _userAttributeSources.AddRange(userSources ?? new IUserAttributeSource[0]);
            _intentAttributeSources.AddRange(intentSources ?? new IIntentAttributeSource[0]);
            _environmentAttributeSources.AddRange(environmentSources ?? new IEnvironmentAttributeSource[0]);
        }

        public IEnumerable<IAttribute> EnvironmentAttributes()
        => _environmentAttributeSources.SelectMany(_source => _source.GetAttributes().Resolve());

        public IEnumerable<IAttribute> IntentAttributes()
        => _intentAttributeSources.SelectMany(_source => _source.GetAttributes().Resolve());

        public IEnumerable<IAttribute> SubjectAttributes()
        => _userAttributeSources.SelectMany(_source => _source.GetAttributes().Resolve());
    }
}
