using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using AfarsoftResourcePlan.EntityFrameworkCore.Seed;

namespace AfarsoftResourcePlan.EntityFrameworkCore
{
    [DependsOn(
        typeof(AfarsoftResourcePlanCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class AfarsoftResourcePlanEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<AfarsoftResourcePlanDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        AfarsoftResourcePlanDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        AfarsoftResourcePlanDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AfarsoftResourcePlanEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
