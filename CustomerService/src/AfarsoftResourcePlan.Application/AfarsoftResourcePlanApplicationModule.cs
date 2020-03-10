using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using AfarsoftResourcePlan.Authorization;
using AfarsoftResourcePlan.Startup;

namespace AfarsoftResourcePlan
{
    [DependsOn(
        typeof(AfarsoftResourcePlanCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class AfarsoftResourcePlanApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AfarsoftResourcePlanAuthorizationProvider>();
            Configuration.Settings.Providers.Add<AbpZeroSettingProvider>();
            Configuration.Navigation.Providers.Add<AfarsoftResourcePlanNavigationProvider>();
            //Configuration.Authorization.Providers.Add<AfarsoftResourcePlanAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(AfarsoftResourcePlanApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
