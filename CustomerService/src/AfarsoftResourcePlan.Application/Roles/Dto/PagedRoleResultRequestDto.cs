using Abp.Application.Services.Dto;

namespace AfarsoftResourcePlan.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

