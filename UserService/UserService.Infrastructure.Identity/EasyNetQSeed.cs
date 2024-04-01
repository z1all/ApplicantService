using EasyNetQ.Topology;
using EasyNetQ;

namespace UserService.Infrastructure.Identity
{
    internal static class EasyNetQSeed
    {
        public static void AddQueue(IAdvancedBus advancedBus)
        {
            //// Нужно создать очередь для каждого уведомления
            //// 

            //var exchange = advancedBus.ExchangeDeclare("user.create", ExchangeType.Topic);

            //var queue = advancedBus.QueueDeclare("3test3_123");

            //var binding = advancedBus.Bind(exchange, queue, "#");
        }
    }
}
