using System.Threading.Tasks;

namespace OneCalc.Domain.Commands
{
    public interface IHistoryCommand
    {
        Task AddHistoryAsync(long userId, string input, string result);
    }
}
