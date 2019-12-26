using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneCalc.Domain.Entities;
using OneCalc.Domain.Queries;
using OneCalc.Infrastructure.DataAccess;

namespace OneCalc.Infrastructure.Queries
{
    public class HistoryQuery : IHistoryQuery
    {
        private readonly ApplicationDbContext _db;

        public HistoryQuery(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<History>> GetAllMyAsync(long userId)
        {
            return await _db
                .Histories
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
      
        public async Task<List<History>> GetAllAsync()
        {
            return await _db
                .Histories
                .AsNoTracking()
                .OrderBy(x => x.UserId)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
