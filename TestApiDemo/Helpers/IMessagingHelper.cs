namespace TestApiDemo.Helpers
{
    public interface IMessagingHelper
    {
        void Produce(string serverUri, string topic, string message);
        string Consume(string serverUri, string topic, string groupId);
    }
}
