using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Runtime.Session;
using AfarsoftResourcePlan.Common;
using AfarsoftResourcePlan.Sessions.Dto;

namespace AfarsoftResourcePlan.Sessions
{
    public class SessionAppService : AfarsoftResourcePlanAppServiceBase, ISessionAppService
    {
        private readonly IUserNavigationManager _userNavigationManager;
        public SessionAppService(IUserNavigationManager userNavigationManager)
        {
            _userNavigationManager = userNavigationManager;
        }
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return output;
        }
        public async Task<BaseDataOutput<GetCurrentLoginInformationsOutput>> GetAppCurrentLoginInformation()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return new BaseDataOutput<GetCurrentLoginInformationsOutput> { Data = output };
        }
        [AbpAuthorize]
        public async Task<BaseDataOutput<IList<UserMenuItem>>> GetAppNavigation()
        {
            var menu = await _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier());
            var items = menu.Items.Where(e => e.Target == "app").FirstOrDefault().Items;
            return new BaseDataOutput<IList<UserMenuItem>> { Data = items };
        }
        public async Task<BaseDataOutput<IList<UserMenuItem>>> GetPCNavigation()
        {
            var menu = await _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier());
            var items = menu.Items.Where(e => e.Target == "pc" || string.IsNullOrEmpty(e.Target)).ToList();
            return new BaseDataOutput<IList<UserMenuItem>> { Data = items };
        }
        //public BaseDataOutput<Dictionary<string, string>> TestLoginData()
        //{
        //    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        //    try
        //    {
        //        keyValuePairs.Add("AreaId", AbpSession.GetAreaIdRealation());
        //    }
        //    catch (System.Exception ex)
        //    {

        //        return new BaseDataOutput<Dictionary<string, string>> { Code = 1, Message = ex.Message };
        //    }
        //    return new BaseDataOutput<Dictionary<string, string>> { Data = keyValuePairs };
        //}
    }
}
