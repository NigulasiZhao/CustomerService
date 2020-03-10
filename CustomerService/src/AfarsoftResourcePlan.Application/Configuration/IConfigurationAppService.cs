using System.Threading.Tasks;
using AfarsoftResourcePlan.Configuration.Dto;

namespace AfarsoftResourcePlan.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
