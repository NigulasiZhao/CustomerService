using Abp.AspNetCore.Mvc.ViewComponents;

namespace AfarsoftResourcePlan.Web.Views
{
    public abstract class AfarsoftResourcePlanViewComponent : AbpViewComponent
    {
        protected AfarsoftResourcePlanViewComponent()
        {
            LocalizationSourceName = AfarsoftResourcePlanConsts.LocalizationSourceName;
        }
    }
}
