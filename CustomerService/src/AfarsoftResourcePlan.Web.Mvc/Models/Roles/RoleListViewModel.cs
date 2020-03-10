using System.Collections.Generic;
using AfarsoftResourcePlan.Roles.Dto;

namespace AfarsoftResourcePlan.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<RoleListDto> Roles { get; set; }

        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
