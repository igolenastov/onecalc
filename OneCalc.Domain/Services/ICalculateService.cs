using System.Threading.Tasks;

namespace OneCalc.Domain.Services
{
    public interface ICalculateService
    {
        Task<string> CalculateAsync(long userId, string input);
    }
}
