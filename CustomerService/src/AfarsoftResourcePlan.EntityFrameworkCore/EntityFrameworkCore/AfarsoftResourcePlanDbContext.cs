using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using AfarsoftResourcePlan.Authorization.Roles;
using AfarsoftResourcePlan.Authorization.Users;
using AfarsoftResourcePlan.MultiTenancy;
using AfarsoftResourcePlan.CustomerService;
using AfarsoftResourcePlan.OauthLogin;

namespace AfarsoftResourcePlan.EntityFrameworkCore
{
    public class AfarsoftResourcePlanDbContext : AbpZeroDbContext<Tenant, Role, User, AfarsoftResourcePlanDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<ServiceConnectRecords> ServiceConnectRecords { get; set; }
        public DbSet<ChatRecords> ChatRecords { get; set; }
        public DbSet<CustomerConnectRecords> CustomerConnectRecords { get; set; }
        public DbSet<ServiceRecords> ServiceRecords { get; set; }
        public DbSet<OauthSetting> OauthSetting { get; set; }
        public AfarsoftResourcePlanDbContext(DbContextOptions<AfarsoftResourcePlanDbContext> options)
            : base(options)
        {
        }
    }
}
