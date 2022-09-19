using AutoMapper;
using Azure.Messaging.ServiceBus;
using Mango.Contracts.Connections;
using Mango.Contracts.Messages;
using Mango.Contracts.Models.Service;
using Mango.Services.Orders.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Orders.Consumer
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly OrderRepository _orderRepository;

        private readonly IMapper _mapper;

        private readonly ServiceBusProcessor _checkoutProcessor;

        public AzureServiceBusConsumer(OrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            var serviceBusConnection = Connections.GetConnectionStringFrom<string>("bus-connection.json", "ServiceBusConnection");
            var topic = Connections.GetConnectionStringFrom<string>("bus-connection.json", "Topic");
            var subscription = Connections.GetConnectionStringFrom<string>("bus-connection.json", "Subscription");

            var client = new ServiceBusClient(serviceBusConnection);
            _checkoutProcessor = client.CreateProcessor(topic, subscription);
        }

        public async Task Start()
        {
            _checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
            _checkoutProcessor.ProcessErrorAsync += Errorhandler;
            await _checkoutProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            _checkoutProcessor.StopProcessingAsync();
            _checkoutProcessor.DisposeAsync();
        }

        private Task Errorhandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckoutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = _mapper.Map<OrderHeader>(checkoutHeaderDto);
            IEnumerable<OrderDetail> orderDetails = _mapper.Map<IEnumerable<OrderDetail>>(checkoutHeaderDto.CartDetails);
            orderHeader.CartTotalItems = orderDetails.Count();
            orderHeader.OrderDetails = orderDetails;
            orderHeader.OrderDateTime = new DateTime();

            await _orderRepository.AddOrder(orderHeader);

        }
    }
}
