using System.Collections.Generic;
using System.Threading.Tasks;
using OneCalc.Domain.Entities;

namespace OneCalc.Domain.Queries
{
    public interface IUserQuery
    {
        Task<List<ApplicationUser>> GetAllAsync();
    }
}
