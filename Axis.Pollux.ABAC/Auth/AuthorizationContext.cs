using Axis.Pollux.ABAC.Services;
using Axis.Sigma.Core;
using System.Collections.Generic;
using System.Linq;

namespace Axis.Pollux.ABAC.Auth
{
    /// <summary>
    /// </summary>
    public class AuthorizationContext : IAuthorizationContext
    {
        private List<IAttributeSource> _subjectAttributeSources = new List<IAttributeSource>();
        private List<IAttributeSource> _environmentAttributeSources = new List<IAttributeSource>();
        private List<IAttributeSource> _intentAttributeSources = new List<IAttributeSource>();


        public AuthorizationContext(IEnumerable<IAttributeSource> subjectSources,
                                    IEnumerable<IAttributeSource> intentSources,
                                    IEnumerable<IAttributeSource> environmentSources)
        {
            _subjectAttributeSources.AddRange(subjectSources ?? new IAttributeSource[0]);
            _intentAttributeSources.AddRange(intentSources ?? new IAttributeSource[0]);
            _environmentAttributeSources.AddRange(environmentSources ?? new IAttributeSource[0]);
        }

        public AuthorizationContext(IAttributeSource subjectSource, IAttributeSource intentSource, IAttributeSource environmenetSource)
        : this(new[] { subjectSource }, new[] { intentSource }, new[] { environmenetSource })
        {
        }

        public IEnumerable<IAttribute> EnvironmentAttributes()
        => _environmentAttributeSources.SelectMany(_source => _source.GetAttributes().Resolve());

        public IEnumerable<IAttribute> IntentAttributes()
        => _intentAttributeSources.SelectMany(_source => _source.GetAttributes().Resolve());

        public IEnumerable<IAttribute> SubjectAttributes()
        => _subjectAttributeSources.SelectMany(_source => _source.GetAttributes().Resolve());
    }
}
