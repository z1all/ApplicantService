using Common.EasyNetQ.Logger.Receiver.Interfaces;
using EasyNetQ.AutoSubscribe;

namespace Common.EasyNetQ.Logger.Receiver
{
    public class ReceiverBackgroundListener : IConsumeAsync<LogDTO> 
    {
        private readonly IReceiverService _service;

        public ReceiverBackgroundListener(IReceiverService service)
        {
            _service = service;
        }

        public async Task ConsumeAsync(LogDTO message, CancellationToken cancellationToken = default)
        {
            await _service.Processing(message);
        }
    }
}
