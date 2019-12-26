using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OneCalc.Domain.Entities;
using OneCalc.Infrastructure.DataAccess;
using OneCalc.Test.Integration.Model;
using OneCalc.WebApi.Controllers.Users.V1.Requests;


namespace OneCalc.Test.Integration
{
    public class TestUsers : BaseTest
    {
        [Test]
        public async Task Test_Create_User_CheckToken()
        {
            var user = new CreateRequest
            {
                Email = "test@calctest.ru",
                Operations = "+,-,*,/",
                Password = "12345"
            };

            var result = await GetResultGateway<UserResponse>("/api/v1/users/create", user);

            result.ErrorCode.Should().Be(0);
            result.Token.Should().NotBeNullOrEmpty();

            var userId = result.UserId;

            var authorization = new AuthenticationHeaderValue("Bearer", result.Token);
            result = await GetResultGateway<UserResponse>("/api/v1/users/info", authorization);

            result.UserId.Should().Be(userId);
        }

        [Test]
        public async Task Test_Create_User_AuthCheck()
        {
            var user = new CreateRequest
            {
                Email = "testauth@calctest.ru",
                Operations = "+,-,*,/",
                Password = "12345"
            };

            var result = await GetResultGateway<UserResponse>("/api/v1/users/create", user);

            result.ErrorCode.Should().Be(0);
            result.Token.Should().NotBeNullOrEmpty();

            var userId = result.UserId;

            var authUser = new AuthenticateRequest()
            {
                Email = "testauth@calctest.ru",
                Password = "12345"
            };

            var tResult = await GetResultGateway<UserResponse>("/api/v1/users/token", authUser);

            tResult.UserId.Should().Be(userId);

            var authorization = new AuthenticationHeaderValue("Bearer", tResult.Token);
            var aResult = await GetResultGateway<UserResponse>("/api/v1/users/info", authorization);

            aResult.UserId.Should().Be(userId);
        }

        protected override async Task CleanAsync()
        {
            var db = GetService<ApplicationDbContext>();
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == "test@calctest.ru");
            var user1 = await db.Users.FirstOrDefaultAsync(x => x.Email == "testauth@calctest.ru");

            if (user != null)
                db.Users.Remove(user);

            if (user1 != null)
                db.Users.Remove(user1);

            await db.SaveChangesAsync();

        }
    }
}
