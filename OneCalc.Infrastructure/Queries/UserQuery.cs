using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneCalc.Domain.Entities;
using OneCalc.Domain.Queries;
using OneCalc.Infrastructure.DataAccess;

namespace OneCalc.Infrastructure.Queries
{
    public class UserQuery : IUserQuery
    {
        private readonly ApplicationDbContext _db;

        public UserQuery(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _db
                .Users
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
