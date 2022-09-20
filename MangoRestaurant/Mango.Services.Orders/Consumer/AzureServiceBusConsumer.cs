using AutoMapper;
using Azure.Messaging.ServiceBus;
using Mango.Contracts.Messages;
using Mango.Contracts.Models.Service;
using Mango.Services.Orders.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Orders.Consumer
{
    public class AzureServiceBusConsumer : BaseAzureServiceBusConsumer
    {
        private readonly OrderRepository _orderRepository;

        private readonly IMapper _mapper;

        public AzureServiceBusConsumer(OrderRepository orderRepository, IMapper mapper) : base()
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        protected override Task Errorhandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        protected override async Task OnCheckoutMessageReceived(ProcessMessageEventArgs args)
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
