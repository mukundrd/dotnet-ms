using Azure.Messaging.ServiceBus;
using Mango.Contracts.Messages;
using Mango.MessageBus.Consumers;
using Mango.MessageBus.Producers.Messages;
using Newtonsoft.Json;
using PaymentProcessor;
using System.Text;

namespace Mango.Services.Payment.Consumer
{
    public class AzureServiceBusConsumer : BaseAzureServiceBusConsumer
    {
        private readonly IMessageBus _messageBus;

        private readonly string OrderUpdatePaymentResultTopic;

        private readonly IProcessPayment _processPayment;

        public AzureServiceBusConsumer(IMessageBus messageBus, IProcessPayment processPayment) : base()
        {
            _messageBus = messageBus;
            _processPayment = processPayment;
            OrderUpdatePaymentResultTopic = GetValueForKey("OrderUpdatePaymentResultTopic");
        }

        protected override Task Errorhandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        protected override async Task OnMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentRequest = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            try
            {
                bool result = _processPayment.Process();
                UpdatePaymentResultMessage resultMessage = new UpdatePaymentResultMessage()
                {
                    Orderid = paymentRequest.Orderid,
                    Paid = result
                };
                await _messageBus.PublishMessage(resultMessage, OrderUpdatePaymentResultTopic);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
