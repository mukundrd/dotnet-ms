using Azure.Messaging.ServiceBus;
using Mango.Contracts.Connections;
using Mango.MessageBus.Consumers;

namespace Mango.Services.Orders.Consumer
{
    public abstract class BaseAzureServiceBusConsumer : IAzureServiceBusConsumer
    {

        private readonly ServiceBusProcessor _servicebusProcessor;

        public BaseAzureServiceBusConsumer()
        {
            var serviceBusConnection = Connections.GetConnectionStringFrom<string>("bus-connection.json", "ServiceBusConnection");
            var topic = Connections.GetConnectionStringFrom<string>("bus-connection.json", "Topic");
            var subscription = Connections.GetConnectionStringFrom<string>("bus-connection.json", "Subscription");

            var client = new ServiceBusClient(serviceBusConnection);
            _servicebusProcessor = client.CreateProcessor(topic, subscription);
        }

        public async Task Start()
        {
            _servicebusProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
            _servicebusProcessor.ProcessErrorAsync += Errorhandler;
            await _servicebusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            _servicebusProcessor.StopProcessingAsync();
            _servicebusProcessor.DisposeAsync();
        }

        protected abstract Task Errorhandler(ProcessErrorEventArgs arg);

        protected abstract Task OnCheckoutMessageReceived(ProcessMessageEventArgs args);
    }
}
