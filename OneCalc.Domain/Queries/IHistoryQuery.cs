using System.Collections.Generic;
using System.Threading.Tasks;
using OneCalc.Domain.Entities;

namespace OneCalc.Domain.Queries
{
    public interface IHistoryQuery
    {
        Task<List<History>> GetAllMyAsync(long userId);
        
        Task<List<History>> GetAllAsync();
    }
}
