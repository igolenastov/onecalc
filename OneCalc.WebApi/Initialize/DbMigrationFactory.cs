

using System.Reflection;
#if DEBUG
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OneCalc.Infrastructure.DataAccess;

namespace OneCalc.WebApi.Initialize
{
    /// <summary>
    /// Для создания миграций
    /// </summary>
    public class DbMigrationFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true);

            var configuration = builder.Build();
            
            string connection = configuration.GetConnectionString("DefaultConnection");
            string dbContextAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name;

            optionsBuilder.UseNpgsql(connection, x => x.MigrationsAssembly(dbContextAssembly));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

#endif