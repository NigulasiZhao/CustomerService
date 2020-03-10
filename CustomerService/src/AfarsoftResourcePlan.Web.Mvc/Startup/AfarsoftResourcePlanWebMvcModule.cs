using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AfarsoftResourcePlan.Configuration;

namespace AfarsoftResourcePlan.Web.Startup
{
    [DependsOn(typeof(AfarsoftResourcePlanWebCoreModule))]
    public class AfarsoftResourcePlanWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AfarsoftResourcePlanWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            //Configuration.Navigation.Providers.Add<AfarsoftResourcePlanNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AfarsoftResourcePlanWebMvcModule).GetAssembly());
        }
    }
}
