namespace Mango.Contracts.Models.Service
{
    public class Cart
    {
        public CartHeader CartHeader { get; set; }

        public IEnumerable<CartDetail> CartDetails { get; set; }
    }
}
