using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OneCalc.Domain.Entities;
using OneCalc.Infrastructure.DataAccess;
using OneCalc.Test.Integration.Model;
using OneCalc.WebApi.Controllers.Calculate.V1.Requests;
using OneCalc.WebApi.Controllers.Users.V1.Requests;


namespace OneCalc.Test.Integration
{
    public class TestCalculate : BaseTest
    {
        [Test]
        public async Task Test_Create_User_Calculate_History()
        {
            var user = new CreateRequest
            {
                Email = "testv1@calctest.ru",
                Operations = "+,-,*,/",
                Password = "12345"
            };

            var result = await GetResultGateway<UserResponse>("/api/v1/users/create", user);

            result.ErrorCode.Should().Be(0);
            result.Token.Should().NotBeNullOrEmpty();
            var authorization = new AuthenticationHeaderValue("Bearer", result.Token);

            var calcRequest = new CalcRequest
            {
                Calculate = "5 + 2 * 100"
            };

            var calcResult = await GetResultGateway<CalcResponse>("/api/v1/calculate/execute", calcRequest, authorization);

            calcResult.Calculate.Should().Be(calcRequest.Calculate);
            calcResult.Result.Should().Be("205");

            var histroryResult = await GetResultGateway<List<CalcResponse>>("/api/v1/calculate/history", authorization);

            histroryResult.Count.Should().BeGreaterThan(0);
            histroryResult[0].Calculate.Should().Be(calcRequest.Calculate);
            histroryResult[0].Result.Should().Be("205");
        }

        [Test]
        public async Task Test_Create_User_CalculateV2_History()
        {
            var user = new CreateRequest
            {
                Email = "testv2@calctest.ru",
                Operations = "+,-,*,/,(,)",
                Password = "12345"
            };

            var result = await GetResultGateway<UserResponse>("/api/v1/users/create", user);

            result.ErrorCode.Should().Be(0);
            result.Token.Should().NotBeNullOrEmpty();
            var authorization = new AuthenticationHeaderValue("Bearer", result.Token);

            var calcRequest = new CalcRequest
            {
                Calculate = "(5 + 2) * 100"
            };

            var calcResult = await GetResultGateway<CalcResponse>("/api/v2/calculate/execute", calcRequest, authorization);

            calcResult.Calculate.Should().Be(calcRequest.Calculate);
            calcResult.Result.Should().Be("700");

            var histroryResult = await GetResultGateway<List<CalcResponse>>("/api/v1/calculate/history", authorization);

            histroryResult.Count.Should().BeGreaterThan(0);
            histroryResult[0].Calculate.Should().Be(calcRequest.Calculate);
            histroryResult[0].Result.Should().Be("700");
        }

        [Test]
        public async Task Test_Create_User_CalculateError()
        {
            var user = new CreateRequest
            {
                Email = "testerrv1@calctest.ru",
                Operations = "+,-,",
                Password = "12345"
            };

            var result = await GetResultGateway<UserResponse>("/api/v1/users/create", user);

            result.ErrorCode.Should().Be(0);
            result.Token.Should().NotBeNullOrEmpty();
            var authorization = new AuthenticationHeaderValue("Bearer", result.Token);

            var calcRequest = new CalcRequest
            {
                Calculate = "5 + 2 * 100"
            };

            var calcResult = await GetResultGateway<CalcResponse>("/api/v1/calculate/execute", calcRequest, authorization);

            calcResult.ErrorCode.Should().BeGreaterThan(0);
        }

        protected override async Task CleanAsync()
        {
            var db = GetService<ApplicationDbContext>();
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == "testv1@calctest.ru");
            var user1 = await db.Users.FirstOrDefaultAsync(x => x.Email == "testerrv1@calctest.ru");
            var user2 = await db.Users.FirstOrDefaultAsync(x => x.Email == "testv2@calctest.ru");

            if (user != null)
                db.Users.Remove(user);

            if (user1 != null)
                db.Users.Remove(user1);

            if (user2 != null)
                db.Users.Remove(user2);

            await db.SaveChangesAsync();
        }
    }
}
