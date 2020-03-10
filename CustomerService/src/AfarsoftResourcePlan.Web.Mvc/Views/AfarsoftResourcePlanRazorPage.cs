using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace AfarsoftResourcePlan.Web.Views
{
    public abstract class AfarsoftResourcePlanRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected AfarsoftResourcePlanRazorPage()
        {
            LocalizationSourceName = AfarsoftResourcePlanConsts.LocalizationSourceName;
        }
    }
}
