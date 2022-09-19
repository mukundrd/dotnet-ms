namespace Mango.Contracts.Messages
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();

        Task Stop();
    }
}
