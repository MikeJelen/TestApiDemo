using System;
using System.Threading.Tasks;
using TestKafka;

namespace TestApiDemo.Messaging
{
    public class KafkaMessaging : IMessaging
    {
        public string Consume(string serverUri, string topic, string groupId)
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
