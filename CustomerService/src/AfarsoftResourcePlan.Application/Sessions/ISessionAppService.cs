using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Application.Services;
using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.Sessions.Dto;

namespace AfarsoftResourcePlan.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
        Task<BaseDataOutput<IList<UserMenuItem>>> GetAppNavigation();
        Task<BaseDataOutput<IList<UserMenuItem>>> GetPCNavigation();
        Task<BaseDataOutput<GetCurrentLoginInformationsOutput>> GetAppCurrentLoginInformation();
        //BaseDataOutput<Dictionary<string, string>> TestLoginData();
    }
}
