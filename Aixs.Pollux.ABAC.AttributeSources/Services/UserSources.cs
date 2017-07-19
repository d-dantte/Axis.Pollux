using Axis.Pollux.ABAC.Services;
using System.Collections.Generic;
using Axis.Luna.Operation;
using Axis.Sigma.Core;
using Axis.Pollux.RoleAuth.Services;
using Axis.Pollux.Common.Services;
using Axis.Pollux.ABAC.Auth;
using System.Linq;
using System;
using Axis.Pollux.Identity.Services;

using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.EnumerableExtensions;

namespace Axis.Pollux.ABAC.DAS.Services
{
    public class UserRoleSource : IAttributeSource
    {
        private IRoleManager _roleManager;
        private IUserContext _userContext;

        public UserRoleSource(IRoleManager manager, IUserContext userContext)
        {
            ThrowNullArguments(() => manager,
                               () => userContext);

            _roleManager = manager;
            _userContext = userContext;
        }

        public IOperation<IEnumerable<IAttribute>> GetAttributes()
        => _roleManager
            .GetUserRolesFor(_userContext.User())
            .Then(_urar =>
            {
                return _urar.Select(_ur => new SubjectAuthorizationAttribute
                {
                    Type = Luna.Utils.CommonDataType.String,
                    Name = Constants.SubjectAttribute_UserRole,
                    Data = _ur.Role.RoleName
                });
            });
    }


    public class UserIdentitySource : IAttributeSource
    {
        private IUserContext _userContext;

        public UserIdentitySource(IUserContext userContext)
        {
            ThrowNullArguments(() => userContext);
            
            _userContext = userContext;
        }

        public IOperation<IEnumerable<IAttribute>> GetAttributes()
        => LazyOp.Try(() =>
        {
            var user = _userContext.User();
            return Enumerate(
                new SubjectAuthorizationAttribute
                {
                    Type = Luna.Utils.CommonDataType.String,
                    Name = Constants.SubjectAttribute_UserName,
                    Data = user.UserId
                },
                new SubjectAuthorizationAttribute
                {
                    Type = Luna.Utils.CommonDataType.Guid,
                    Name = Constants.SubjectAttribute_UserUUID,
                    Data = user.UId.ToString()
                }
            );
        });
    }

    public class UserBioSource: IAttributeSource
    {

        private IUserContext _userContext;
        private IUserManager _userManager;

        public UserBioSource(IUserManager manager, IUserContext userContext)
        {
            ThrowNullArguments(() => manager,
                               () => userContext);

            _userManager = manager;
            _userContext = userContext;
        }

        public IOperation<IEnumerable<IAttribute>> GetAttributes()
        => LazyOp.Try(() =>
        {
            var bio = _userManager.GetBioData();
            return Enumerate(
                new SubjectAuthorizationAttribute
                {
                    Type = Luna.Utils.CommonDataType.String,
                    Name = Constants
                }
            );
        });
    }
}
