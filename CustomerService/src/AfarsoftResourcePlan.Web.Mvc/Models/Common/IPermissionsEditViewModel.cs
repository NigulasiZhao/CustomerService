using System.Collections.Generic;
using AfarsoftResourcePlan.Roles.Dto;

namespace AfarsoftResourcePlan.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}