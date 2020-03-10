using System.Collections.Generic;
using AfarsoftResourcePlan.Roles.Dto;
using AfarsoftResourcePlan.Users.Dto;

namespace AfarsoftResourcePlan.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
