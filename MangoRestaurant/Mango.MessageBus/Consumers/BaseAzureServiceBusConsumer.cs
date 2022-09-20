using Azure.Messaging.ServiceBus;
using Mango.Contracts.Connections;
using Mango.MessageBus.Consumers;

namespace Mango.MessageBus.Consumers
{
    public abstract class BaseAzureServiceBusConsumer : IAzureServiceBusConsumer
    {

        private readonly ServiceBusProcessor _servicebusProcessor;

        protected readonly ServiceBusClient _servicebusClient;

        public BaseAzureServiceBusConsumer()
        {
            var serviceBusConnection = GetValueForKey("ServiceBusConnectionString");
            var topic = GetValueForKey("Topic");
            var subscription = GetValueForKey("Subscription");

            _servicebusClient = new ServiceBusClient(serviceBusConnection);
            _servicebusProcessor = _servicebusClient.CreateProcessor(topic, subscription);
        }

        public virtual async Task Start()
        {
            _servicebusProcessor.ProcessMessageAsync += OnMessageReceived;
            _servicebusProcessor.ProcessErrorAsync += Errorhandler;
            await _servicebusProcessor.StartProcessingAsync();
        }

        public virtual async Task Stop()
        {
            _servicebusProcessor.StopProcessingAsync();
            _servicebusProcessor.DisposeAsync();
        }

        protected string GetValueForKey(string key)
        {
            return Connections.GetConnectionStringFrom<string>("bus-connection.json", key);
        }

        protected abstract Task Errorhandler(ProcessErrorEventArgs arg);

        protected abstract Task OnMessageReceived(ProcessMessageEventArgs args);

    }

}
