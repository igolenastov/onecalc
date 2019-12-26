using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OneCalc.Domain.Entities;
using OneCalc.Infrastructure.DataAccess;
using OneCalc.Infrastructure.DependencyInjection;

namespace OneCalc.Test.Unit
{
    public abstract class BaseTest
    {
        protected const long UserId = 1;

        private IServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CalcDatabase")
                .Options;

            var dbContext = new ApplicationDbContext(options);


            var services = new ServiceCollection().AddScoped<ApplicationDbContext>((a) => dbContext);

            services.AddCalcServices();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            await InitDataAsync();
        }

        protected virtual async Task InitDataAsync()
        {
            var db = GetService<ApplicationDbContext>();

            var user = new ApplicationUser
            {
                Id = UserId,
                Email = "testcalc@test.com"
            };

            db.Users.Add(user);

            await db.SaveChangesAsync();
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        protected T GetService<T>()
        {
            try
            {
                return _serviceProvider.GetService<T>();
            }
            catch (Exception e)
            {

            }

            return default(T);
        }
    }
}
