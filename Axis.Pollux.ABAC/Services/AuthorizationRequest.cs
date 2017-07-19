using System.Collections.Generic;
using Axis.Sigma.Core;
using Axis.Sigma.Core.Request;
using System.Linq;

namespace Axis.Pollux.ABAC.Services
{
    public class AuthorizationRequest : IAuthorizationRequest
    {
        private List<IAttributeSource> _subjectAttributeSources = new List<IAttributeSource>();
        private List<IAttributeSource> _environmentAttributeSources = new List<IAttributeSource>();
        private List<IAttributeSource> _intentAttributeSources = new List<IAttributeSource>();


        public AuthorizationRequest(IEnumerable<IAttributeSource> subjectSources, 
                                    IEnumerable<IAttributeSource> intentSources,
                                    IEnumerable<IAttributeSource> environmentSources)
        {
            _subjectAttributeSources.AddRange(subjectSources ?? new IAttributeSource[0]);
            _intentAttributeSources.AddRange(intentSources ?? new IAttributeSource[0]);
            _environmentAttributeSources.AddRange(environmentSources ?? new IAttributeSource[0]);
        }

        public IEnumerable<IAttribute> EnvironmentAttributes()
        => _environmentAttributeSources.SelectMany(_source => _source.GetAttributes().Resolve());

        public IEnumerable<IAttribute> IntentAttributes()
        => _intentAttributeSources.SelectMany(_source => _source.GetAttributes().Resolve());

        public IEnumerable<IAttribute> SubjectAttributes()
        => _subjectAttributeSources.SelectMany(_source => _source.GetAttributes().Resolve());
    }
}
