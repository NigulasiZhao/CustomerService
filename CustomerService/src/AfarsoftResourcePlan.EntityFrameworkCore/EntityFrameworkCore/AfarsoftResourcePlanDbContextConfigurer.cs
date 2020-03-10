using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AfarsoftResourcePlan.EntityFrameworkCore
{
    public static class AfarsoftResourcePlanDbContextConfigurer
    {
        public static readonly LoggerFactory MyLoggerFactory
= new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });
        public static void Configure(DbContextOptionsBuilder<AfarsoftResourcePlanDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString).UseLoggerFactory(MyLoggerFactory);
        }

        public static void Configure(DbContextOptionsBuilder<AfarsoftResourcePlanDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}
