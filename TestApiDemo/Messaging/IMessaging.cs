namespace TestApiDemo.Messaging
{
    public interface IMessaging
    {
        void Produce(string serverUri, string topic, string message);
        string Consume(string serverUri, string topic, string groupId);
    }
}
