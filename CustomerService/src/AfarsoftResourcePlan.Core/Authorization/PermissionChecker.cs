using Abp.Authorization;
using AfarsoftResourcePlan.Authorization.Roles;
using AfarsoftResourcePlan.Authorization.Users;

namespace AfarsoftResourcePlan.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
