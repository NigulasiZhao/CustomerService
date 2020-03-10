using Abp.AutoMapper;
using AfarsoftResourcePlan.Sessions.Dto;

namespace AfarsoftResourcePlan.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}
