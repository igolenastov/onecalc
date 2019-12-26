using System;
using Microsoft.AspNetCore.NodeServices;
using System.IO;
using System.Threading.Tasks;
using OneCalc.Domain.Commands;
using OneCalc.Domain.Services;

namespace OneCalc.Infrastructure.Services
{
    public class CalculateService : ICalculateService
    {
        private readonly INodeServices _nodeServices;
        private readonly IHistoryCommand _historyCommand;

        public CalculateService(INodeServices nodeServices, IHistoryCommand historyCommand)
        {
            _nodeServices = nodeServices;
            _historyCommand = historyCommand;
        }
        public async Task<string> CalculateAsync(long userId, string input)
        {
            var fiilPath = AppContext.BaseDirectory;
            
            var result = await _nodeServices.InvokeAsync<string>(Path.Combine(fiilPath, "js/tools.js"), input);

            await _historyCommand.AddHistoryAsync(userId, input, result);

            return result;
        }
    }
}
