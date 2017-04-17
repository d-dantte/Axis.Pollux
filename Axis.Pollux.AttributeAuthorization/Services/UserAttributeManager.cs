using Axis.Pollux.ABAC.Services;
using System;
using System.Collections.Generic;
using Axis.Luna;
using Axis.Pollux.ABAC.Auth;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core;
using Axis.Pollux.ABAC.Query;
using Axis.Pollux.Common.Services;
using Axis.Jupiter.Kore.Command;
using Axis.Luna.Extensions;
using System.Linq;

using static Axis.Luna.Extensions.ObjectExtensions;
using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.AttributeAuthorization.Services
{
    public class UserAttributeManager : IUserAttributeManager
    {
        private IUserAttributeQuery _query;
        private IUserContext _userContext;
        private IPersistenceCommands _pcommand;

        public UserAttributeManager(IUserAttributeQuery query, IUserContext userContext, IPersistenceCommands pcommand)
        {
            ThrowNullArguments(() => query,
                               () => userContext,
                               () => pcommand);

            _query = query;
            _userContext = userContext;
            _pcommand = pcommand;
        }


        public Operation<IAttribute> AddAttribute(SubjectAuthorizationAttribute attribute) => AssignAttribute(_userContext.User(), attribute);

        public Operation<IAttribute> AssignAttribute(User user, SubjectAuthorizationAttribute attribute)
        => Operation.Try(() =>
        {
            return attribute
                .With(new { Name = NormalizeAttributeName(attribute.Name) })
                .ToUserData(user)
                .Pipe(_pcommand.Add)
                .Then(opr => opr.Result.ToSubjectAttribute().As<IAttribute>());
        });

        public Operation<IEnumerable<IAttribute>> GetAttributes() => GetAttributesFor(_userContext.User());

        public Operation<IEnumerable<IAttribute>> GetAttributesFor(User user)
        => Operation.Try(() => _query.GetUserAttributes(user)
                                     .Select(_ud => _ud.ToSubjectAttribute().As<IAttribute>()));

        public Operation<IAttribute> RemoveAttribute(string attributeName)
        => Operation.Try(() =>
        {
            return _query
                .GetUserAttribute(_userContext.User(), NormalizeAttributeName(attributeName))
                .ThrowIfNull()
                .Pipe(_pcommand.Delete)
                .Then(opr => opr.Result.ToSubjectAttribute().As<IAttribute>());
        });

        public Operation<IAttribute> UpdateAttribute(SubjectAuthorizationAttribute attribute)
        => Operation.Try(() =>
        {
            return attribute
                .With(new { Name = NormalizeAttributeName(attribute.Name) })
                .ToUserData(_userContext.User())
                .Pipe(_pcommand.Update)
                .Then(opr => opr.Result.ToSubjectAttribute().As<IAttribute>());
        });

        private string NormalizeAttributeName(string name)
        => !name.ThrowIf(string.IsNullOrWhiteSpace, new Exception("invalid attribute name"))
               .StartsWith(Constants.Misc_UserAttributeNamePrefix) ?
               $"{Constants.Misc_UserAttributeNamePrefix}{name}" :
               name;
    }
}
