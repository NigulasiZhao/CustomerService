using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace AfarsoftResourcePlan.Controllers
{
    public abstract class AfarsoftResourcePlanControllerBase: AbpController
    {
        protected AfarsoftResourcePlanControllerBase()
        {
            LocalizationSourceName = AfarsoftResourcePlanConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
