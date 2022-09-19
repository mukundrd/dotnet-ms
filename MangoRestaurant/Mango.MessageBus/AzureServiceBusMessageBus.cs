using Azure.Messaging.ServiceBus;
using Mango.Contracts.Connections;
using Mango.Contracts.Messages;
using Newtonsoft.Json;
using System.Text;

namespace Mango.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {

        private readonly ServiceBusClient client;

        public AzureServiceBusMessageBus()
        {
            string connectionString = Connections.GetConnectionStringFrom("bus-connection.json", "DefaultConnection");
            client = new ServiceBusClient(connectionString);
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
