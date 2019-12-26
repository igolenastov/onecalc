using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using OneCalc.Domain.Entities;
using OneCalc.Domain.Services;
using OneCalc.Infrastructure.DataAccess;

namespace OneCalc.Test.Unit
{
    public class TestCalculate : BaseTest
    {
        private const long UserId = 1;

        protected override async Task InitDataAsync()
        {
            var db = GetService<ApplicationDbContext>();

            var user = new ApplicationUser
            {
                Id = UserId,
                Email = "test@test.com"
            };

            db.Users.Add(user);

            await db.SaveChangesAsync();
        }

        [Test]
        public async Task Test_Calculate()
        {
            var calc = GetService<ICalculateService>();
            var db = GetService<ApplicationDbContext>();

            var expression = "2 + 2 - 2";
            var result = await calc.CalculateAsync(UserId, expression);

            var history = db.Histories.FirstOrDefault(x => x.UserId == UserId);

            history.Should().NotBeNull();
            history.Input.Should().Be(expression);
            history.Result.Should().Be("2");
        }
    }
}
