namespace Mango.Contracts.Messages
{
    public class PaymentRequestMessage : BaseMessage
    {
        public int Orderid { get; set; }

        public string Name { get; set; }

        public string CardNumber { get; set; }

        public string CVV { get; set; }

        public string ExpiryMonthYear { get; set; }

        public Double OrderTotal { get; set; }
    }
}
