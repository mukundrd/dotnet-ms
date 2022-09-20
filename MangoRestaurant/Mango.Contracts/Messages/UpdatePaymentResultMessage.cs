namespace Mango.Contracts.Messages
{
    public class UpdatePaymentResultMessage : BaseMessage
    {
        public int Orderid { get; set; }
        public bool Paid { get; set; }
    }
}
