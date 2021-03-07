using System.Threading;
using System.Threading.Tasks;
using CmgLogParser;
using Microsoft.Extensions.Hosting;

namespace CmgLogParser.Microservice
{
    public class LogParserService : BackgroundService
    {
        public LogParserService(ILogParser logParser)
        {
            
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}