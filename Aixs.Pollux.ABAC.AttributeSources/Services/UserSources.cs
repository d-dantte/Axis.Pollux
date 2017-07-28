using Axis.Pollux.ABAC.Services;
using System.Collections.Generic;
using Axis.Luna.Operation;
using Axis.Sigma.Core;
using Axis.Pollux.RoleAuth.Services;
using Axis.Pollux.ABAC.Auth;
using System.Linq;
using Axis.Pollux.Identity.Services;

using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.EnumerableExtensions;
using Axis.Luna.Utils;

namespace Axis.Pollux.ABAC.AttributeSources.Services
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
                return _urar.Page.Select(_ur => new SubjectAuthorizationAttribute
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
        private IUserManager _userManager;

        public UserBioSource(IUserManager manager)
        {
            ThrowNullArguments(() => manager);

            _userManager = manager;
        }

        public IOperation<IEnumerable<IAttribute>> GetAttributes()
        => LazyOp.Try(() =>
        {
            var bio = _userManager.GetBioData().Resolve();
            return Enumerate(
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.DateTime,
                    Name = Constants.SubjectAttribute_BioDOB,
                    Data = bio.Dob?.ToString(DataItemHelper.DefaultDateTimeFormat)
                },
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_BioFirstName,
                    Data = bio.FirstName
                },
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_BioGenderName,
                    Data = bio.Gender.ToString()
                },
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_BioLastName,
                    Data = bio.LastName
                },
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_BioMiddleName,
                    Data = bio.MiddleName
                },
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_BioNationality,
                    Data = bio.Nationality
                }
            );
        });
    }

    public class UserContactSource : IAttributeSource
    {
        private IUserManager _userManager;

        public UserContactSource(IUserManager userManager)
        {
            ThrowNullArguments(() => userManager);
            
            _userManager = userManager;
        }

        public IOperation<IEnumerable<IAttribute>> GetAttributes()
        => _userManager
            .GetContactData()
            .Then(_data =>
            {
                return _data.Page
                    .Select(_ud => new SubjectAuthorizationAttribute(_ud));
            });
    }

    public class UserAddressSource : IAttributeSource
    {
        private IUserManager _userManager;

        public UserAddressSource(IUserManager manager)
        {
            ThrowNullArguments(() => manager);

            _userManager = manager;
        }

        public IOperation<IEnumerable<IAttribute>> GetAttributes()
        => LazyOp.Try(() =>
        {
            var addresses = _userManager.GetAddresses(Identity.Principal.AddressStatus.Active).Resolve();
            return addresses.Page
                .SelectMany(_addr => Enumerate(
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_AddressCity,
                    Data = _addr.City
                },
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_AddressCountry,
                    Data = _addr.Country
                },
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_AddressStateProvince,
                    Data = _addr.StateProvince
                },
                new SubjectAuthorizationAttribute
                {
                    Type = CommonDataType.String,
                    Name = Constants.SubjectAttribute_AddressStreet,
                    Data = _addr.StateProvince
                }
            ));
        });
    }
}