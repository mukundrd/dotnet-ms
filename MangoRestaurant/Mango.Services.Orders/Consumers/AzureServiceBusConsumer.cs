using AutoMapper;
using Azure.Messaging.ServiceBus;
using Mango.Contracts.Messages;
using Mango.Contracts.Models.Service;
using Mango.MessageBus.Consumers;
using Mango.MessageBus.Producers.Messages;
using Mango.Services.Orders.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Orders.Consumers
{
    public class AzureServiceBusConsumer : BaseAzureServiceBusConsumer
    {
        private readonly OrderRepository _orderRepository;

        private readonly IMapper _mapper;

        private readonly IMessageBus _messageBus;

        private readonly string OrderPaymentProcessTopic;

        private readonly ServiceBusProcessor _orderPaymentProcessProcessor;

        public AzureServiceBusConsumer(OrderRepository orderRepository, IMapper mapper, IMessageBus messageBus) : base()
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _messageBus = messageBus;
            OrderPaymentProcessTopic = GetValueForKey("OrderPaymentProcessTopic");

            _orderPaymentProcessProcessor = _servicebusClient.CreateProcessor(GetValueForKey("OrderUpdatePaymentResultTopic"), GetValueForKey("Subscription"));
        }

        public override async Task Start()
        {
            base.Start();
            _orderPaymentProcessProcessor.ProcessMessageAsync += OnOrderProcessPaymentResultReceived;
            _orderPaymentProcessProcessor.ProcessErrorAsync += Errorhandler;
            await _orderPaymentProcessProcessor.StartProcessingAsync();
        }

        public override async Task Stop()
        {
            base.Stop();
            _orderPaymentProcessProcessor.StopProcessingAsync();
            _orderPaymentProcessProcessor.DisposeAsync();
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

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = _mapper.Map<OrderHeader>(checkoutHeaderDto);
            List<OrderDetail> orderDetails = new List<OrderDetail>() { };
            foreach(var cartDetail in checkoutHeaderDto.CartDetails)
            {
                var orderDetail = _mapper.Map<OrderDetail>(cartDetail);
                orderDetail.Price = cartDetail.Count * cartDetail.Product.Price;
                orderDetails.Add(orderDetail);
                orderHeader.CartTotalItems += 1;
            }

            orderHeader.OrderDetails = orderDetails;
            orderHeader.OrderDateTime = DateTime.Now;

            await _orderRepository.AddOrder(orderHeader);

            PaymentRequestMessage paymentMessage = new PaymentRequestMessage()
            {
                Orderid = orderHeader.OrderHeaderId,
                Name = orderHeader.FirstName + " " + orderHeader.LastNaame,
                OrderTotal = orderHeader.OrderTotal,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonthYear = orderHeader.ExpiryMonthYear,
            };

            try
            {
                await _messageBus.PublishMessage(paymentMessage, OrderPaymentProcessTopic);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private async Task OnOrderProcessPaymentResultReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage paymentResult = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            if (paymentResult != null)
            {
                _orderRepository.UpdateOrderStatusPaymentStatus(paymentResult.Orderid, paymentResult.Paid);
            }
        }
    }
}
