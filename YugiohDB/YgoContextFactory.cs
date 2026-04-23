using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using YGODeckBuilder.Data;

namespace YugiohDB
{
    // Used by `dotnet ef` tooling to construct YgoContext at design time.
    // Required because YgoContext no longer has an OnConfiguring fallback.
    public class YgoContextFactory : IDesignTimeDbContextFactory<YgoContext>
    {
        public YgoContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("YGODatabase")
                ?? throw new InvalidOperationException("YGODatabase connection string not found in appsettings.json.");

            var options = new DbContextOptionsBuilder<YgoContext>()
                .UseSqlServer(connectionString, sql => sql.MigrationsAssembly("YugiohDB"))
                .Options;

            return new YgoContext(options);
        }
    }
}
