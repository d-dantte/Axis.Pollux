namespace Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl.Queries
{
    public interface IUserRoleQuery
    {
        UserRole GetUserRole(string userId, string roleName);
    }
}
