using Azure.Messaging.ServiceBus;
using Mango.Contracts.Connections;
using Mango.Contracts.Messages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Mango.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {

        private readonly ServiceBusClient client;

        public AzureServiceBusMessageBus(IConfiguration configuration)
        {
            string serviceBusConnectionString = Connections.GetConnectionStringFrom<string>("bus-connection.json", "ServiceBusConnectionString");
            client = new ServiceBusClient(serviceBusConnectionString);
        }

        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);
            var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);
            await sender.DisposeAsync();
        }
    }
}
