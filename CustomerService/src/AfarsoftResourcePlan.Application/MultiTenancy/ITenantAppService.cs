using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AfarsoftResourcePlan.MultiTenancy.Dto;

namespace AfarsoftResourcePlan.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

