using System;
using System.Threading.Tasks;
using TestKafka;

namespace TestApiDemo.Helpers
{
    public class KafkaMessageHelper : IMessagingHelper
    {
        public string Consume(string serverUri, string topic)
        {
            throw new NotImplementedException();
        }

        public void Produce(string serverUri, string topic, string message)
        {
            _ = ProduceMessage(serverUri, topic, message);
        }

        private static async Task ProduceMessage(string serverUri, string topic, string message)
        {
            await MessageHandler.ProduceMessage(serverUri, topic, message);
        }
    }
}
