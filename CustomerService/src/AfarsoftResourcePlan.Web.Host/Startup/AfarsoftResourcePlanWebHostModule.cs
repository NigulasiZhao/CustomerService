using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AfarsoftResourcePlan.Configuration;

namespace AfarsoftResourcePlan.Web.Host.Startup
{
    [DependsOn(
       typeof(AfarsoftResourcePlanWebCoreModule))]
    public class AfarsoftResourcePlanWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AfarsoftResourcePlanWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AfarsoftResourcePlanWebHostModule).GetAssembly());
        }
    }
}
