using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestApiDemo.Messaging
{
    public class MockMessaging : IMessaging
    {
        private readonly List<Tuple<string, string>> _messages = new List<Tuple<string, string>>();

        public MockMessaging()
        {
            LogManager.GetCurrentClassLogger().Info("==> IMessaging: MockMessaging");
        }

        public string Consume(string serverUri, string topic, string groupId)
        {
            return string.Join("|", (_messages
                 .Where(m => m.Item1.Equals(topic))
                 .Select(m => m.Item2))
                 .ToArray());
        }

        public void Produce(string serverUri, string topic, string message)
        {
            _messages.Add(Tuple.Create(topic, message));
        }
    }
}
