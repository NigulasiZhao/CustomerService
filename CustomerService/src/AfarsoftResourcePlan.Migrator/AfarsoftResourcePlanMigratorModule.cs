using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AfarsoftResourcePlan.Configuration;
using AfarsoftResourcePlan.EntityFrameworkCore;
using AfarsoftResourcePlan.Migrator.DependencyInjection;

namespace AfarsoftResourcePlan.Migrator
{
    [DependsOn(typeof(AfarsoftResourcePlanEntityFrameworkModule))]
    public class AfarsoftResourcePlanMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AfarsoftResourcePlanMigratorModule(AfarsoftResourcePlanEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(AfarsoftResourcePlanMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                AfarsoftResourcePlanConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AfarsoftResourcePlanMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
