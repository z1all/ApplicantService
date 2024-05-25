using LoggerService.Core.Application.Interfaces;
using LoggerService.Core.Application.Mapper;
using Common.EasyNetQ.Logger;
using Common.EasyNetQ.Logger.Receiver.Interfaces;

namespace LoggerService.Core.Application.Services
{
    public class LogService : IReceiverService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task Processing(LogDTO log)
        { 
           await _logRepository.AddAsync(log.ToLog());
        }
    }
}
