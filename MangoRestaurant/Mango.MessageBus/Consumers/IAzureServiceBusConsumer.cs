namespace Mango.MessageBus.Consumers
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();

        Task Stop();
    }
}
