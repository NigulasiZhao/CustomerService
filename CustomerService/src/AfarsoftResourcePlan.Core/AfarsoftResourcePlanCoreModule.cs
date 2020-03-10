using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching;
using Abp.Runtime.Caching.Redis;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using AfarsoftResourcePlan.Authorization.Roles;
using AfarsoftResourcePlan.Authorization.Users;
using AfarsoftResourcePlan.Configuration;
using AfarsoftResourcePlan.Localization;
using AfarsoftResourcePlan.MultiTenancy;
using AfarsoftResourcePlan.Timing;
using Microsoft.Extensions.Configuration;
using System;

namespace AfarsoftResourcePlan
{
    [DependsOn(typeof(AbpZeroCoreModule), typeof(AbpRedisCacheModule))]
    public class AfarsoftResourcePlanCoreModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AfarsoftResourcePlanCoreModule()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //_env = env;
            //_appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
            var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (!string.IsNullOrEmpty(environmentName))
            {
                builder.AddJsonFile($"appsettings.{environmentName}.json", true, true);
            }
            _appConfiguration = builder.Build();
        }
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            AfarsoftResourcePlanLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = AfarsoftResourcePlanConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            #region  启用模块化Redis缓存
            IocManager.Register<ICacheManager, AbpRedisCacheManager>();
            Configuration.Caching.UseRedis(options =>
            {
                options.ConnectionString = _appConfiguration["Abp:RedisCache:ConnectionString"];
                options.DatabaseId = _appConfiguration.GetValue<int>("Abp:RedisCache:DatabaseId");
            });
            //设置所有缓存的默认过期时间
            Configuration.Caching.ConfigureAll(cache =>
            {
                //cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(2);
            });
            #endregion
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AfarsoftResourcePlanCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
