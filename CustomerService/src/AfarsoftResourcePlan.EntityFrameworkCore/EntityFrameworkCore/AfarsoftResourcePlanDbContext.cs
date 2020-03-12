using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using AfarsoftResourcePlan.Authorization.Roles;
using AfarsoftResourcePlan.Authorization.Users;
using AfarsoftResourcePlan.MultiTenancy;
using AfarsoftResourcePlan.CustomerService;

namespace AfarsoftResourcePlan.EntityFrameworkCore
{
    public class AfarsoftResourcePlanDbContext : AbpZeroDbContext<Tenant, Role, User, AfarsoftResourcePlanDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<ServiceConnectRecords> ServiceConnectRecords { get; set; }
        public AfarsoftResourcePlanDbContext(DbContextOptions<AfarsoftResourcePlanDbContext> options)
            : base(options)
        {
        }
    }
}
