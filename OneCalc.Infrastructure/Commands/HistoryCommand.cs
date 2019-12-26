using System;
using System.Threading.Tasks;
using OneCalc.Domain.Commands;
using OneCalc.Domain.Entities;
using OneCalc.Infrastructure.DataAccess;

namespace OneCalc.Infrastructure.Commands
{
    public class HistoryCommand : IHistoryCommand
    {
        private readonly ApplicationDbContext _db;

        public HistoryCommand(ApplicationDbContext db)
        {
            _db = db;
        }
    
        public async Task AddHistoryAsync(long userId, string input, string result)
        {
            var entity = new History
            {
                UserId = userId,
                Input = input,
                Result = result,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Histories.AddAsync(entity);

            await _db.SaveChangesAsync();
        }
    }
}
