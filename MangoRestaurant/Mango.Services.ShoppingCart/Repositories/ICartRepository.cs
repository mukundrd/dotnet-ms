using Mango.Contracts.Dtos;

namespace Mango.Services.ShoppingCart.Repositories
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserid(string userId);

        Task<CartDto> CreateUpdateCart(CartDto cartDto);

        Task<bool> RemoveFromCart(int cartDetailId);

        Task<bool> ClearCart(string userId);

        Task<bool> ApplyCoupon(string userId, string CouponCode);

        Task<bool> RemoveCoupon(string userId);
    }
}
