using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
#if DEBUG
using TestApiDemo.Exceptions;
#endif

namespace TestApiDemo.Messaging
{
    public class MockMessaging : IMessaging
    {
        private readonly List<Tuple<string, string>> _messages = new List<Tuple<string, string>>();

        public MockMessaging()
        {
#if DEBUG
            LogManager.GetCurrentClassLogger().Info("==> IMessaging: MockMessaging");
#else
            throw new InvalidServiceException("MockMessaging is only available in DEBUG");
#endif
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
