using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Sigma.Core;
using System.Collections.Generic;
using Axis.Pollux.Common.Services;
using System.Linq;

namespace Axis.Pollux.AttributeAuthorization.Services
{
    public class UserContextAttributeSource : IAttributeSource
    {
        
        private IUserContext _userContext;

        private List<IAttribute> _attributes;


        public AttributeCategory SourceCategory => AttributeCategory.Subject;

        public IEnumerable<IAttribute> Attributes
        => _attributes?.ToArray() ?? (_attributes = RetrieveAttributes().ToList()).ToArray();

        public IAttribute Attribute(string name) => _attributes.FirstOrDefault(_att => _att.Name == name);

        public string Value(string name) => Attribute(name).Data;

        public V Value<V>(string name) => Attribute(name).ResolveData<V>();

        private IEnumerable<IAttribute> RetrieveAttributes()
        => _userContext
            .UserAttributes()
            .Where(_di => _di.Name.StartsWith(Constants.Misc_UserAttributeNamePrefix))
            .Select(_di => _di.ToSubjectAttribute());


        public UserContextAttributeSource(IUserContext userContext)
        {
            ThrowNullArguments(() => userContext);
            
            _userContext = userContext;
        }
    }
}
