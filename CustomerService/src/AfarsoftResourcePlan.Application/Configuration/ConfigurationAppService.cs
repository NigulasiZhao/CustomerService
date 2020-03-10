using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using AfarsoftResourcePlan.Configuration.Dto;

namespace AfarsoftResourcePlan.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : AfarsoftResourcePlanAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
