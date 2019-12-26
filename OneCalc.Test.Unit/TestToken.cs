using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OneCalc.Domain.AppSettings;
using OneCalc.Domain.Services;
using OneCalc.Infrastructure.DataAccess;
using System.Threading.Tasks;

namespace OneCalc.Test.Unit
{
    public class TestToken : BaseTest
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            var opt = new JwtSetting {SecretKey = "this is my custom Secret key for authnetication" };
            var mock = new Mock<IOptionsSnapshot<JwtSetting>>();
            mock.Setup(ap => ap.Value).Returns(opt);

            services.AddTransient((o) => mock.Object);

        }

        [Test]
        public async Task Test_Token()
        {
            var jwt = GetService<IJwtService>();
            var db = GetService<ApplicationDbContext>();
            
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == UserId);
            
            jwt.GetJwtToken(user);
        }
    }
}
