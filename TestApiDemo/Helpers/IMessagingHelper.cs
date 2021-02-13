using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApiDemo.Helpers
{
    public interface IMessagingHelper
    {
        void Produce(string serverUri, string topic, string message);
        string Consume(string serverUri, string topic);
    }
}
