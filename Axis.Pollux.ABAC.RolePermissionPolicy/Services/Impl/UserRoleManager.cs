using Axis.Pollux.ABAC.Services;
using System;
using Axis.Luna;
using Axis.Pollux.ABAC.Auth;
using Axis.Sigma.Core;
using Axis.Jupiter.Kore.Command;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl.Queries;
using Axis.Luna.Extensions;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl
{
    /// <summary>
    /// Attribute manager that deals STRICTLY with UserRole attributes ONLY.
    /// </summary>
    public class UserRoleManager : IUserAttributeManager
    {
        private IPersistenceCommands _pcommand;
        private IUserRoleQuery _query;

        public UserRoleManager(IPersistenceCommands persistenceCommand, IUserRoleQuery query)
        {
            ThrowNullArguments(() => persistenceCommand, () => query);

            _pcommand = persistenceCommand;
            _query = query;
        }


        public Operation<IAttribute> AssignAttribute(string userId, SubjectAuthorizationAttribute attribute)
        => Operation.Try(() =>
        {
            if (!attribute.Name.Equals(CommonAttributeNames.SubjectAttribute_UserRole)) throw new Exception("Invalid Subject Attribute supplied");

            return _query.GetUserRole(userId, attribute.ResolveData<string>()) != null ?
                   attribute.As<IAttribute>() :
                   _pcommand.Add(new UserRole { UserId = userId, RoleName = attribute.ResolveData<string>() })
                            .Then(opr => attribute)
                            .Resolve();
        });

        public Operation<IAttribute> RemoveAttribute(string userId, string attributeName)
        => Operation.Try(() =>
        {
            attributeName.ThrowIf(_n => !CommonAttributeNames.SubjectAttribute_UserRole.Equals(attributeName), "Invalid Subject Attribute name supplied");
            var userRole = _query.GetUserRole(userId, attributeName).ThrowIfNull("Subject Attribute does not exist");
            return _pcommand.Delete(userRole).Then(opr => new SubjectAuthorizationAttribute
            {
                Name = CommonAttributeNames.SubjectAttribute_UserRole,
                Data = attributeName,
                Type = CommonDataType.String
            })
            .As<IAttribute>();
        });
    }
}
