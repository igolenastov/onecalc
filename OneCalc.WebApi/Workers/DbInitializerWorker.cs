using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OneCalc.Domain.AppSettings;
using OneCalc.Domain.Entities;
using OneCalc.Domain.Enums;
using OneCalc.Domain.Security;
using OneCalc.Infrastructure.DataAccess;

namespace OneCalc.WebApi.Workers
{
    /// <summary>
    /// Воркер для автоматической миграции БД
    /// </summary>
    public class DbInitializerWorker: IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;

        /// <summary>
        /// Воркер для автоматической миграции БД
        /// </summary>
        /// <param name="serviceProvider"></param>
        public DbInitializerWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Проведение автоматической миграции в БД
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (_serviceScope = _serviceProvider.CreateScope())
            {
                var context = _serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                await MigrateAsync(context, cancellationToken);

                await InitRolesAsync(context, cancellationToken);

                await InitRootAdmin(context, cancellationToken);
            }
        }

        /// <summary>
        /// Миграция
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task MigrateAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            await context.Database.MigrateAsync(cancellationToken);
        }

        /// <summary>
        /// Добавляем роли в БД
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        private async Task InitRolesAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var roles = await context.Roles.ToListAsync(cancellationToken);

            foreach (RoleEnum role in Enum.GetValues(typeof(RoleEnum)))
            {
                if (roles.All(x => x.Role != role))
                {
                    var nameLower = role.GetType().GetMember(role.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.Description ?? string.Empty;

                    context.Roles.Add(new ApplicationRole
                    {
                        Name = nameLower,
                        Role = role,
                        CreatedAt = DateTime.UtcNow,
                        NormalizedName = nameLower.ToUpperInvariant()
                    });
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Добавляем админа в систему
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task InitRootAdmin(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var adminOptions = _serviceScope.ServiceProvider.GetService<IOptions<AdminUserOptions>>()?.Value;
            var userManager = _serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            if (adminOptions ==null)
                return;

            ApplicationRole adminRole = await context.Roles.FirstOrDefaultAsync(x => x.Role == RoleEnum.Administrator, cancellationToken);

            if (adminRole == null)
                return;

            ApplicationUser adminUser = context.Users.Include(x => x.Roles).FirstOrDefault(x => x.Roles.Any(y => y.RoleId == adminRole.Id));

            if (adminUser != null)
                return;

            adminUser = new ApplicationUser
            {
                AllowOperation = OperationEnum.Div | OperationEnum.Minus | OperationEnum.Mul | OperationEnum.Plus, 
                CreatedAt = DateTime.UtcNow,
                Email = adminOptions.Email,
                UserName = adminOptions.Email,
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            await userManager.CreateAsync(adminUser, adminOptions.Password);
            await userManager.AddToRoleAsync(adminUser, AuthorizationConstant.AuthorizedAdmin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;;
        }
    }
}
