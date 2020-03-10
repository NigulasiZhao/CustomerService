using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using AfarsoftResourcePlan.Configuration;
using AfarsoftResourcePlan.Web;

namespace AfarsoftResourcePlan.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class AfarsoftResourcePlanDbContextFactory : IDesignTimeDbContextFactory<AfarsoftResourcePlanDbContext>
    {
        public AfarsoftResourcePlanDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AfarsoftResourcePlanDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            AfarsoftResourcePlanDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AfarsoftResourcePlanConsts.ConnectionStringName));

            return new AfarsoftResourcePlanDbContext(builder.Options);
        }
    }
}
